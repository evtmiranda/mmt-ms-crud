using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class FormaPagamentoDAO : GenericDAO<FormaDePagamento>
    {
        public FormaPagamentoDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        /// <summary>
        /// Faz a busca de uma forma de pagamento
        /// </summary>
        /// <param name="idParceiro">id da forma de pagamento</param>
        /// <returns></returns>
        public override FormaDePagamento BuscarPorId(int id)
        {
            FormaDePagamento pagamento;
            List<FormaDePagamentoEntidade> listaPagamentoEntidade;

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
                                                            WHERE id_forma_pagamento = @id_forma_pagamento");

                sqlConn.Command.Parameters.AddWithValue("@id_forma_pagamento", id);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaPagamentoEntidade = new ModuloClasse().PreencheClassePorDataReader<FormaDePagamentoEntidade>(sqlConn.Reader);

                if (listaPagamentoEntidade.Count == 0)
                    throw new KeyNotFoundException();

                pagamento = listaPagamentoEntidade[0].ToFormaPagamento();

                //fecha o reader
                sqlConn.Reader.Close();

                return pagamento;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar a forma de pagamento", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }

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
                                                            WHERE id_loja = @id_loja");


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

        /// <summary>
        /// Faz o cadastro de uma forma de pagamento
        /// </summary>
        /// <param name="formaPagamento">dados</param>
        /// <returns></returns>
        public override void Adicionar(FormaDePagamento formaPagamento)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_forma_pagamento(id_loja, nm_forma_pagamento, bol_ativo)
                                                                VALUES(@id_loja, @nm_forma_pagamento, @bol_ativo);");

                sqlConn.Command.Parameters.AddWithValue("@id_loja", formaPagamento.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_forma_pagamento", formaPagamento.Nome);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", formaPagamento.Ativo);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao cadastrar a forma de pagamento", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Atualiza os dados de uma forma de pagametno
        /// </summary>
        /// <param name="formaPagamento">formaPagamento que será atualizada</param>
        /// <returns></returns>
        public override void Atualizar(FormaDePagamento formaPagamento)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.AddWithValue("@nm_forma_pagamento", formaPagamento.Nome);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", formaPagamento.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_forma_pagamento", formaPagamento.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_forma_pagamento
	                                                            SET nm_forma_pagamento = @nm_forma_pagamento,
		                                                            bol_ativo = @bol_ativo
                                                            WHERE id_forma_pagamento = @id_forma_pagamento;");

                sqlConn.Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar a forma de pagamento", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Seta uma forma de pagamento como inativo
        /// </summary>
        /// <param name="formaPagamento">formaPagamento que será inativada</param>
        /// <returns></returns>
        public override void Excluir(FormaDePagamento formaPagamento)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", formaPagamento.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_forma_pagamento", formaPagamento.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_forma_pagamento
	                                                            SET bol_ativo = @bol_ativo
                                                            WHERE id_forma_pagamento = @id_forma_pagamento;");

                sqlConn.Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar a forma de pagamento", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
    }
}
