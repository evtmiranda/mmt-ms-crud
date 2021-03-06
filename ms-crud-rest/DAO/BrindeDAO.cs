﻿using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;
using ClassesMarmitex.Utils;

namespace ms_crud_rest.DAO
{
    public class BrindeDAO : GenericDAO<Brinde>
    {
        public BrindeDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        #region brindes

        public override void Adicionar(Brinde brinde)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = @"INSERT INTO tab_brinde(id_loja, nm_brinde, nm_descricao, url_imagem, bol_ativo)
                                                VALUES(@id_loja, @nm_brinde, @nm_descricao, @url_imagem, @bol_ativo)";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", brinde.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_brinde", brinde.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", brinde.Descricao ?? "");
                sqlConn.Command.Parameters.AddWithValue("@url_imagem", brinde.Imagem);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", brinde.Ativo);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = brinde.IdLoja, Mensagem = "Erro ao cadastrar o brinde", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override Brinde BuscarPorId(int id, int idLoja)
        {
            Brinde brinde;
            List<BrindeEntidade> listaBrindeEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = @"SELECT
	                                                id_brinde,
                                                    id_loja,
	                                                nm_brinde,
	                                                nm_descricao,
	                                                url_imagem,
	                                                bol_ativo
                                                FROM tab_brinde
                                                WHERE id_brinde = @id_brinde
                                                AND bol_excluido = 0";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_brinde", id);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listaBrindeEntidade = new ModuloClasse().PreencheClassePorDataReader<BrindeEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException();

                brinde = listaBrindeEntidade[0].ToBrinde();

