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
                int idPedido = 0;

                sqlConn.StartConnection();

                //1 - insere o pedido
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_pedido(id_usuario_parceiro, hr_entrega, vlr_troco, nm_observacao)
                                                            VALUES(@id_usuario_parceiro, @hr_entrega, @vlr_troco, @nm_observacao); SELECT @@IDENTITY;");

                //parametros do pedido
                sqlConn.Command.Parameters.AddWithValue("@id_usuario_parceiro", pedido.Cliente.Id);
                sqlConn.Command.Parameters.AddWithValue("@hr_entrega", pedido.HorarioEntrega);
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
                                                            VALUES({0}, {1}, {2}, '{3}');",
                                                            produto.Produto.Id, idPedido, produto.Quantidade, produto.ValorTotal.ToString().Replace(",","."));
                    sqlConn.Command.ExecuteNonQuery();
                }

                //3 - insere as formas de pagamento do pedido
                foreach (var formaPagamento in pedido.ListaFormaPagamento)
                {

                    sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_forma_pagamento_pedido(id_forma_pagamento, id_pedido)
                                                                  VALUES({0}, {1});",
                                                            formaPagamento.Id, idPedido);
                    sqlConn.Command.ExecuteNonQuery();
                }

                //retorna o id do pedido
                return idPedido;
            }
            catch (PedidoNaoCadastradoClienteException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao finalizar o pedido", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
    }
}
