using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ms_crud_rest.DAO
{
    public class PedidoClienteDAO : GenericDAO<PedidoCliente>
    {
        public PedidoClienteDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override List<PedidoCliente> Listar(int idUsuarioParceiro)
        {
            try
            {
                List<PedidoCliente> listaPedidos = new List<PedidoCliente>();
                List<PedidoClienteEntidade> listaPedidosEntidade = new List<PedidoClienteEntidade>();

                List<ProdutoCliente> listaProdutos = new List<ProdutoCliente>();
                List<ProdutoClienteEntidade> listaProdutosEntidade = new List<ProdutoClienteEntidade>();

                sqlConn.StartConnection();

                //1 - busca o cabeçalho do pedido
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
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
                    pedido.Produtos = listaProdutos.Where(p => p.IdPedido == pedido.IdPedido).ToList();
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