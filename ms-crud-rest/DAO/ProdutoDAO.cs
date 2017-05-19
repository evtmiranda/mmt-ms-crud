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

        public override Produto BuscarPorId(int id)
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
                //busca o produtos
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
                                                            WHERE id_produto = @id_produto
                                                            AND bol_ativo = 1;");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_produto", id);
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
                //busca os dados adicionais do produto
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            tpa.id_produto_adicional,
                                                                tpa.id_loja,
	                                                            tpa.nm_adicional,
	                                                            tpa.nm_descricao,
	                                                            tpap.nr_qtd_min,
	                                                            tpap.nr_qtd_max,
	                                                            tpap.nr_ordem_exibicao,
	                                                            tpa.bol_ativo
                                                            FROM tab_produto_adicional_produto AS tpap
                                                            INNER JOIN tab_produto_adicional AS tpa
                                                            ON tpap.id_produto_adicional = tpa.id_produto_adicional
                                                            INNER JOIN tab_produto AS tp
                                                            ON tp.id_produto = tpap.id_produto
                                                            WHERE tpap.bol_ativo = 1
                                                            AND tpa.bol_ativo = 1
                                                            AND tp.bol_ativo = 1
                                                            AND tp.id_produto = @id_produto;");

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
                                                FROM tab_produto_adicional_item
                                                WHERE bol_ativo = 1;";

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
	                                                tpap.id_produto,
                                                    tp.nm_produto,
	                                                tpap.id_produto_adicional,
	                                                tpa.nm_adicional,
	                                                tpa.nm_descricao,
	                                                nr_qtd_min,
	                                                nr_qtd_max,
	                                                nr_ordem_exibicao,
                                                    tpap.bol_ativo
                                                FROM tab_produto_adicional_produto AS tpap
                                                INNER JOIN tab_produto_adicional AS tpa
                                                ON tpap.id_produto_adicional = tpa.id_produto_adicional
                                                INNER JOIN tab_produto AS tp
                                                ON tp.id_produto = tpap.id_produto
                                                WHERE tpap.bol_ativo = 1
                                                AND tpa.bol_ativo = 1
                                                AND tp.bol_ativo = 1;";

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

                return listaProduto.FirstOrDefault();
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
                                                                tpa.id_loja,
	                                                            tpa.nm_adicional,
	                                                            tpa.nm_descricao,
	                                                            tpap.nr_qtd_min,
	                                                            tpap.nr_qtd_max,
	                                                            tpap.nr_ordem_exibicao,
	                                                            tpa.bol_ativo
                                                            FROM tab_produto_adicional_produto AS tpap
                                                            INNER JOIN tab_produto_adicional AS tpa
                                                            ON tpap.id_produto_adicional = tpa.id_produto_adicional
                                                            INNER JOIN tab_produto AS tp
                                                            ON tp.id_produto = tpap.id_produto
                                                            WHERE tpap.bol_ativo = 1
                                                            AND tpa.bol_ativo = 1
                                                            AND tp.bol_ativo = 1
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
                                                FROM tab_produto_adicional_item
                                                WHERE bol_ativo = 1;";

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
                                                    tpap.id_produto,
                                                    tp.nm_produto,
                                                    tpap.id_produto_adicional,
                                                    tpa.nm_adicional,
                                                    tpa.nm_descricao,
                                                    nr_qtd_min,
                                                    nr_qtd_max,
                                                    nr_ordem_exibicao,
                                                    tpap.bol_ativo
                                                FROM tab_produto_adicional_produto AS tpap
                                                INNER JOIN tab_produto_adicional AS tpa
                                                ON tpap.id_produto_adicional = tpa.id_produto_adicional
                                                INNER JOIN tab_produto AS tp
                                                ON tp.id_produto = tpap.id_produto
                                                WHERE tpap.bol_ativo = 1
                                                AND tpa.bol_ativo = 1
                                                AND tp.bol_ativo = 1;";

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

        public override void Adicionar(Produto produto)
        {
            try
            {
                sqlConn.StartConnection();

                //busca o produtos
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_produto(id_menu_cardapio, nm_produto, nm_descricao, vlr_produto, url_imagem, bol_ativo)
                                                          VALUES(@id_menu_cardapio, @nm_produto, @nm_descricao, @vlr_produto, @url_imagem, @bol_ativo);");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_menu_cardapio", produto.IdMenuCardapio);
                sqlConn.Command.Parameters.AddWithValue("@nm_produto", produto.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", produto.Descricao);
                sqlConn.Command.Parameters.AddWithValue("@vlr_produto", produto.Valor);
                sqlConn.Command.Parameters.AddWithValue("@url_imagem", produto.Imagem);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", produto.Ativo);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public override void ExcluirPorId(int id)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region atualiza os dados do parceiro

                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", 0);
                sqlConn.Command.Parameters.AddWithValue("@id_produto", id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_produto
	                                                            SET bol_ativo = @bol_ativo
                                                            WHERE id_produto = @id_produto;");

                sqlConn.Command.ExecuteNonQuery();

                #endregion

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao excluir o produto ", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Seta um produto como inativo
        /// </summary>
        /// <param name="produto">produto que será inativado</param>
        /// <returns></returns>
        public override void Excluir(Produto produto)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region atualiza os dados do parceiro

                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", produto.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_produto", produto.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_produto
	                                                            SET bol_ativo = @bol_ativo
                                                            WHERE id_produto = @id_produto;");

                sqlConn.Command.ExecuteNonQuery();

                #endregion

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar os dados do produto", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Atualiza os dados de um parceiro
        /// </summary>
        /// <param name="parceiro">parceiro que será atualizado</param>
        /// <returns></returns>
        public override void Atualizar(Produto produto)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region atualiza os dados do produto

                sqlConn.Command.Parameters.AddWithValue("@id_produto", produto.Id);
                sqlConn.Command.Parameters.AddWithValue("@id_menu_cardapio", produto.IdMenuCardapio);
                sqlConn.Command.Parameters.AddWithValue("@nm_produto", produto.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", produto.Descricao);
                sqlConn.Command.Parameters.AddWithValue("@vlr_produto", produto.Valor);
                sqlConn.Command.Parameters.AddWithValue("@url_imagem", produto.Imagem);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", produto.Ativo);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_produto
	                                                            SET id_menu_cardapio = @id_menu_cardapio,
		                                                            nm_produto = @nm_produto,
		                                                            nm_descricao = @nm_descricao,
                                                                    vlr_produto = @vlr_produto,
                                                                    url_imagem = @url_imagem,
                                                                    bol_ativo = @bol_ativo
                                                            WHERE id_produto = @id_produto;");

                sqlConn.Command.ExecuteNonQuery();

                #endregion

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar os dados do produto", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                sqlConn.Rollback();

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }


        #region produtos adicionais do produto

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
	                                                            tpap.id_produto,
                                                                tp.nm_produto,
	                                                            tpap.id_produto_adicional,
	                                                            tpa.nm_adicional,
	                                                            tpa.nm_descricao,
	                                                            nr_qtd_min,
	                                                            nr_qtd_max,
	                                                            nr_ordem_exibicao,
                                                                tpap.bol_ativo
                                                            FROM tab_produto_adicional_produto AS tpap
                                                            INNER JOIN tab_produto_adicional AS tpa
                                                            ON tpap.id_produto_adicional = tpa.id_produto_adicional
                                                            INNER JOIN tab_produto AS tp
                                                            ON tp.id_produto = tpap.id_produto
                                                            WHERE tpap.id_produto = @id_produto
                                                            AND tpap.bol_ativo = 1
                                                            AND tpa.bol_ativo = 1
                                                            AND tp.bol_ativo = 1;");

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

        public DadosProdutoAdicionalProduto BuscarProdutoAdicionalDeUmProduto(int idProdutoAdicionalProduto)
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
	                                                            tpap.id_produto,
                                                                tp.nm_produto,
	                                                            tpap.id_produto_adicional,
	                                                            tpa.nm_adicional,
	                                                            tpa.nm_descricao,
	                                                            nr_qtd_min,
	                                                            nr_qtd_max,
	                                                            nr_ordem_exibicao,
                                                                tpap.bol_ativo
                                                            FROM tab_produto_adicional_produto AS tpap
                                                            INNER JOIN tab_produto_adicional AS tpa
                                                            ON tpap.id_produto_adicional = tpa.id_produto_adicional
                                                            INNER JOIN tab_produto AS tp
                                                            ON tp.id_produto = tpap.id_produto
                                                            WHERE tpap.id_produto_adicional_produto = @id_produto_adicional_produto
                                                            AND tpap.bol_ativo = 1
                                                            AND tpa.bol_ativo = 1
                                                            AND tp.bol_ativo = 1;");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional_produto", idProdutoAdicionalProduto);
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

                return listaProdutoAdicionalProduto.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao buscar o produto adicional do produto", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.Command.Parameters.Clear();
                sqlConn.CloseConnection();
            }
        }

        public void AdicionarProdutoAdicional(DadosProdutoAdicionalProduto produtoAdicionalProduto)
        {
            try
            {
                sqlConn.StartConnection();

                //insere o produto adicional para o produto
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_produto_adicional_produto(id_produto, id_produto_adicional, nr_qtd_min, nr_qtd_max, nr_ordem_exibicao)
                                                                VALUES(@id_produto, @id_produto_adicional, @nr_qtd_min, @nr_qtd_max, @nr_ordem_exibicao);");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_produto", produtoAdicionalProduto.IdProduto);
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional", produtoAdicionalProduto.IdProdutoAdicional);
                sqlConn.Command.Parameters.AddWithValue("@nr_qtd_min", produtoAdicionalProduto.QtdMin);
                sqlConn.Command.Parameters.AddWithValue("@nr_qtd_max", produtoAdicionalProduto.QtdMax);
                sqlConn.Command.Parameters.AddWithValue("@nr_ordem_exibicao", produtoAdicionalProduto.OrdemExibicao);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Seta um produto adicional como inativo
        /// </summary>
        /// <param name="produtoAdicional">produto que será inativado</param>
        /// <returns></returns>
        public void ExcluirProdutoAdicional(DadosProdutoAdicionalProduto produtoAdicional)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region atualiza os dados do parceiro

                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", produtoAdicional.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional_produto", produtoAdicional.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_produto_adicional_produto
	                                                            SET bol_ativo = @bol_ativo
                                                            WHERE id_produto_adicional_produto = @id_produto_adicional_produto;");

                sqlConn.Command.ExecuteNonQuery();

                #endregion

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar os dados do produto adicional", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Atualiza os dados de um produto adicional
        /// </summary>
        /// <param name="dadosProdutoAdicionalProduto">produto adicional que será atualizado</param>
        /// <returns></returns>
        public void AtualizarProdutoAdicionalProduto(DadosProdutoAdicionalProduto dadosProdutoAdicionalProduto)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region atualiza os dados do produto

                sqlConn.Command.Parameters.AddWithValue("@nr_qtd_min", dadosProdutoAdicionalProduto.QtdMin);
                sqlConn.Command.Parameters.AddWithValue("@nr_qtd_max", dadosProdutoAdicionalProduto.QtdMax);
                sqlConn.Command.Parameters.AddWithValue("@nr_ordem_exibicao", dadosProdutoAdicionalProduto.OrdemExibicao);
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional_produto", dadosProdutoAdicionalProduto.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_produto_adicional_produto
	                                                            SET nr_qtd_min = @nr_qtd_min,
		                                                            nr_qtd_max = @nr_qtd_max,
		                                                            nr_ordem_exibicao = @nr_ordem_exibicao
                                                            WHERE id_produto_adicional_produto = @id_produto_adicional_produto;");

                sqlConn.Command.ExecuteNonQuery();

                #endregion

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar os dados do produto adicional do produto", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                sqlConn.Rollback();

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
        

        #endregion

    }
}
