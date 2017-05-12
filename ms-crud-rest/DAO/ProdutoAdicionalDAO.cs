using ClassesMarmitex;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class ProdutoAdicionalDAO : GenericDAO<DadosProdutoAdicional>
    {
        public ProdutoAdicionalDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override List<DadosProdutoAdicional> Listar(int idLoja)
        {
            List<DadosProdutoAdicionalEntidade> listaProdutoAdicionalEntidade = new List<DadosProdutoAdicionalEntidade>();
            List<DadosProdutoAdicional> listaProdutoAdicional = new List<DadosProdutoAdicional>();

            List<DadosProdutoAdicionalItemEntidade> listaProdutoAdicionalItemEntidade = new List<DadosProdutoAdicionalItemEntidade>();
            List<DadosProdutoAdicionalItem> listaProdutoAdicionalItem = new List<DadosProdutoAdicionalItem>();

            try
            {
                sqlConn.StartConnection();


                #region Produtos Adicionais
                //busca os produtos
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_produto_adicional,
	                                                            nm_adicional,
	                                                            nm_descricao,
	                                                            bol_ativo
                                                            FROM tab_produto_adicional
                                                            WHERE id_loja = @id_loja;");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);
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

                //Adiciona os itens adicionais aos produtos adicionais
                for (int i = 0; i < listaProdutoAdicional.Count; i++)
                {
                    listaProdutoAdicional[i].ItensAdicionais =
                        listaProdutoAdicionalItem.Where(p => p.IdProdutoAdicional == listaProdutoAdicional[i].Id).ToList();
                }

                return listaProdutoAdicional;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao buscar os produtos adicionais", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.Command.Parameters.Clear();
                sqlConn.CloseConnection();
            }
        }

        public List<DadosProdutoAdicionalProduto> BuscarProdutosAdicionaisDeUmProduto(int idProduto)
        {
            List<DadosProdutoAdicionalProdutoEntidade> listaProdutoAdicionalProdutoEntidade = new List<DadosProdutoAdicionalProdutoEntidade>();
            List<DadosProdutoAdicionalProduto> listaProdutoAdicionalProduto = new List<DadosProdutoAdicionalProduto>();

            try
            {
                sqlConn.StartConnection();


                #region Produtos Adicionais do Produto
                //busca o produtos
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_produto_adicional_produto,
	                                                            id_produto,
	                                                            tpap.id_produto_adicional,
	                                                            tpa.nm_adicional,
	                                                            tpa.nm_descricao,
	                                                            nr_qtd_min,
	                                                            nr_qtd_max,
	                                                            nr_ordem_exibicao
                                                            FROM tab_produto_adicional_produto AS tpap
                                                            INNER JOIN tab_produto_adicional AS tpa
                                                            ON tpap.id_produto_adicional = tpa.id_produto_adicional
                                                            WHERE id_produto = @id_produto;");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_produto", idProduto);
                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                //transforma a entidade em objeto
                listaProdutoAdicionalProdutoEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalProdutoEntidade>(sqlConn.Reader);

                foreach (var produtoAdicionalProdutoEntidade in listaProdutoAdicionalProdutoEntidade)
                {
                    listaProdutoAdicionalProduto.Add(produtoAdicionalProdutoEntidade.ToProdutoAdicionalProduto());
                }

                //limpa os dados da execução anterior
                sqlConn.Command.CommandText = "";
                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();

                #endregion

                return listaProdutoAdicionalProduto;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao buscar os produtos adicionais do produto", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

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