                return brinde;
            }
            //sempre que for realizado uma busca por id, é necessário que o recurso existe. se o recurso não existir, é um erro interno, por este motivo
            //o log é gravado
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Brinde nao encontrado com id " + id, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar o brinde com id " + id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        public override List<Brinde> Listar(int idLoja)
        {
            List<BrindeEntidade> listaBrindeEntidade = new List<BrindeEntidade>();
            List<Brinde> listaBrinde = new List<Brinde>();

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = @"SELECT
	                                                id_brinde,
                                                    id_loja,
	                                                nm_brinde,
	                                                nm_descricao,
	                                                url_imagem,
	                                                bol_ativo
                                                FROM tab_brinde
                                                WHERE id_loja = @id_loja
                                                AND bol_excluido = 0";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listaBrindeEntidade = new ModuloClasse().PreencheClassePorDataReader<BrindeEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException();

                //transforma a entidade em objeto
                foreach (var brinde in listaBrindeEntidade)
                    listaBrinde.Add(brinde.ToBrinde());

                return listaBrinde;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao listar os brindes para a loja: " + idLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        public override void Atualizar(Brinde brinde)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_brinde", brinde.Id);
                sqlConn.Command.Parameters.AddWithValue("@nm_brinde", brinde.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", brinde.Descricao);
                sqlConn.Command.Parameters.AddWithValue("@url_imagem", brinde.Imagem);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", brinde.Ativo);

                sqlConn.Command.CommandText = @"UPDATE tab_brinde
	                                                SET nm_brinde = @nm_brinde,
                                                        nm_descricao = @nm_descricao,
                                                        url_imagem = @url_imagem,
		                                                bol_ativo = @bol_ativo
                                                WHERE id_brinde = @id_brinde";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = brinde.IdLoja, Mensagem = "Erro ao atualizar o brinde", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Excluir(Brinde brinde)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.BeginTransaction();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_brinde", brinde.Id);

                sqlConn.Command.CommandText = @"UPDATE tab_brinde_parceiro
                                                    SET bol_excluido = 1, bol_ativo = 0
                                                WHERE id_brinde = @id_brinde;

                                                UPDATE tab_brinde
                                                    SET bol_excluido = 1, bol_ativo = 0
                                                WHERE id_brinde = @id_brinde";

                sqlConn.Command.ExecuteNonQuery();
                sqlConn.Commit();
            }
            catch (Exception ex)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { IdLoja = brinde.IdLoja, Mensagem = "Erro ao excluir o brinde", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Desativar(Brinde brinde)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.BeginTransaction();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_brinde", brinde.Id);

                sqlConn.Command.CommandText = @"DECLARE @ativo INT;
                                                SET @ativo = (SELECT bol_ativo FROM tab_brinde WHERE id_brinde = @id_brinde);

                                                UPDATE tab_brinde_parceiro
	                                                SET bol_ativo = CASE WHEN @ativo = 1 THEN 0 ELSE 0 END
                                                WHERE id_brinde = @id_brinde;

                                                UPDATE tab_brinde
	                                                SET bol_ativo = CASE WHEN @ativo = 1 THEN 0 ELSE 1 END
                                                WHERE id_brinde = @id_brinde;";

                sqlConn.Command.ExecuteNonQuery();
                sqlConn.Commit();
            }
            catch (Exception ex)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { IdLoja = brinde.IdLoja, Mensagem = "Erro ao desativar o brinde", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        #endregion

        #region brindes parceiros

        public void AdicionarBrindeParceiro(BrindeParceiro brinde)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.BeginTransaction();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = @"INSERT INTO tab_brinde_parceiro(id_parceiro, id_brinde, bol_ativo)
                                                VALUES(@id_parceiro, @id_brinde, @bol_ativo);

                                                -- se o mesmo parceiro tiver o mesmo brinde duas vezes, um é excluido e o outro fica ativo
                                                DECLARE @qtd INT;
                                                SET @qtd = (SELECT COUNT(1) FROM tab_brinde_parceiro WHERE id_parceiro = @id_parceiro AND id_brinde = @id_brinde);

                                                IF(@qtd > 1)
	                                                BEGIN
	
	                                                DECLARE @min_id_brinde INT;
	                                                SET @min_id_brinde = (SELECT MIN(id_brinde_parceiro) FROM tab_brinde_parceiro WHERE id_parceiro = @id_parceiro AND id_brinde = @id_brinde);

	                                                DELETE FROM tab_brinde_parceiro
	                                                WHERE id_brinde_parceiro = @min_id_brinde;

	                                                UPDATE tab_brinde_parceiro
	                                                SET bol_ativo = 1
	                                                WHERE id_parceiro = @id_parceiro AND id_brinde = @id_brinde;

	                                                END";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", brinde.IdParceiro);
                sqlConn.Command.Parameters.AddWithValue("@id_brinde", brinde.IdBrinde);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", brinde.Ativo);

                sqlConn.Command.ExecuteNonQuery();
                sqlConn.Commit();
            }
            catch (Exception ex)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { IdLoja = brinde.IdLoja, Mensagem = "Erro ao cadastrar o brinde parceiro", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public void ExcluirBrindeParceiro(BrindeParceiro brinde)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_brinde", brinde.Id);
                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", brinde.IdParceiro);

                sqlConn.Command.CommandText = @"UPDATE tab_brinde_parceiro
                                                    SET bol_excluido = 1, bol_ativo = 0
                                                WHERE id_brinde = @id_brinde
                                                AND id_parceiro = @id_parceiro;";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = brinde.IdLoja, Mensagem = "Erro ao excluir o brinde parceiro", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public void DesativarBrindeParceiro(BrindeParceiro brinde)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.BeginTransaction();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", brinde.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_brinde", brinde.Id);
                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", brinde.IdParceiro);

                sqlConn.Command.CommandText = @"DECLARE @ativo INT;
                                                SET @ativo = (SELECT bol_ativo FROM tab_brinde_parceiro WHERE id_brinde = @id_brinde AND bol_excluido = 0 AND id_parceiro = @id_parceiro);
                                                
                                                -- se ativar o brinde para o parceiro(tab_brinde_parceiro), o brinde(tab_brinde) também é ativado
                                                IF(@ativo = 0)
                                                    BEGIN
                                                        UPDATE tab_brinde
                                                            SET bol_ativo = 1
                                                        WHERE id_brinde = @id_brinde
                                                    END
                                                
                                                UPDATE tab_brinde_parceiro
	                                                SET bol_ativo = CASE WHEN @ativo = 1 THEN 0 ELSE 1 END
                                                WHERE id_brinde = @id_brinde
                                                AND bol_excluido = 0 
                                                AND id_parceiro = @id_parceiro;";

                sqlConn.Command.ExecuteNonQuery();
                sqlConn.Commit();
            }
            catch (Exception ex)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { IdLoja = brinde.IdLoja, Mensagem = "Erro ao desativar o brinde parceiro", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public DadosBrindeParceiro ListarPorParceiro(int idParceiro, int idLoja)
        {
            List<BrindeEntidade> listaBrindeEntidade = new List<BrindeEntidade>();
            List<Brinde> listaBrinde = new List<Brinde>();

            List<DadosBrindeParceiroEntidade> listaDadosBrindeParceiroEntidade = new List<DadosBrindeParceiroEntidade>();
            DadosBrindeParceiro dadosBrindeParceiro = new DadosBrindeParceiro();

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = @"SELECT
	                                                tb.id_brinde,
                                                    tb.id_loja,
	                                                tb.nm_brinde,
	                                                tb.nm_descricao,
	                                                tb.url_imagem,
	                                                tbp.bol_ativo
                                                FROM tab_brinde AS tb
                                                INNER JOIN tab_brinde_parceiro AS tbp
                                                ON tb.id_brinde = tbp.id_brinde
                                                WHERE tbp.id_parceiro = @id_parceiro
                                                AND tb.bol_excluido = 0
                                                AND tbp.bol_excluido = 0";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", idParceiro);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listaBrindeEntidade = new ModuloClasse().PreencheClassePorDataReader<BrindeEntidade>(sqlConn.Reader);

                sqlConn.Reader.Close();

                //transforma a entidade em objeto
                foreach (var brinde in listaBrindeEntidade)
                    listaBrinde.Add(brinde.ToBrinde());

                //busca os dados do parceiro
                sqlConn.Command.CommandText = @"SELECT
                                                    id_loja,
	                                                id_parceiro,
	                                                nm_parceiro
                                                FROM tab_parceiro
                                                WHERE id_parceiro = @id_parceiro
                                                AND bol_excluido = 0";

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listaDadosBrindeParceiroEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosBrindeParceiroEntidade>(sqlConn.Reader);
                else
                    throw new ParceiroNaoCadastradoException();

                dadosBrindeParceiro = listaDadosBrindeParceiroEntidade[0].ToDadosBrindeParceiro();
                dadosBrindeParceiro.Brindes = listaBrinde;

                return dadosBrindeParceiro;
            }
            catch (ParceiroNaoCadastradoException)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Tentativa de listar brindes de um parceiro que não existe ou não está ativo." + idParceiro, Descricao = "", StackTrace = "BrindeDAO/ListarPorParceiro" });
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao listar os brindes para o parceiro: " + idParceiro, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        #endregion
    }
}