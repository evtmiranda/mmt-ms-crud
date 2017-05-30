using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class FormaPagamentoDAO : GenericDAO<FormaDePagamento>
    {
        public FormaPagamentoDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override void Adicionar(FormaDePagamento formaPagamento)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = @"INSERT INTO tab_forma_pagamento(id_loja, nm_forma_pagamento, bol_ativo)
                                                    VALUES(@id_loja, @nm_forma_pagamento, @bol_ativo)";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", formaPagamento.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_forma_pagamento", formaPagamento.Nome);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", formaPagamento.Ativo);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = formaPagamento.IdLoja, Mensagem = "Erro ao cadastrar a forma de pagamento", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override FormaDePagamento BuscarPorId(int id, int idLoja)
        {
            FormaDePagamento pagamento;
            List<FormaDePagamentoEntidade> listaPagamentoEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = @"SELECT
	                                                id_forma_pagamento,
	                                                id_loja,
	                                                nm_forma_pagamento,
	                                                bol_ativo
                                                FROM tab_forma_pagamento
                                                WHERE id_forma_pagamento = @id_forma_pagamento";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_forma_pagamento", id);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if(sqlConn.Reader.HasRows)
                    listaPagamentoEntidade = new ModuloClasse().PreencheClassePorDataReader<FormaDePagamentoEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException();

                pagamento = listaPagamentoEntidade[0].ToFormaPagamento();

                return pagamento;
            }
            //sempre que for realizado uma busca por id, é necessário que o recurso existe. se o recurso não existir, é um erro interno, por este motivo
            //o log é gravado
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Forma de pagamento nao encontrada com id " + id, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar a forma de pagamento com id " + id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if(sqlConn.Reader != null)
                sqlConn.Reader.Close();
            }
        }

        public FormaDePagamento BuscarPorNome(string nome, int idLoja)
        {
            FormaDePagamento pagamento;
            List<FormaDePagamentoEntidade> listaPagamentoEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = @"SELECT
	                                                id_forma_pagamento,
	                                                id_loja,
	                                                nm_forma_pagamento,
	                                                bol_ativo
                                                FROM tab_forma_pagamento
                                                WHERE nm_forma_pagamento = @nm_forma_pagamento
                                                AND id_loja = @id_loja";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@nm_forma_pagamento", nome);
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listaPagamentoEntidade = new ModuloClasse().PreencheClassePorDataReader<FormaDePagamentoEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException();

                pagamento = listaPagamentoEntidade[0].ToFormaPagamento();

                return pagamento;
            }
            //sempre que for realizado uma busca por nome, é necessário que o recurso existe. se o recurso não existir, é um erro interno, por este motivo
            //o log é gravado
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Forma de pagamento nao encontrada com nome " + nome, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar a forma de pagamento com nome " + nome, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
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
                sqlConn.Command.CommandText = @"SELECT
	                                                id_forma_pagamento,
	                                                id_loja,
	                                                nm_forma_pagamento,
	                                                bol_ativo
                                                FROM tab_forma_pagamento
                                                WHERE id_loja = @id_loja";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listaPagamentoEntidade = new ModuloClasse().PreencheClassePorDataReader<FormaDePagamentoEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException();

                //transforma a entidade em objeto
                foreach (var pagamento in listaPagamentoEntidade)
                    listaPagamento.Add(pagamento.ToFormaPagamento());

                return listaPagamento;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao listar as formas de pagamento para a loja: " + idLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if(sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        public override void Atualizar(FormaDePagamento formaPagamento)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@nm_forma_pagamento", formaPagamento.Nome);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", formaPagamento.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_forma_pagamento", formaPagamento.Id);

                sqlConn.Command.CommandText = @"UPDATE tab_forma_pagamento
	                                                SET nm_forma_pagamento = @nm_forma_pagamento,
		                                                bol_ativo = @bol_ativo
                                                WHERE id_forma_pagamento = @id_forma_pagamento";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = formaPagamento.IdLoja, Mensagem = "Erro ao atualizar a forma de pagamento", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Excluir(FormaDePagamento formaPagamento)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_forma_pagamento", formaPagamento.Id);

                sqlConn.Command.CommandText = @"DELETE FROM tab_forma_pagamento
                                                WHERE id_forma_pagamento = @id_forma_pagamento";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = formaPagamento.IdLoja, Mensagem = "Erro ao excluir a forma de pagamento", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Desativar(FormaDePagamento formaPagamento)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", formaPagamento.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_forma_pagamento", formaPagamento.Id);

                sqlConn.Command.CommandText = @"UPDATE tab_forma_pagamento
	                                                SET bol_ativo = @bol_ativo
                                                WHERE id_forma_pagamento = @id_forma_pagamento";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = formaPagamento.IdLoja, Mensagem = "Erro ao desativar a forma de pagamento", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
    }
}
