using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ms_crud_rest.DAO
{
    public class ParceiroDAO : GenericDAO<Parceiro>
    {
        LojaDAO lojaDAO;
        public ParceiroDAO(SqlServer sqlConn, LogDAO logDAO, LojaDAO lojaDAO) : base(sqlConn, logDAO)
        {
            this.lojaDAO = lojaDAO;
        }

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
        /// Faz a busca de um parceiro pelo nome
        /// </summary>
        /// <param name="idParceiro">id do parceiro</param>
        /// <returns></returns>
        public Parceiro BuscarParceiro(string nomeParceiro)
        {
            Parceiro parceiro = new Parceiro();
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
                                                            WHERE nm_parceiro = @nm_parceiro;");

                sqlConn.Command.Parameters.AddWithValue("@nm_parceiro", nomeParceiro);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaParceirosEntidade = new ModuloClasse().PreencheClassePorDataReader<ParceiroEntidade>(sqlConn.Reader);

                if (listaParceirosEntidade != null && listaParceirosEntidade.Count > 0)
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
                sqlConn.Reader.Close();

                if (listaParceirosEntidade.Count == 0)
                    throw new LojaNaoPossuiParceirosException();

                foreach (var parceiro in listaParceirosEntidade)
                {

                    
                    try
                    {
                        #region busca o endereco

                        List<EnderecoEntidade> endEntidade = new List<EnderecoEntidade>();
                        Endereco endereco = new Endereco();

                        sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_endereco,
	                                                            nm_cep,
	                                                            nm_uf,
	                                                            nm_cidade,
	                                                            nm_bairro,
	                                                            nm_logradouro,
	                                                            nm_numero_endereco,
	                                                            nm_complemento_endereco
                                                            FROM tab_endereco
                                                            WHERE id_endereco = @id_endereco;");

                        sqlConn.Command.Parameters.Clear();
                        sqlConn.Command.Parameters.AddWithValue("@id_endereco", parceiro.IdEndereco);

                        sqlConn.Reader = sqlConn.Command.ExecuteReader();

                        endEntidade = new ModuloClasse().PreencheClassePorDataReader<EnderecoEntidade>(sqlConn.Reader);

                        if (endEntidade.Count == 0)
                            throw new Exception("Não foi possível consultar o endereço do parceiro: " + parceiro.Nome);

                        endereco = endEntidade.FirstOrDefault().ToEndereco();

                        Parceiro parceiroComEndereco = new Parceiro();
                        parceiroComEndereco = parceiro.ToParceiro();
                        parceiroComEndereco.Endereco = endereco;

                        #endregion

                        listaParceiros.Add(parceiroComEndereco);
                    }
                    finally
                    {
                        sqlConn.Reader.Close();
                    }
                    
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

        /// <summary>
        /// Faz o cadastro de um parceiro
        /// </summary>
        /// <param name="parceiro">dados do parceiro</param>
        /// <returns></returns>
        public void AdicionarParceiro(string urlLoja, Parceiro parceiro)
        {
            try
            {

                int idEndereco = 0;

                #region busca o id da loja

                Loja loja = new Loja();

                loja = lojaDAO.BuscarLoja(urlLoja);

                if (loja == null)
                    throw new Exception("Não foi possível identificar a loja");

                #endregion

                sqlConn.StartConnection();
                sqlConn.BeginTransaction();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region adiciona o endereco

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_endereco(nm_cep, nm_uf, nm_cidade, nm_bairro, nm_logradouro, nm_numero_endereco, nm_complemento_endereco)
                                                              VALUES(@nm_cep, @nm_uf, @nm_cidade, @nm_bairro, @nm_logradouro, @nm_numero_endereco, @nm_complemento_endereco); SELECT @@identity");

                sqlConn.Command.Parameters.AddWithValue("@nm_cep", parceiro.Endereco.Cep);
                sqlConn.Command.Parameters.AddWithValue("@nm_uf", parceiro.Endereco.UF);
                sqlConn.Command.Parameters.AddWithValue("@nm_cidade", parceiro.Endereco.Cidade);
                sqlConn.Command.Parameters.AddWithValue("@nm_bairro", parceiro.Endereco.Bairro);
                sqlConn.Command.Parameters.AddWithValue("@nm_logradouro", parceiro.Endereco.Logradouro);
                sqlConn.Command.Parameters.AddWithValue("@nm_numero_endereco", parceiro.Endereco.NumeroEndereco);
                sqlConn.Command.Parameters.AddWithValue("@nm_complemento_endereco", parceiro.Endereco.ComplementoEndereco);

                idEndereco = Convert.ToInt32(sqlConn.Command.ExecuteScalar());

                if(idEndereco == 0)
                    throw new Exception("Não foi cadastrar o endereço");

                #endregion

                #region adiciona o parceiro

                sqlConn.Command.Parameters.Clear();

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_parceiro(id_loja, nm_parceiro, nm_descricao, id_endereco, nm_codigo)
                                                              VALUES(@id_loja, @nm_parceiro, @nm_descricao, @id_endereco, @nm_codigo)");

                sqlConn.Command.Parameters.AddWithValue("@id_loja", loja.Id);
                sqlConn.Command.Parameters.AddWithValue("@nm_parceiro", parceiro.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", parceiro.Descricao);
                sqlConn.Command.Parameters.AddWithValue("@id_endereco", idEndereco);
                sqlConn.Command.Parameters.AddWithValue("@nm_codigo", parceiro.Codigo);

                sqlConn.Command.ExecuteNonQuery();

                #endregion

                sqlConn.Commit();

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao cadastrar o parceiro", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                sqlConn.Rollback();
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