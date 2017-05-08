using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ms_crud_rest.DAO
{
    public class LojaDAO : GenericDAO<Loja>
    {
        public LojaDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        /// <summary>
        /// Busca uma loja pelo dominio de url
        /// </summary>
        /// <returns></returns>
        [Authorize]
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
                                                WHERE nm_dominio_loja = @nm_dominio_loja
                                                AND bol_ativo = 1";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@nm_dominio_loja", dominio);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaLojaEntidade = new ModuloClasse().PreencheClassePorDataReader<LojaEntidade>(sqlConn.Reader);

                //verifica se o retorno foi positivo
                if (listaLojaEntidade.Count == 0)
                    throw new LojaNaoEncontradaException();

                loja = listaLojaEntidade[0].ToLoja();

                //verifica se o retorno foi positivo
                if (loja.Id == 0)
                    throw new LojaNaoEncontradaException();

                return loja;
            }
            catch (LojaNaoEncontradaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar a loja", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
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