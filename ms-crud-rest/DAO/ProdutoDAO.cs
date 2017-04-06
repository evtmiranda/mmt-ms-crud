using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class ProdutoDAO : GenericDAO<Produto>
    {
        public ProdutoDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override List<Produto> Listar(int idMenuCardapio)
        {
            List<ProdutoEntidade> listaProdutoEntidade = new List<ProdutoEntidade>();
            List<Produto> listaProduto = new List<Produto>();

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_produto,
	                                                            id_menu_cardapio,
	                                                            nm_produto,
	                                                            nm_descricao,
	                                                            vlr_produto,
	                                                            img_imagem,
	                                                            bol_ativo
                                                            FROM tab_produto
                                                            WHERE id_menu_cardapio = @id_menu_cardapio
                                                            AND bol_ativo = 1;");

                sqlConn.Command.Parameters.AddWithValue("@id_menu_cardapio", idMenuCardapio);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaProdutoEntidade = new ModuloClasse().PreencheClassePorDataReader<ProdutoEntidade>(sqlConn.Reader);

                foreach (var produtoEntidade in listaProdutoEntidade)
                {
                    listaProduto.Add(produtoEntidade.ToProduto());
                }

                return listaProduto;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao buscar os produtos", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.Command.Parameters.Clear();
                sqlConn.CloseConnection();
            }
        }
    }
}
