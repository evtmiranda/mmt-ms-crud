using ClassesMarmitex;
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

        public Parceiro BuscarParceiro(int idParceiro, int idLoja)
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
                                                                vlr_taxa_entrega,
	                                                            bol_ativo
                                                            FROM tab_parceiro
                                                            WHERE id_parceiro = @id_parceiro
                                                            AND bol_excluido = 0;");

                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", idParceiro);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaParceirosEntidade = new ModuloClasse().PreencheClassePorDataReader<ParceiroEntidade>(sqlConn.Reader);

                if (listaParceirosEntidade.Count == 0)
                    throw new KeyNotFoundException("Parceiro não encontrado com o id: " + idParceiro);

                parceiro = listaParceirosEntidade[0].ToParceiro();

                //fecha o reader
                sqlConn.Reader.Close();

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
                sqlConn.Command.Parameters.AddWithValue("@id_endereco", parceiro.Endereco.Id);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                endEntidade = new ModuloClasse().PreencheClassePorDataReader<EnderecoEntidade>(sqlConn.Reader);

                if (endEntidade.Count == 0)
                    throw new KeyNotFoundException("Endereço não encontrado com o id: " + parceiro.Endereco.Id);

                endereco = endEntidade.FirstOrDefault().ToEndereco();

                #endregion

                parceiro.Endereco = endereco;

                return parceiro;
            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Parceiro ou endereço do parceiro não encontrado. id parceiro: " + idParceiro, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar o parceiro com id: " + idParceiro, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }

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
                                                                vlr_taxa_entrega,
	                                                            bol_ativo
                                                            FROM tab_parceiro
                                                            WHERE id_loja = @id_loja
                                                            AND bol_excluido = 0");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaParceirosEntidade = new ModuloClasse().PreencheClassePorDataReader<ParceiroEntidade>(sqlConn.Reader);
                sqlConn.Reader.Close();

                if (listaParceirosEntidade.Count == 0)
                    throw new KeyNotFoundException();

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
                            throw new KeyNotFoundException("Não foi possível consultar o endereço do parceiro: " + parceiro.Id);

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
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Parceiros ou endereço de algum parceiro não encontrado para a loja: " + idLoja, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar parceiros para a loja: " + idLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        public void AdicionarParceiro(int idLoja, Parceiro parceiro)
        {
            try
            {
                int idEndereco = 0;

                sqlConn.StartConnection();
                sqlConn.BeginTransaction();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region adiciona o endereco

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_endereco(nm_cep, nm_uf, nm_cidade, nm_bairro, nm_logradouro, nm_numero_endereco, nm_complemento_endereco)
                                                              VALUES(@nm_cep, @nm_uf, @nm_cidade, @nm_bairro, @nm_logradouro, @nm_numero_endereco, @nm_complemento_endereco); SELECT @@identity");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@nm_cep", parceiro.Endereco.Cep);
                sqlConn.Command.Parameters.AddWithValue("@nm_uf", parceiro.Endereco.UF);
                sqlConn.Command.Parameters.AddWithValue("@nm_cidade", parceiro.Endereco.Cidade);
                sqlConn.Command.Parameters.AddWithValue("@nm_bairro", parceiro.Endereco.Bairro);
                sqlConn.Command.Parameters.AddWithValue("@nm_logradouro", parceiro.Endereco.Logradouro);
                sqlConn.Command.Parameters.AddWithValue("@nm_numero_endereco", parceiro.Endereco.NumeroEndereco);
                sqlConn.Command.Parameters.AddWithValue("@nm_complemento_endereco", parceiro.Endereco.ComplementoEndereco ?? "");

                idEndereco = Convert.ToInt32(sqlConn.Command.ExecuteScalar());

                if (idEndereco == 0)
                    throw new Exceptions.EnderecoNaoCadastradoException("Não foi possível cadastrar o endereço para o parceiro: " + parceiro.Id);

                #endregion

                #region adiciona o parceiro

                sqlConn.Command.Parameters.Clear();

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_parceiro(id_loja, nm_parceiro, nm_descricao, id_endereco, nm_codigo, vlr_taxa_entrega)
                                                              VALUES(@id_loja, @nm_parceiro, @nm_descricao, @id_endereco, @nm_codigo, @vlr_taxa_entrega)");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_parceiro", parceiro.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", parceiro.Descricao ?? "");
                sqlConn.Command.Parameters.AddWithValue("@id_endereco", idEndereco);
                sqlConn.Command.Parameters.AddWithValue("@nm_codigo", parceiro.Codigo);
                sqlConn.Command.Parameters.AddWithValue("@vlr_taxa_entrega", parceiro.TaxaEntrega);

                sqlConn.Command.ExecuteNonQuery();

                #endregion

                sqlConn.Commit();

            }
            catch (Exceptions.EnderecoNaoCadastradoException endEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = endEx.Message ?? "Não foi possível cadastrar o endereço para o parceiro.", Descricao = endEx.Message ?? "", StackTrace = endEx.StackTrace ?? "" });
                throw endEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao cadastrar um parceiro para a loja: " + idLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Atualizar(Parceiro parceiro)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.BeginTransaction();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region atualiza os dados do parceiro

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@nm_parceiro", parceiro.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", parceiro.Descricao);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", parceiro.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", parceiro.Id);
                sqlConn.Command.Parameters.AddWithValue("@vlr_taxa_entrega", parceiro.TaxaEntrega);
                

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_parceiro
	                                                            SET nm_parceiro = @nm_parceiro,
		                                                            nm_descricao = @nm_descricao,
                                                                    vlr_taxa_entrega = @vlr_taxa_entrega,
		                                                            bol_ativo = @bol_ativo
                                                            WHERE id_parceiro = @id_parceiro;");

                sqlConn.Command.ExecuteNonQuery();

                #endregion

                #region atualiza o endereço do parceiro

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_endereco", parceiro.Endereco.Id);
                sqlConn.Command.Parameters.AddWithValue("@nm_cep", parceiro.Endereco.Cep);
                sqlConn.Command.Parameters.AddWithValue("@nm_uf", parceiro.Endereco.UF);
                sqlConn.Command.Parameters.AddWithValue("@nm_cidade", parceiro.Endereco.Cidade);
                sqlConn.Command.Parameters.AddWithValue("@nm_bairro", parceiro.Endereco.Bairro);
                sqlConn.Command.Parameters.AddWithValue("@nm_logradouro", parceiro.Endereco.Logradouro);
                sqlConn.Command.Parameters.AddWithValue("@nm_numero_endereco", parceiro.Endereco.NumeroEndereco);
                sqlConn.Command.Parameters.AddWithValue("@nm_complemento_endereco", parceiro.Endereco.ComplementoEndereco ?? "");

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_endereco
	                                                            SET nm_cep = @nm_cep,
		                                                            nm_uf = @nm_uf,
		                                                            nm_cidade = @nm_cidade,
		                                                            nm_bairro = @nm_bairro,
		                                                            nm_logradouro = @nm_logradouro,
		                                                            nm_numero_endereco = @nm_numero_endereco,
		                                                            nm_complemento_endereco = @nm_complemento_endereco
                                                            WHERE id_endereco = @id_endereco;");

                sqlConn.Command.ExecuteNonQuery();

                #endregion

                sqlConn.Commit();

            }
            catch (Exception ex)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { IdLoja = parceiro.IdLoja, Mensagem = "Erro ao atualizar os dados do parceiro: " + parceiro.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Excluir(Parceiro parceiro)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", parceiro.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", parceiro.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_parceiro
                                                                SET bol_excluido = 1, bol_ativo = 0
                                                            WHERE id_parceiro = @id_parceiro;");

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = parceiro.IdLoja, Mensagem = "Erro ao excluir o parceiro: " + parceiro.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Desativar(Parceiro parceiro)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", parceiro.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", parceiro.Id);

                sqlConn.Command.CommandText = @"DECLARE @ativo INT;
                                                SET @ativo = (SELECT bol_ativo FROM tab_parceiro WHERE id_parceiro = @id_parceiro);

                                                UPDATE tab_parceiro
	                                                SET bol_ativo = CASE WHEN @ativo = 1 THEN 0 ELSE 1 END
                                                WHERE id_parceiro = @id_parceiro;";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = parceiro.IdLoja, Mensagem = "Erro ao desativar o parceiro: " + parceiro.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
    }
}