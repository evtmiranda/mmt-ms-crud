using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class MenuCardapioDAO : GenericDAO<MenuCardapio>
    {
        public MenuCardapioDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

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
    }
}
