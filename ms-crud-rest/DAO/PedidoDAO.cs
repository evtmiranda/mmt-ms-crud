using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class PedidoDAO : GenericDAO<Pedido>
    {
        public PedidoDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        /// <summary>
        /// Faz o cadastro de um pedido
        /// 1 - insere o pedido
        /// 2 - insere os produtos do pedido
        /// 3 - insere as formas de pagamento do pedido
        /// </summary>
        /// <param name="pedido">objeto com as informações do pedido</param>
        /// <returns>id do pedido cadastrado</returns>
        public override int AdicionarPedido(Pedido pedido)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.BeginTransaction();

                int idPedido = 0;
                int idProdutoPedido = 0;

                //1 - insere o pedido
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_pedido(id_usuario_parceiro, dt_entrega, vlr_troco, nm_observacao)
                                                            VALUES(@id_usuario_parceiro, @dt_entrega, @vlr_troco, @nm_observacao); SELECT @@IDENTITY;");

                pedido.DataPedido = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd " + pedido.HorarioEntrega));

                //parametros do pedido
                sqlConn.Command.Parameters.AddWithValue("@id_usuario_parceiro", pedido.Cliente.Id);
                sqlConn.Command.Parameters.AddWithValue("@dt_entrega", pedido.DataPedido);
                sqlConn.Command.Parameters.AddWithValue("@vlr_troco", pedido.Troco);
                sqlConn.Command.Parameters.AddWithValue("@nm_observacao", pedido.Observacao);

                var varRetornoPedido = sqlConn.Command.ExecuteScalar();

                idPedido = Convert.ToInt32(varRetornoPedido);

                //verifica se o retorno foi positivo
                if (idPedido == 0)
                    throw new PedidoNaoCadastradoClienteException();

                //2 - insere os produtos do pedido
                //parâmetros dos produtos do pedido
                sqlConn.Command.Parameters.Clear();

                //faz um insert para cada produto
                foreach (var produto in pedido.ListaProdutos)
                {
                    //valor total do produto
                    //produto.ValorTotal = produto.Quantidade * produto.Produto.Valor;

                    sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_produto_pedido(id_produto, id_pedido, nr_qtd_produto, vlr_total_produto)
                                                            VALUES({0}, {1}, {2}, '{3}'); SELECT @@IDENTITY",
                                                            produto.Produto.Id, idPedido, produto.Quantidade, produto.ValorTotal.ToString().Replace(",", "."));
                    var varRetornoProdutoPedido = sqlConn.Command.ExecuteScalar();
                    idProdutoPedido = Convert.ToInt32(varRetornoProdutoPedido);

                    //verifica se o retorno foi positivo
                    if (idProdutoPedido == 0)
                        throw new PedidoNaoCadastradoClienteException();

                    //insere os itens adicionais dos produtos do pedido
                    foreach (var adicionaisProduto in produto.Produto.DadosAdicionaisProdutos)
                    {
                        sqlConn.Command.CommandText = "";

                        foreach (var itemAdicional in adicionaisProduto.ItensAdicionais)
                        {
                            if (itemAdicional.Qtd > 0)
                                sqlConn.Command.CommandText += string.Format(@"INSERT INTO tab_produto_adicional_pedido(id_pedido, id_produto_pedido, id_produto_adicional_item, qtd_item_adicional)
                                                        VALUES({0}, {1}, {2}, {3});",
                                                            idPedido, idProdutoPedido, itemAdicional.Id, itemAdicional.Qtd);
                        }

                        sqlConn.Command.ExecuteNonQuery();
                    }
                }

                //3 - insere as formas de pagamento do pedido
                foreach (var formaPagamento in pedido.ListaFormaPagamento)
                {

                    sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_forma_pagamento_pedido(id_forma_pagamento, id_pedido)
                                                                  VALUES({0}, {1});",
                                                            formaPagamento.Id, idPedido);
                    sqlConn.Command.ExecuteNonQuery();
                }

                //commita a transação
                sqlConn.Commit();

                //retorna o id do pedido
                return idPedido;
            }
            catch (PedidoNaoCadastradoClienteException)
            {
                sqlConn.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { Mensagem = "erro ao finalizar o pedido", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Monta uma lista com todos os pedidos de um determinado cliente
        /// </summary>
        /// <param name="idUsuarioParceiro">id do cliente</param>
        /// <returns></returns>
        public List<Pedido> ConsultarPedidosCliente(int idUsuarioParceiro)
        {
            try
            {
                List<Pedido> listaPedidos = new List<Pedido>();
                List<PedidoEntidade> listaPedidosEntidade = new List<PedidoEntidade>();

                List<ProdutoCliente> listaProdutos = new List<ProdutoCliente>();
                List<ProdutoClienteEntidade> listaProdutosEntidade = new List<ProdutoClienteEntidade>();

                sqlConn.StartConnection();

                //1 - busca o cabeçalho do pedido
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT DISTINCT
	                                                            tp.id_pedido,
	                                                            tp.dt_pedido
                                                            FROM tab_pedido AS tp
                                                            INNER JOIN tab_forma_pagamento_pedido AS tfp
                                                            ON tfp.id_pedido = tp.id_pedido
                                                            INNER JOIN tab_produto_pedido AS tpp
                                                            ON tpp.id_pedido = tp.id_pedido
                                                            INNER JOIN tab_produto AS tprod
                                                            ON tprod.id_produto = tpp.id_produto
                                                            WHERE tp.id_usuario_parceiro = @id_usuario_parceiro;");

                //parametros do pedido
                sqlConn.Command.Parameters.AddWithValue("@id_usuario_parceiro", idUsuarioParceiro);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaPedidosEntidade = new ModuloClasse().PreencheClassePorDataReader<PedidoEntidade>(sqlConn.Reader);

                //fecha o reader
                sqlConn.Reader.Close();

                //verifica se o retorno foi positivo
                if (listaPedidosEntidade.Count == 0)
                    throw new ClienteNuncaFezPedidosException();

                foreach (var pedidoEnt in listaPedidosEntidade)
                {
                    //listaPedidos.Add(pedidoEnt());
                }

                //2 - busca os produtos do pedido
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
                                                                tp.id_pedido,
	                                                            tprod.nm_produto,
	                                                            tpp.nr_qtd_produto
                                                            FROM tab_pedido AS tp
                                                            INNER JOIN tab_forma_pagamento_pedido AS tfp
                                                            ON tfp.id_pedido = tp.id_pedido
                                                            INNER JOIN tab_produto_pedido AS tpp
                                                            ON tpp.id_pedido = tp.id_pedido
                                                            INNER JOIN tab_produto AS tprod
                                                            ON tprod.id_produto = tpp.id_produto
                                                            WHERE tp.id_usuario_parceiro = @id_usuario_parceiro;");

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaProdutosEntidade = new ModuloClasse().PreencheClassePorDataReader<ProdutoClienteEntidade>(sqlConn.Reader);

                //fecha o reader
                sqlConn.Reader.Close();

                foreach (var produtoEnt in listaProdutosEntidade)
                {
                    listaProdutos.Add(produtoEnt.ToProdutoCliente());
                }

                //adiciona os produtos aos pedidos
                foreach (var pedido in listaPedidos)
                {
                    //pedido.Produtos = listaProdutos.Where(p => p.IdPedido == pedido.IdPedido).ToList();
                }

                //retorna a lista de pedidos
                return listaPedidos;
            }
            catch (ClienteNuncaFezPedidosException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao consultar os pedidos", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Busca todos os pedidos de uma determinada loja
        /// </summary>
        /// <param name="idLoja">Id da loja</param>
        /// <param name="ehPedidoFila">Diz se quer buscar os pedidos que estão na fila de entrega</param>
        /// <param name="ehPedidoAndamento">Diz se quer buscar os pedidos que estão em andamento</param>
        /// <param name="ehPedidoEntregue">Diz se quer buscar os pedidos que já foram entregues</param>
        /// <returns></returns>
        public List<PedidoCliente> ConsultarPedidosLoja(int idLoja, bool ehPedidoFila, bool ehPedidoAndamento, bool ehPedidoEntregue)
        {
            try
            {
                List<PedidoCliente> listaPedidos = new List<PedidoCliente>();
                List<PedidoClienteEntidade> listaPedidosEntidade = new List<PedidoClienteEntidade>();

                List<ProdutoCliente> listaProdutos = new List<ProdutoCliente>();
                List<ProdutoClienteEntidade> listaProdutosEntidade = new List<ProdutoClienteEntidade>();

                sqlConn.StartConnection();

                //1 - busca os dados do pedido
                //os dados adicionais, como produto, formas de pagamento e etc serão preenchidos posteriormente
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT DISTINCT
	                                                            tp.id_pedido,
	                                                            tp.dt_pedido
                                                            FROM tab_pedido AS tp
                                                            INNER JOIN tab_usuario_parceiro as tup
                                                            ON tp.id_usuario_parceiro = tup.id_usuario_parceiro
                                                            INNER JOIN tab_parceiro as tpedido
                                                            ON tup.id_parceiro = tpedido.id_parceiro
                                                            INNER JOIN tab_forma_pagamento_pedido AS tfp
                                                            ON tfp.id_pedido = tp.id_pedido
                                                            INNER JOIN tab_produto_pedido AS tpp
                                                            ON tpp.id_pedido = tp.id_pedido
                                                            INNER JOIN tab_produto AS tprod
                                                            ON tprod.id_produto = tpp.id_produto
                                                            WHERE tpedido.id_loja = @id_loja;");

                //parametros do pedido
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaPedidosEntidade = new ModuloClasse().PreencheClassePorDataReader<PedidoClienteEntidade>(sqlConn.Reader);

                //fecha o reader
                sqlConn.Reader.Close();

                //verifica se o retorno foi positivo
                if (listaPedidosEntidade.Count == 0)
                    throw new ClienteNuncaFezPedidosException();

                foreach (var pedidoEnt in listaPedidosEntidade)
                {
                    listaPedidos.Add(pedidoEnt.ToPedidoCliente());
                }

                //2 - busca os produtos do pedido
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
                                                                tp.id_pedido,
	                                                            tprod.nm_produto,
	                                                            tpp.nr_qtd_produto
                                                            FROM tab_pedido AS tp
                                                            INNER JOIN tab_forma_pagamento_pedido AS tfp
                                                            ON tfp.id_pedido = tp.id_pedido
                                                            INNER JOIN tab_produto_pedido AS tpp
                                                            ON tpp.id_pedido = tp.id_pedido
                                                            INNER JOIN tab_produto AS tprod
                                                            ON tprod.id_produto = tpp.id_produto
                                                            WHERE tp.id_usuario_parceiro = @id_usuario_parceiro;");

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaProdutosEntidade = new ModuloClasse().PreencheClassePorDataReader<ProdutoClienteEntidade>(sqlConn.Reader);

                //fecha o reader
                sqlConn.Reader.Close();

                foreach (var produtoEnt in listaProdutosEntidade)
                {
                    listaProdutos.Add(produtoEnt.ToProdutoCliente());
                }

                //adiciona os produtos aos pedidos
                foreach (var pedido in listaPedidos)
                {
                    //pedido.Produtos = listaProdutos.Where(p => p.IdPedido == pedido.IdPedido).ToList();
                }

                //retorna a lista de pedidos
                return listaPedidos;
            }
            catch (ClienteNuncaFezPedidosException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao consultar os pedidos", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
    }
}
