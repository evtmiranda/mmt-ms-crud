using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public int AdicionarPedido(Pedido pedido)
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
                    throw new PedidoNaoCadastradoClienteException("Erro ao inserir os dados na tab_pedido");

                //2 - insere o status do pedido
                sqlConn.Command.Parameters.Clear();

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_pedido_status(id_pedido, id_status)
                                                              VALUES(@id_pedido, @id_status);");

                pedido.DataPedido = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd " + pedido.HorarioEntrega));

                //parametros do pedido
                sqlConn.Command.Parameters.AddWithValue("@id_pedido", idPedido);

                //status "na fila"
                sqlConn.Command.Parameters.AddWithValue("@id_status", 0);

                sqlConn.Command.ExecuteNonQuery();

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
                        throw new PedidoNaoCadastradoClienteException("Erro ao inserir os dados na tab_produto_pedido");

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

                        if (!string.IsNullOrEmpty(sqlConn.Command.CommandText))
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
            catch (PedidoNaoCadastradoClienteException pedEx)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { IdLoja = pedido.Parceiro.IdLoja, Mensagem = "Erro ao finalizar o pedido. Cliente: " + pedido.Cliente.Id, Descricao = pedEx.Message ?? "", StackTrace = pedEx.StackTrace ?? "" });
                throw;
            }
            catch (Exception ex)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { IdLoja = pedido.Parceiro.IdLoja, Mensagem = "Erro ao finalizar o pedido. Cliente: " + pedido.Cliente.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Busca todos os pedidos de um determinado cliente
        /// </summary>
        /// <param name="idUsuarioParceiro"></param>
        /// <returns></returns>
        public List<Pedido> ConsultarPedidosCliente(int idUsuarioParceiro, int idLoja)
        {
            List<Pedido> listaPedidos = new List<Pedido>();

            try
            {
                listaPedidos = ConsultarPedidos(idUsuarioParceiro);
            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Nenhum pedido encontrado para o cliente: " + idUsuarioParceiro, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao consultar os pedidos para o cliente: " + idUsuarioParceiro, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }

            return listaPedidos;
        }

        /// <summary>
        /// Busca todos os pedidos de uma determinada loja
        /// </summary>
        /// <param name="idLoja">Id da loja</param>
        /// <param name="ehDoDia">Diz se quer buscar os pedidos do dia</param>
        /// <param name="estadoPedido">Estado do pedido que deseja buscar. Na fila, em andamento ou entregue</param>
        /// <returns></returns>
        public List<Pedido> ConsultarPedidosLoja(int idLoja, bool ehDoDia, EstadoPedido estadoPedido)
        {
            List<Pedido> listaPedidos = new List<Pedido>();

            try
            {
                listaPedidos = ConsultarPedidos(0, idLoja, ehDoDia);

                if (estadoPedido == EstadoPedido.Fila)
                    listaPedidos = listaPedidos.FindAll(p => p.PedidoStatus.IdStatus == 0);

                if (estadoPedido == EstadoPedido.EmAndamento)
                    listaPedidos = listaPedidos.FindAll(p => p.PedidoStatus.IdStatus == 1);

                if (estadoPedido == EstadoPedido.Entregue)
                    listaPedidos = listaPedidos.FindAll(p => p.PedidoStatus.IdStatus == 2);

            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return listaPedidos;
        }

        /// <summary>
        /// Monta uma lista com todos os pedidos de um determinado cliente ou loja
        /// É necessário informar ou o id do parceiro ou o id da loja..
        /// </summary>
        /// <param name="idUsuarioParceiro">Id do usuário parceiro (cliente)</param>
        /// <param name="idLoja">id da loja</param>
        private List<Pedido> ConsultarPedidos(int idUsuarioParceiro = 0, int idLoja = 0, bool ehDoDia = false)
        {
            List<Pedido> listaPedidos = new List<Pedido>();
            List<PedidoEntidade> listaPedidosEntidade = new List<PedidoEntidade>();

            List<PedidoStatus> listaPedidoStatus = new List<PedidoStatus>();
            List<PedidoStatusEntidade> listaPedidoStatusEntidade = new List<PedidoStatusEntidade>();

            UsuarioParceiro usuarioParceiro = new UsuarioParceiro();
            UsuarioParceiroEntidade usuarioParceiroEnt = new UsuarioParceiroEntidade();

            Parceiro parceiro = new Parceiro();
            ParceiroEntidade parceiroEntidade = new ParceiroEntidade();

            Endereco endereco = new Endereco();
            EnderecoEntidade enderecoEntidade = new EnderecoEntidade();

            List<ProdutoPedido> listaProdutoPedido = new List<ProdutoPedido>();
            List<ProdutoPedidoEntidade> listaProdutoPedidoEntidade = new List<ProdutoPedidoEntidade>();

            Produto produto = new Produto();
            ProdutoEntidade produtoEntidade = new ProdutoEntidade();

            List<DadosProdutoAdicional> listaProdutosAdicionais = new List<DadosProdutoAdicional>();
            List<DadosProdutoAdicionalEntidade> listaProdutosAdicionaisEntidade = new List<DadosProdutoAdicionalEntidade>();

            List<DadosProdutoAdicionalItem> listaItensProdutosAdicionais = new List<DadosProdutoAdicionalItem>();
            List<DadosProdutoAdicionalItemEntidade> listaItensProdutosAdicionaisEntidade = new List<DadosProdutoAdicionalItemEntidade>();

            List<DadosProdutoAdicionalPedido> listaDadosProdutosAdicionaisPedido = new List<DadosProdutoAdicionalPedido>();
            List<DadosProdutoAdicionalPedidoEntidade> listaDadosProdutosAdicionaisPedidoEntidade = new List<DadosProdutoAdicionalPedidoEntidade>();

            List<FormaDePagamento> listaFormaPagamento = new List<FormaDePagamento>();
            List<FormaDePagamentoEntidade> listaFormaPagamentoEntidade = new List<FormaDePagamentoEntidade>();

            sqlConn.StartConnection();

            try
            {
                #region 1 - monta o pedido

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                if (idUsuarioParceiro > 0)
                {
                    sqlConn.Command.CommandText = string.Format(@"SELECT
                                                                    id_pedido,
                                                                    id_usuario_parceiro,
                                                                    dt_pedido,
                                                                    dt_entrega,
                                                                    vlr_troco,
                                                                    nm_observacao
                                                                FROM tab_pedido
                                                                WHERE 1 = 1");

                    if (ehDoDia)
                        sqlConn.Command.CommandText += " AND dt_entrega BETWEEN CONVERT(DATETIME,FORMAT(GETDATE(), 'yyyy-dd-MM 00:00')) AND CONVERT(DATETIME,FORMAT(GETDATE(), 'yyyy-dd-MM 23:59'))";

                    sqlConn.Command.CommandText += " AND id_usuario_parceiro = @id_usuario_parceiro";

                    sqlConn.Command.CommandText += " ORDER BY dt_entrega ASC";

                    //parametros do pedido
                    sqlConn.Command.Parameters.AddWithValue("@id_usuario_parceiro", idUsuarioParceiro);
                }
                else
                {
                    sqlConn.Command.CommandText = string.Format(@"SELECT
                                                                    tp.id_pedido,
                                                                    tp.id_usuario_parceiro,
                                                                    tp.dt_pedido,
                                                                    tp.dt_entrega,
                                                                    tp.vlr_troco,
                                                                    tp.nm_observacao
                                                                FROM tab_pedido AS tp
                                                                INNER JOIN tab_usuario_parceiro AS tup
                                                                ON tp.id_usuario_parceiro = tup.id_usuario_parceiro
                                                                INNER JOIN tab_parceiro AS tparc
                                                                ON tparc.id_parceiro = tup.id_parceiro
                                                                WHERE 1 = 1");

                    if (ehDoDia)
                        sqlConn.Command.CommandText += " AND dt_entrega BETWEEN CONVERT(DATETIME,FORMAT(GETDATE(), 'yyyy-dd-MM 00:00')) AND CONVERT(DATETIME,FORMAT(GETDATE(), 'yyyy-dd-MM 23:59'))";

                    sqlConn.Command.CommandText += " AND tparc.id_loja = @id_loja";

                    //parametros do pedido
                    sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);
                }

                sqlConn.Reader = sqlConn.Command.ExecuteReader();
                listaPedidosEntidade = new ModuloClasse().PreencheClassePorDataReader<PedidoEntidade>(sqlConn.Reader);

                //fecha o reader
                sqlConn.Reader.Close();

                //verifica se algum pedido foi encontrado
                if (listaPedidosEntidade.Count == 0)
                    throw new KeyNotFoundException("Nenhum pedido encontrado.");

                foreach (var pedidoEnt in listaPedidosEntidade)
                {
                    listaPedidos.Add(pedidoEnt.ToPedido());
                }

                #endregion

                //percorre pedido a pedido para preencher os objetos
                foreach (var pedido in listaPedidos)
                {
                    #region 1.0 preenche o "PedidoStatus" do pedido

                    sqlConn.Command.Parameters.Clear();
                    sqlConn.Command.CommandText = "";

                    sqlConn.Command.CommandText = @"SELECT
	                                                    id_status_pedido,
	                                                    id_pedido,
	                                                    id_status,
	                                                    dt_status,
	                                                    bol_ativo
                                                    FROM tab_pedido_status
                                                    WHERE id_pedido = @id_pedido;";

                    sqlConn.Command.Parameters.AddWithValue("@id_pedido", pedido.Id);

                    sqlConn.Reader = sqlConn.Command.ExecuteReader();

                    if (sqlConn.Reader.HasRows)
                        listaPedidoStatusEntidade = new ModuloClasse().PreencheClassePorDataReader<PedidoStatusEntidade>(sqlConn.Reader);

                    foreach (var statusEntidade in listaPedidoStatusEntidade)
                    {
                        listaPedidoStatus.Add(statusEntidade.ToPedidoStatus());
                    }

                    //fecha o reader
                    sqlConn.Reader.Close();

                    //adiciona o usuário parceiro(cliente) ao pedido
                    pedido.PedidoStatus = listaPedidoStatus.Where(p => p.Ativo == 1).FirstOrDefault();

                    #endregion

                    #region 1.1 - preenche o "UsuarioParceiro" do pedido

                    sqlConn.Command.Parameters.Clear();
                    sqlConn.Command.CommandText = "";

                    sqlConn.Command.CommandText = @"SELECT
                                                        tup.id_usuario_parceiro,
                                                        tup.id_parceiro,
                                                        tp.id_loja,
                                                        tup.nm_usuario,
                                                        tup.nm_apelido,
                                                        tup.nm_email,
                                                        tup.nm_celular,
                                                        tup.nm_senha,
                                                        tup.bol_ativo
                                                    FROM tab_usuario_parceiro AS tup
                                                    INNER JOIN tab_parceiro AS tp
                                                    ON tup.id_parceiro = tp.id_parceiro
                                                    WHERE tup.id_usuario_parceiro = @id_usuario_parceiro;";

                    sqlConn.Command.Parameters.AddWithValue("@id_usuario_parceiro", pedido.Cliente.Id);

                    sqlConn.Reader = sqlConn.Command.ExecuteReader();

                    if (sqlConn.Reader.HasRows)
                    {
                        usuarioParceiroEnt = new ModuloClasse().PreencheClassePorDataReader<UsuarioParceiroEntidade>(sqlConn.Reader).FirstOrDefault();
                        usuarioParceiro = usuarioParceiroEnt.ToUsuarioParceiro();
                    }

                    //fecha o reader
                    sqlConn.Reader.Close();

                    //adiciona o usuário parceiro(cliente) ao pedido
                    pedido.Cliente = usuarioParceiro;

                    #endregion

                    #region 1.2 - preenche o "Parceiro" do pedido

                    sqlConn.Command.Parameters.Clear();
                    sqlConn.Command.CommandText = "";

                    sqlConn.Command.CommandText = @"SELECT
                                                        id_parceiro,
                                                        id_loja,
                                                        nm_parceiro,
                                                        nm_descricao,
                                                        id_endereco,
                                                        nm_codigo,
                                                        bol_ativo
                                                    FROM tab_parceiro
                                                    WHERE id_parceiro = @id_parceiro;";

                    sqlConn.Command.Parameters.AddWithValue("@id_parceiro", pedido.Cliente.IdParceiro);

                    sqlConn.Reader = sqlConn.Command.ExecuteReader();

                    if (sqlConn.Reader.HasRows)
                    {
                        parceiroEntidade = new ModuloClasse().PreencheClassePorDataReader<ParceiroEntidade>(sqlConn.Reader).FirstOrDefault();
                        parceiro = parceiroEntidade.ToParceiro();
                    }

                    //fecha o reader
                    sqlConn.Reader.Close();

                    //adiciona o parceiro(empresa onde o cliente trabalha) ao pedido
                    pedido.Parceiro = parceiro;

                    #endregion

                    #region 1.3 - preenche o "Endereco" do parceiro do pedido

                    sqlConn.Command.Parameters.Clear();
                    sqlConn.Command.CommandText = "";

                    sqlConn.Command.CommandText = @"SELECT
                                                        id_endereco,
                                                        nm_cep,
                                                        nm_uf,
                                                        nm_cidade,
                                                        nm_bairro,
                                                        nm_logradouro,
                                                        nm_numero_endereco,
                                                        nm_complemento_endereco
                                                    FROM tab_endereco
                                                    WHERE id_endereco = @id_endereco;";

                    sqlConn.Command.Parameters.AddWithValue("@id_endereco", pedido.Parceiro.Endereco.Id);

                    sqlConn.Reader = sqlConn.Command.ExecuteReader();

                    if (sqlConn.Reader.HasRows)
                    {
                        enderecoEntidade = new ModuloClasse().PreencheClassePorDataReader<EnderecoEntidade>(sqlConn.Reader).FirstOrDefault();
                        endereco = enderecoEntidade.ToEndereco();
                    }

                    //fecha o reader
                    sqlConn.Reader.Close();

                    //adiciona o endereco do parceiro
                    pedido.Parceiro.Endereco = endereco;

                    #endregion

                    #region 1.4 - preenche a lista de "ProdutoPedido"

                    sqlConn.Command.Parameters.Clear();
                    sqlConn.Command.CommandText = "";

                    sqlConn.Command.CommandText = @"SELECT
                                                        id_produto_pedido,
                                                        id_produto,
                                                        id_pedido,
                                                        nr_qtd_produto,
                                                        vlr_total_produto
                                                    FROM tab_produto_pedido
                                                    WHERE id_pedido = @id_pedido;";

                    sqlConn.Command.Parameters.AddWithValue("@id_pedido", pedido.Id);

                    sqlConn.Reader = sqlConn.Command.ExecuteReader();
                    listaProdutoPedidoEntidade = new ModuloClasse().PreencheClassePorDataReader<ProdutoPedidoEntidade>(sqlConn.Reader);

                    //preenche a lista de produto pedido
                    listaProdutoPedido = new List<ProdutoPedido>();
                    foreach (var produtoPedidoEntidade in listaProdutoPedidoEntidade)
                    {
                        listaProdutoPedido.Add(produtoPedidoEntidade.ToProdutoPedido());
                    }

                    //fecha o reader
                    sqlConn.Reader.Close();

                    #region 1.4.1 - preenche os produtos dos "ProdutosPedido"
                    foreach (var produtoPedido in listaProdutoPedido)
                    {
                        sqlConn.Command.Parameters.Clear();
                        sqlConn.Command.CommandText = "";

                        sqlConn.Command.CommandText = @"SELECT
                                                            id_produto,
                                                            id_menu_cardapio,
                                                            nm_produto,
                                                            nm_descricao,
                                                            vlr_produto,
                                                            url_imagem,
                                                            bol_ativo
                                                        FROM tab_produto
                                                        WHERE id_produto = @id_produto;";

                        sqlConn.Command.Parameters.AddWithValue("@id_produto", produtoPedido.Produto.Id);

                        sqlConn.Reader = sqlConn.Command.ExecuteReader();

                        if (sqlConn.Reader.HasRows)
                        {
                            produtoEntidade = new ModuloClasse().PreencheClassePorDataReader<ProdutoEntidade>(sqlConn.Reader).FirstOrDefault();
                            produto = produtoEntidade.ToProduto();
                        }

                        //adiciona o produto ao produto pedido
                        produtoPedido.Produto = produto;

                        //fecha o reader
                        sqlConn.Reader.Close();

                        #endregion

                        #region 1.4.2 - preenche os produtos adicionais dos produtos

                        sqlConn.Command.Parameters.Clear();
                        sqlConn.Command.CommandText = "";

                        sqlConn.Command.CommandText = @"SELECT
                                                            tpa.id_produto_adicional,
                                                            tpa.id_loja,
                                                            tpa.nm_adicional,
                                                            tpa.nm_descricao,
                                                            tpap.nr_qtd_min,
                                                            tpap.nr_qtd_max,
                                                            tpap.nr_ordem_exibicao,
                                                            tpa.bol_ativo
                                                        FROM tab_produto_adicional AS tpa
                                                        INNER JOIN tab_produto_adicional_produto AS tpap
                                                        ON tpa.id_produto_adicional = tpap.id_produto_adicional
                                                        WHERE tpap.id_produto = @id_produto;";

                        sqlConn.Command.Parameters.AddWithValue("@id_produto", produtoPedido.Produto.Id);

                        sqlConn.Reader = sqlConn.Command.ExecuteReader();

                        if (sqlConn.Reader.HasRows)
                        {
                            listaProdutosAdicionaisEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalEntidade>(sqlConn.Reader);
                        }

                        //fecha o reader
                        sqlConn.Reader.Close();

                        //transforma a entidade em objeto
                        listaProdutosAdicionais = new List<DadosProdutoAdicional>();
                        foreach (var produtoAdicionalEntidade in listaProdutosAdicionaisEntidade)
                        {
                            listaProdutosAdicionais.Add(produtoAdicionalEntidade.ToProdutoAdicional());
                        }

                        #endregion

                        #region 1.4.3 - preenche os itens dos produtos adicionais do produto

                        foreach (var produtoAdicional in listaProdutosAdicionais)
                        {
                            sqlConn.Command.Parameters.Clear();
                            sqlConn.Command.CommandText = "";

                            sqlConn.Command.CommandText = @"SELECT
                                                                id_produto_adicional_item,
                                                                id_produto_adicional,
                                                                nm_adicional_item,
                                                                nm_descricao_item,
                                                                vlr_adicional_item,
                                                                bol_ativo
                                                            FROM tab_produto_adicional_item
                                                            WHERE id_produto_adicional = @id_produto_adicional;";

                            sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional", produtoAdicional.Id);

                            sqlConn.Reader = sqlConn.Command.ExecuteReader();

                            if (sqlConn.Reader.HasRows)
                            {
                                listaItensProdutosAdicionaisEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalItemEntidade>(sqlConn.Reader);
                            }

                            //fecha o reader
                            sqlConn.Reader.Close();

                            //transforma a entidade em objeto
                            listaItensProdutosAdicionais = new List<DadosProdutoAdicionalItem>();
                            foreach (var itemProdutoAdicionalEntidade in listaItensProdutosAdicionaisEntidade)
                            {
                                listaItensProdutosAdicionais.Add(itemProdutoAdicionalEntidade.ToProdutoAdicionalItem());
                            }

                            #endregion

                            #region 1.4.4 - atualiza a quantidade dos itens adicionais
                            //busca todos os itens dos produtos adicionais do pedido

                            sqlConn.Command.Parameters.Clear();
                            sqlConn.Command.CommandText = "";

                            sqlConn.Command.CommandText = @"SELECT
                                                                id,
                                                                id_pedido,
                                                                id_produto_pedido,
                                                                id_produto_adicional_item,
                                                                qtd_item_adicional
                                                            FROM tab_produto_adicional_pedido
                                                            WHERE id_pedido = @id_pedido
                                                            AND id_produto_pedido = @id_produto_pedido";

                            sqlConn.Command.Parameters.AddWithValue("@id_pedido", pedido.Id);
                            sqlConn.Command.Parameters.AddWithValue("@id_produto_pedido", produtoPedido.Id);

                            sqlConn.Reader = sqlConn.Command.ExecuteReader();

                            if (sqlConn.Reader.HasRows)
                            {
                                listaDadosProdutosAdicionaisPedidoEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalPedidoEntidade>(sqlConn.Reader);
                            }

                            //fecha o reader
                            sqlConn.Reader.Close();

                            //transforma a entidade em objeto
                            listaDadosProdutosAdicionaisPedido = new List<DadosProdutoAdicionalPedido>();
                            foreach (var dadosProdutosAdicionaisPedidoEntidade in listaDadosProdutosAdicionaisPedidoEntidade)
                            {
                                listaDadosProdutosAdicionaisPedido.Add(dadosProdutosAdicionaisPedidoEntidade.ToProdutoAdicionalPedido());
                            }

                            //atualiza a quantidade de itens adicionais
                            foreach (var itemProdutoAdicional in listaItensProdutosAdicionais)
                            {
                                if (listaDadosProdutosAdicionaisPedido.Where(p => p.IdProdutoAdicionalItem == itemProdutoAdicional.Id && p.IdProdutoPedido == produtoPedido.Id).Count() == 0)
                                    continue;

                                itemProdutoAdicional.Qtd = listaDadosProdutosAdicionaisPedido.Where(p => p.IdProdutoAdicionalItem == itemProdutoAdicional.Id && p.IdProdutoPedido == produtoPedido.Id).
                                SingleOrDefault().QtdItemAdicional;
                            }

                            //adiciona os produtos adicionais ao produto
                            produtoAdicional.ItensAdicionais = listaItensProdutosAdicionais;

                            #endregion

                        }

                        //atualiza os dados adicionais do produto
                        produto.DadosAdicionaisProdutos = listaProdutosAdicionais;

                    }


                    //adiciona a lista de produtoPedido ao pedido
                    pedido.ListaProdutos = listaProdutoPedido;

                    #endregion

                    #region 1.5 - preenche a lista de "Forma de pagamento"

                    sqlConn.Command.Parameters.Clear();
                    sqlConn.Command.CommandText = "";

                    sqlConn.Command.CommandText = @"SELECT
                                                        tfp.id_forma_pagamento,
                                                        tfp.id_loja,
                                                        tfp.nm_forma_pagamento,
                                                        tfp.bol_ativo
                                                    FROM tab_forma_pagamento AS tfp
                                                    INNER JOIN tab_forma_pagamento_pedido AS tfpp
                                                    ON tfp.id_forma_pagamento = tfpp.id_forma_pagamento
                                                    WHERE tfpp.id_pedido = @id_pedido;";

                    sqlConn.Command.Parameters.AddWithValue("@id_pedido", pedido.Id);

                    sqlConn.Reader = sqlConn.Command.ExecuteReader();

                    if (sqlConn.Reader.HasRows)
                    {
                        listaFormaPagamentoEntidade = new ModuloClasse().PreencheClassePorDataReader<FormaDePagamentoEntidade>(sqlConn.Reader);
                    }

                    listaFormaPagamento = new List<FormaDePagamento>();

                    foreach (var formaPagamentoEntidade in listaFormaPagamentoEntidade)
                    {
                        listaFormaPagamento.Add(formaPagamentoEntidade.ToFormaPagamento());
                    }

                    //fecha o reader
                    sqlConn.Reader.Close();

                    //adiciona as formas de pagamento ao pedido
                    pedido.ListaFormaPagamento = listaFormaPagamento;

                    #endregion
                }

                //retorna a lista de pedidos
                return listaPedidos;
            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Nenhum pedido encontrado para a loja ou usuário parceiro. Usuário parceiro: " + idUsuarioParceiro, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar os pedidos para a loja ou usuário parceiro. Usuário parceiro: " + idUsuarioParceiro, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }

        public override Pedido BuscarPorId(int idPedido, int idLoja)
        {
            PedidoEntidade pedidoEntidade = new PedidoEntidade();
            Pedido pedido = new Pedido();

            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = string.Format(@"SELECT
                                                                    id_pedido,
                                                                    id_usuario_parceiro,
                                                                    dt_pedido,
                                                                    dt_entrega,
                                                                    vlr_troco,
                                                                    nm_observacao
                                                                FROM tab_pedido
                                                                WHERE 1 = 1");

                sqlConn.Command.CommandText += " AND id_pedido = @id_pedido";

                //parametros do pedido
                sqlConn.Command.Parameters.AddWithValue("@id_pedido", idPedido);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();
                pedidoEntidade = new ModuloClasse().PreencheClassePorDataReader<PedidoEntidade>(sqlConn.Reader).FirstOrDefault();

                pedido = pedidoEntidade.ToPedido();

                //fecha o reader
                sqlConn.Reader.Close();

                if (pedido == null)
                    throw new KeyNotFoundException();

                return pedido;

            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Pedido nao encontrado com id " + idPedido, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar o pedido com id " + idPedido, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
        }

        /// <summary>
        /// seta todos os status do pedido como inativo e insere o status atual
        /// </summary>
        /// <param name="p">Pedido</param>
        public void AtualizarStatusPedido(Pedido p)
        {
            try
            {
                //abre o begin transaction
                sqlConn.BeginTransaction();
                sqlConn.StartConnection();

                //monta os parâmetros que serão utilizados
                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_pedido", p.Id);
                sqlConn.Command.Parameters.AddWithValue("@id_status", p.PedidoStatus.IdStatus);

                //seta o tipo do comando
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region seta todos os status do pedido como inativo
                
                //atualiza todos os status do pedido como inativo
                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_pedido_status
                                                                SET bol_ativo = 0
                                                              WHERE id_pedido = @id_pedido");

                //roda o update
                sqlConn.Command.ExecuteNonQuery();

                #endregion

                #region insere o novo status do pedido

                //monta o insert
                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_pedido_status(id_pedido, id_status)
                                                              VALUES(@id_pedido, @id_status);");


                //roda o insert
                sqlConn.Command.ExecuteNonQuery();

                #endregion

                //se correr tudo bem, roda o commit
                sqlConn.Commit();
            }
            catch (Exception ex)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { IdLoja = p.Parceiro.IdLoja, Mensagem = "Erro ao atualizar o pedido com id " + p.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
        }

    }
}
