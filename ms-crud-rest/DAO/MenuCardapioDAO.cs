using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ms_crud_rest.DAO
{
    public class MenuCardapioDAO : GenericDAO<MenuCardapio>
    {
        LojaDAO lojaDAO;

        public MenuCardapioDAO(SqlServer sqlConn, LogDAO logDAO, LojaDAO lojaDAO) : base(sqlConn, logDAO)
        {
            this.lojaDAO = lojaDAO;
        }

        public override List<MenuCardapio> Listar(int idLoja)
        {
            List<MenuCardapioEntidade> listaMenuCardapioEntidade = new List<MenuCardapioEntidade>();
            List<MenuCardapio> listaMenuCardapio = new List<MenuCardapio>();

            ProdutoDAO produtoDAO = new ProdutoDAO(sqlConn, logDAO);

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT 
	                                                            id_menu_cardapio,
	                                                            id_loja,
	                                                            nm_cardapio,
	                                                            nr_ordem_exibicao,
	                                                            bol_ativo
                                                            FROM tab_menu_cardapio
                                                            WHERE id_loja = @id_loja
                                                            AND bol_ativo = 1;");



                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaMenuCardapioEntidade = new ModuloClasse().PreencheClassePorDataReader<MenuCardapioEntidade>(sqlConn.Reader);

                //fecha o reader
                sqlConn.Reader.Close();

                foreach (var menuCardapio in listaMenuCardapioEntidade)
                {
                    listaMenuCardapio.Add(menuCardapio.ToMenuCardapio());
                }

                //verifica se o retorno foi positivo
                if (listaMenuCardapio.Count == 0)
                    throw new NenhumCardapioEncontradoException();

                //adiciona os produtos ao cardápio
                foreach (var menuCardapio in listaMenuCardapio)
                {
                    menuCardapio.Produtos = produtoDAO.Listar(menuCardapio.Id);
                }

                return listaMenuCardapio;
            }
            catch (NenhumCardapioEncontradoException)
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
            }
        }

        public override MenuCardapio BuscarPorId(int idCardapio)
        {
            List<MenuCardapioEntidade> listaMenuCardapioEntidade = new List<MenuCardapioEntidade>();
            MenuCardapio cardapio = new MenuCardapio();

            ProdutoDAO produtoDAO = new ProdutoDAO(sqlConn, logDAO);

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT 
	                                                            id_menu_cardapio,
	                                                            id_loja,
	                                                            nm_cardapio,
	                                                            nr_ordem_exibicao,
	                                                            bol_ativo
                                                            FROM tab_menu_cardapio
                                                            WHERE id_menu_cardapio = @id_menu_cardapio");



                sqlConn.Command.Parameters.AddWithValue("@id_menu_cardapio", idCardapio);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaMenuCardapioEntidade = new ModuloClasse().PreencheClassePorDataReader<MenuCardapioEntidade>(sqlConn.Reader);

                //fecha o reader
                sqlConn.Reader.Close();

                //verifica se o retorno foi positivo
                if (listaMenuCardapioEntidade.Count == 0)
                    throw new NenhumCardapioEncontradoException();

                cardapio = listaMenuCardapioEntidade.FirstOrDefault().ToMenuCardapio();

                cardapio.Produtos = produtoDAO.Listar(cardapio.Id);

                return cardapio;
            }
            catch (NenhumCardapioEncontradoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao buscar o cardápio", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Faz o cadastro de um cardápio
        /// </summary>
        /// <param name="cardapio">dados do cardápio</param>
        /// <returns></returns>
        public void AdicionarCardapio(int idLoja, MenuCardapio cardapio)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_menu_cardapio(id_loja, nm_cardapio, nr_ordem_exibicao, bol_ativo)
                                                              VALUES(@id_loja, @nm_cardapio, @nr_ordem_exibicao, @bol_ativo);");

                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_cardapio", cardapio.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nr_ordem_exibicao", cardapio.OrdemExibicao);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", cardapio.Ativo);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao cadastrar o cardápio", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                sqlConn.Rollback();
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }


        /// <summary>
        /// Atualiza os dados de um cardapio
        /// </summary>
        /// <param name="cardapio">cardapio que será atualizado</param>
        /// <returns></returns>
        public override void Atualizar(MenuCardapio cardapio)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.AddWithValue("@nm_cardapio", cardapio.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nr_ordem_exibicao", cardapio.OrdemExibicao);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", cardapio.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_menu_cardapio", cardapio.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_menu_cardapio
	                                                            SET nm_cardapio = @nm_cardapio,
		                                                            nr_ordem_exibicao = @nr_ordem_exibicao,
		                                                            bol_ativo = @bol_ativo
                                                            WHERE id_menu_cardapio = @id_menu_cardapio;");

                sqlConn.Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar os dados do cardápio", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                sqlConn.Rollback();

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Seta um cardapio como inativo
        /// </summary>
        /// <param name="cardapio">cardapio que será inativado</param>
        /// <returns></returns>
        public override void Excluir(MenuCardapio cardapio)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region atualiza os dados do cardapio

                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", cardapio.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_menu_cardapio", cardapio.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_menu_cardapio
	                                                            SET bol_ativo = @bol_ativo
                                                            WHERE id_menu_cardapio = @id_menu_cardapio;");

                sqlConn.Command.ExecuteNonQuery();

                #endregion

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar os dados do cardápio", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
    }
}
