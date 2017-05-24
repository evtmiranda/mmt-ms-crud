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

        public override MenuCardapio BuscarPorId(int idCardapio, int idLoja)
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

                //verifica se encontrou algum cardápio
                if (listaMenuCardapioEntidade.Count == 0)
                    throw new KeyNotFoundException();

                cardapio = listaMenuCardapioEntidade.FirstOrDefault().ToMenuCardapio();

                cardapio.Produtos = produtoDAO.Listar(cardapio.Id);

                return cardapio;
            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Cardápio nao encontrado com id " + idCardapio, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar o cardápio com id " + idCardapio, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
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

                foreach (var menuCardapio in listaMenuCardapioEntidade)
                {
                    listaMenuCardapio.Add(menuCardapio.ToMenuCardapio());
                }

                //verifica se encontrou algum cardápio
                if (listaMenuCardapio.Count == 0)
                    throw new KeyNotFoundException();

                //adiciona os produtos ao cardápio
                foreach (var menuCardapio in listaMenuCardapio)
                {
                    menuCardapio.Produtos = produtoDAO.Listar(menuCardapio.Id, idLoja);
                }

                return listaMenuCardapio;
            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Nenhum cardápio foi encontrado encontrado para a loja: " + idLoja, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar os cardápios para a loja: " + idLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }

        public void AdicionarCardapio(MenuCardapio cardapio)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_menu_cardapio(id_loja, nm_cardapio, nr_ordem_exibicao, bol_ativo)
                                                              VALUES(@id_loja, @nm_cardapio, @nr_ordem_exibicao, @bol_ativo);");

                sqlConn.Command.Parameters.AddWithValue("@id_loja", cardapio.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_cardapio", cardapio.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nr_ordem_exibicao", cardapio.OrdemExibicao);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", cardapio.Ativo);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = cardapio.IdLoja, Mensagem = "Erro ao cadastrar o cardápio com id " + cardapio.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

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
                logDAO.Adicionar(new Log { IdLoja = cardapio.IdLoja, Mensagem = "Erro ao atualizar o cardápio com id " + cardapio.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Excluir(MenuCardapio cardapio)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", cardapio.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_menu_cardapio", cardapio.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_menu_cardapio
	                                                            SET bol_ativo = @bol_ativo
                                                            WHERE id_menu_cardapio = @id_menu_cardapio;");

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = cardapio.IdLoja, Mensagem = "Erro ao excluir o cardápio com id " + cardapio.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
    }
}
