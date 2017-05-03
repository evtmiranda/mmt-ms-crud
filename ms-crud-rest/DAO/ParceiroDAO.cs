using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class ParceiroDAO : GenericDAO<Parceiro>
    {
        public ParceiroDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        /// <summary>
        /// Faz a busca de um parceiro pelo id parceiro
        /// </summary>
        /// <param name="idParceiro">id do parceiro</param>
        /// <returns></returns>
        public Parceiro BuscarParceiro(int idParceiro)
        {
            Parceiro parceiro;
            List<ParceiroEntidade> listaParceirosEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_parceiro,
	                                                            id_loja,
	                                                            nm_parceiro,
	                                                            nm_descricao,
	                                                            id_endereco,
	                                                            nm_codigo,
	                                                            bol_ativo
                                                            FROM tab_parceiro
                                                            WHERE id_parceiro = @id_parceiro;");

                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", idParceiro);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaParceirosEntidade = new ModuloClasse().PreencheClassePorDataReader<ParceiroEntidade>(sqlConn.Reader);

                if (listaParceirosEntidade.Count == 0)
                    throw new ParceiroNaoEncontradoException();

                parceiro = listaParceirosEntidade[0].ToParceiro();

                return parceiro;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar os dados do parceiro", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }

        /// <summary>
        /// Faz a busca dos parceiros de uma determinada loja
        /// </summary>
        /// <param name="idLoja">id do parceiro</param>
        /// <returns></returns>
        public List<Parceiro> BuscarParceiroPorLoja(int idLoja)
        {
            List<Parceiro> listaParceiros = new List<Parceiro>();
            List<ParceiroEntidade> listaParceirosEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_parceiro,
	                                                            id_loja,
	                                                            nm_parceiro,
	                                                            nm_descricao,
	                                                            id_endereco,
	                                                            nm_codigo,
	                                                            bol_ativo
                                                            FROM tab_parceiro
                                                            WHERE id_loja = @id_loja;");

                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaParceirosEntidade = new ModuloClasse().PreencheClassePorDataReader<ParceiroEntidade>(sqlConn.Reader);

                if (listaParceirosEntidade.Count == 0)
                    throw new LojaNaoPossuiParceirosException();

                foreach (var parceiro in listaParceirosEntidade)
                {
                    listaParceiros.Add(parceiro.ToParceiro());
                }

                return listaParceiros;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar os parceiros", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }
    }
}