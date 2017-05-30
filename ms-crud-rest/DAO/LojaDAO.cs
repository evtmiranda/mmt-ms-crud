using ClassesMarmitex;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ms_crud_rest.DAO
{
    public class LojaDAO : GenericDAO<Loja>
    {
        public LojaDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        [HttpGet]
        public Loja BuscarLoja(string dominio)
        {
            List<LojaEntidade> listaLojaEntidade = new List<LojaEntidade>();
            Loja loja = new Loja();

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = @"SELECT
	                                                id_loja,
	                                                id_rede,
	                                                nm_loja,
	                                                nm_dominio_loja,
	                                                id_endereco,
	                                                bol_ativo
                                                FROM tab_loja
                                                WHERE nm_dominio_loja = @nm_dominio_loja";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@nm_dominio_loja", dominio);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if(sqlConn.Reader.HasRows)
                    listaLojaEntidade = new ModuloClasse().PreencheClassePorDataReader<LojaEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException();

                loja = listaLojaEntidade[0].ToLoja();

                return loja;
            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = 0, Mensagem = "Loja não encontrada com o dominio: " + dominio, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = 0, Mensagem = "Erro ao buscar loja com o dominio: " + dominio, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if(sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

    }
}