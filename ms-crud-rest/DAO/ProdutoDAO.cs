using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Linq;
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

            List<DadosProdutoAdicionalItemEntidade> listaProdutoAdicionalItemEntidade = new List<DadosProdutoAdicionalItemEntidade>();
            List<DadosProdutoAdicionalItem> listaProdutoAdicionalItem = new List<DadosProdutoAdicionalItem>();

            List<DadosProdutoAdicionalProdutoEntidade> listaProdutoAdicionalProdutoEntidade = new List<DadosProdutoAdicionalProdutoEntidade>();
            List<DadosProdutoAdicionalProduto> listaProdutoAdicionalProduto = new List<DadosProdutoAdicionalProduto>();

            try
            {
                sqlConn.StartConnection();


                #region Produtos
                //busca os produtos
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_produto,
	                                                            id_menu_cardapio,
	                                                            nm_produto,
	                                                            nm_descricao,
	                                                            vlr_produto,
	                                                            url_imagem,
	                                                            bol_ativo
                                                            FROM tab_produto
                                                            WHERE id_menu_cardapio = @id_menu_cardapio
                                                            AND bol_ativo = 1;");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_menu_cardapio", idMenuCardapio);
                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                //transforma a entidade em objeto
                listaProdutoEntidade = new ModuloClasse().PreencheClassePorDataReader<ProdutoEntidade>(sqlConn.Reader);
                foreach (var produtoEntidade in listaProdutoEntidade)
                {
                    listaProduto.Add(produtoEntidade.ToProduto());
                }

                //limpa os dados da execução anterior
                sqlConn.Command.CommandText = "";
                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();

                #endregion

                #region Adicionais Produtos
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

                //transforma a entidade em objeto
                listaProdutoAdicionalEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalEntidade>(sqlConn.Reader);

                foreach (var produtoAdicionalEntidade in listaProdutoAdicionalEntidade)
                {
                    listaProdutoAdicional.Add(produtoAdicionalEntidade.ToProdutoAdicional());
                }

                //limpa os dados da execução anterior
                sqlConn.Command.CommandText = "";
                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();

                #endregion

                #region Itens Adicionais Produtos
                //lista quais itens os produtos adicionais possui
                sqlConn.Command.CommandText = @"SELECT
	                                                id_produto_adicional_item,
	                                                id_produto_adicional,
	                                                nm_adicional_item,
	                                                nm_descricao_item,
	                                                vlr_adicional_item,
	                                                bol_ativo
                                                FROM tab_produto_adicional_item;";

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                //transforma a entidade em objeto
                listaProdutoAdicionalItemEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalItemEntidade>(sqlConn.Reader);

                foreach (var itemAdicional in listaProdutoAdicionalItemEntidade)
                {
                    listaProdutoAdicionalItem.Add(itemAdicional.ToProdutoAdicionalItem());
                }

                //limpa os dados da execução anterior
                sqlConn.Command.CommandText = "";
                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();

                #endregion

                #region Relação de adicionais para um produto
                //monta a relação de adicionais por produto
                sqlConn.Command.CommandText = @"SELECT
	                                                id_produto_adicional_produto,
	                                                id_produto,
	                                                id_produto_adicional
                                                FROM tab_produto_adicional_produto;";

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                //transforma a entidade em objeto
                listaProdutoAdicionalProdutoEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalProdutoEntidade>(sqlConn.Reader);

                foreach (var prodAdicionalProduto in listaProdutoAdicionalProdutoEntidade)
                {
                    listaProdutoAdicionalProduto.Add(prodAdicionalProduto.ToProdutoAdicionalProduto());
                }

                //limpa os dados da execução anterior
                sqlConn.Command.CommandText = "";
                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();

                #endregion

                //Adiciona os itens adicionais aos produtos adicionais
                for (int i = 0; i < listaProdutoAdicional.Count; i++)
                {
                    listaProdutoAdicional[i].ItensAdicionais =
                        listaProdutoAdicionalItem.Where(p => p.IdProdutoAdicional == listaProdutoAdicional[i].Id).ToList();
                }

                //Adiciona os produtos adicionais aos produtos
                for (int i = 0; i < listaProduto.Count; i++)
                {
                    //busca a relação de produtos adicionais para este produto
                    List<DadosProdutoAdicionalProduto> listaIdProdutosAdicionais = new List<DadosProdutoAdicionalProduto>();
                    listaIdProdutosAdicionais = listaProdutoAdicionalProduto.Where(p => p.IdProduto == listaProduto[i].Id).ToList();

                    //monta uma lista somente com os produtos adicionais deste produto
                    List<DadosProdutoAdicional> listaProdutoAdicionalFiltrada = new List<DadosProdutoAdicional>();
                    foreach (var produtoAdicional in listaIdProdutosAdicionais)
                    {

                        var produtoAdicionalFiltrado = listaProdutoAdicional.Where(p => p.Id == produtoAdicional.IdProdutoAdicional).ToList();
                        listaProdutoAdicionalFiltrada.Add((DadosProdutoAdicional)produtoAdicionalFiltrado.FirstOrDefault());
                    }

                    //adiciona os produtos adicionais ao produto
                    listaProduto[i].DadosAdicionaisProdutos = listaProdutoAdicionalFiltrada;
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
