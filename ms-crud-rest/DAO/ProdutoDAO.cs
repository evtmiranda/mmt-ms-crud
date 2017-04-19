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

            List<DadosProdutoAdicionalEntidade> listaProdutoAdicionalEntidade = new List<DadosProdutoAdicionalEntidade>();
            List<DadosProdutoAdicional> listaProdutoAdicional = new List<DadosProdutoAdicional>();

            try
            {
                sqlConn.StartConnection();
                
                //busca os produtos
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

                //transforma os produtos entidades em produtos
                listaProdutoEntidade = new ModuloClasse().PreencheClassePorDataReader<ProdutoEntidade>(sqlConn.Reader);
                foreach (var produtoEntidade in listaProdutoEntidade)
                {
                    listaProduto.Add(produtoEntidade.ToProduto());
                }

                //limpa os dados da execução anterior
                sqlConn.Command.CommandText = "";
                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();


                //lista quais produtos adicionais o produto possui
                sqlConn.Command.CommandText = @"SELECT
	                                                id_produto_adicional_produto,
	                                                id_produto,
	                                                id_produto_adicional
                                                FROM tab_produto_adicional_produto AS tpap;";



                //busca os dados adicionais dos produtos
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            tpa.id_produto_adicional,
	                                                            tpa.nm_adicional,
	                                                            tpa.nm_descricao,
	                                                            tpa.nr_qtd_min,
	                                                            tpa.nr_qtd_max,
	                                                            tpa.nr_ordem_exibicao,
	                                                            tpa.bol_ativo
                                                            FROM tab_produto_adicional_produto AS tpap
                                                            INNER JOIN tab_produto_adicional AS tpa
                                                            ON tpap.id_produto_adicional = tpa.id_produto_adicional
                                                            INNER JOIN tab_produto AS tp
                                                            ON tp.id_produto = tpap.id_produto
                                                            WHERE tpa.bol_ativo = 1
                                                            AND tp.id_menu_cardapio = @id_menu_cardapio;");

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                //transforma os produtos adicionais entidades em produtos adicionais
                listaProdutoAdicionalEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalEntidade>(sqlConn.Reader);

                foreach (var produtoAdicionalEntidade in listaProdutoAdicionalEntidade)
                {
                    listaProdutoAdicional.Add(produtoAdicionalEntidade.ToProdutoAdicional());
                }


                //adiciona os produtos adicionais aos produtos
                for (int i = 0; i < listaProduto.Count; i++)
                {
                    for (int j = 0; j < listaProdutoAdicional.Count; j++)
                    {
                        if(listaProduto[i].Id == listaProdutoAdicional[j].)
                    }
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
