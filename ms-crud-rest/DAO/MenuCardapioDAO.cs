using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class MenuCardapioDAO : GenericDAO<MenuCardapio>
    {
        public MenuCardapioDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override List<MenuCardapio> Listar(int idParceiro)
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
	                                                            tmc.id_loja,
	                                                            nm_cardapio,
	                                                            nr_ordem_exibicao,
	                                                            tmc.bol_ativo
                                                            FROM tab_menu_cardapio as tmc
                                                            INNER JOIN tab_parceiro AS tp
                                                            ON tp.id_loja = tmc.id_loja
                                                            WHERE tp.id_parceiro = @id_parceiro
                                                            AND tmc.bol_ativo = 1;");
                        
                                                                

                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", idParceiro);

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
                    throw new CardapioNaoEncontradoException();

                //adiciona os produtos ao cardápio
                foreach (var menuCardapio in listaMenuCardapio)
                {
                    menuCardapio.Produtos = produtoDAO.Listar(menuCardapio.Id);
                }
                
                return listaMenuCardapio;
            }
            catch (CardapioNaoEncontradoException)
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
