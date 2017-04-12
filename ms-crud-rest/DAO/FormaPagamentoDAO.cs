using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class FormaPagamentoDAO : GenericDAO<FormaDePagamento>
    {
        public FormaPagamentoDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override List<FormaDePagamento> Listar(int idLoja)
        {
            List<FormaDePagamentoEntidade> listaPagamentoEntidade = new List<FormaDePagamentoEntidade>();
            List<FormaDePagamento> listaPagamento = new List<FormaDePagamento>();

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_forma_pagamento,
	                                                            id_loja,
	                                                            nm_forma_pagamento,
	                                                            bol_ativo
                                                            FROM tab_forma_pagamento
                                                            WHERE id_loja = @id_loja
                                                            AND bol_ativo = 1;");



                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaPagamentoEntidade = new ModuloClasse().PreencheClassePorDataReader<FormaDePagamentoEntidade>(sqlConn.Reader);

                //transforma a entidade em objeto
                foreach (var pagamento in listaPagamentoEntidade)
                    listaPagamento.Add(pagamento.ToFormaPagamento());

                //verifica se o retorno foi positivo
                if (listaPagamento.Count == 0)
                    throw new PagamentoNaoEncontradoException();


                return listaPagamento;
            }
            catch (PagamentoNaoEncontradoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao buscar as formas de pagamento", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }

        public FormaDePagamento BuscarPorNome(string nomeFormaPagamento, int idParceiro)
        {
            List<FormaDePagamentoEntidade> listaPagamentoEntidade = new List<FormaDePagamentoEntidade>();
            List<FormaDePagamento> listaPagamento = new List<FormaDePagamento>();

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            tfp.id_forma_pagamento,
	                                                            tfp.id_loja,
	                                                            tfp.nm_forma_pagamento,
	                                                            tfp.bol_ativo
                                                            FROM tab_forma_pagamento AS tfp
                                                            INNER JOIN tab_parceiro AS tp
                                                            ON tp.id_loja = tfp.id_loja
                                                            WHERE tp.id_parceiro = @id_parceiro
                                                            AND tfp.nm_forma_pagamento = @nm_forma_pagamento
                                                            AND tfp.bol_ativo = 1;");

                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", idParceiro);
                sqlConn.Command.Parameters.AddWithValue("@nm_forma_pagamento", nomeFormaPagamento);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaPagamentoEntidade = new ModuloClasse().PreencheClassePorDataReader<FormaDePagamentoEntidade>(sqlConn.Reader);

                //transforma a entidade em objeto
                foreach (var pagamento in listaPagamentoEntidade)
                    listaPagamento.Add(pagamento.ToFormaPagamento());

                //verifica se o retorno foi positivo
                if (listaPagamento.Count == 0)
                    throw new PagamentoNaoEncontradoException();


                return listaPagamento[0];
            }
            catch (PagamentoNaoEncontradoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao buscar os cardápios", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
                sqlConn.Command.Parameters.Clear();
            }
        }
    }
}
