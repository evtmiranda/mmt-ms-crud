using ClassesMarmitex;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class ProdutoAdicionalDAO : GenericDAO<DadosProdutoAdicional>
    {
        public ProdutoAdicionalDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override DadosProdutoAdicional BuscarPorId(int id, int idLoja)
        {
            List<DadosProdutoAdicionalEntidade> listaProdutoAdicionalEntidade = new List<DadosProdutoAdicionalEntidade>();
            List<DadosProdutoAdicional> listaProdutoAdicional = new List<DadosProdutoAdicional>();

            try
            {
                sqlConn.StartConnection();


                #region busca o produto adicional pelo id
                //busca o produtos
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_produto_adicional,
                                                                id_loja,
	                                                            nm_adicional,
	                                                            nm_descricao,
                                                                0 AS nr_qtd_min,
                                                                0 AS nr_qtd_max,
                                                                0 AS nr_ordem_exibicao,
	                                                            bol_ativo
                                                            FROM tab_produto_adicional
                                                            WHERE id_produto_adicional = @id_produto_adicional
                                                            AND bol_excluido = 0;");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional", id);
                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                //transforma a entidade em objeto
                if(sqlConn.Reader.HasRows)
                    listaProdutoAdicionalEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException();

                foreach (var produtoAdicionalEntidade in listaProdutoAdicionalEntidade)
                {
                    listaProdutoAdicional.Add(produtoAdicionalEntidade.ToProdutoAdicional());
                }

                #endregion

                return listaProdutoAdicional.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar produto adicional com id " + id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

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
                                                                id_loja,
	                                                            nm_adicional,
	                                                            nm_descricao,
                                                                0 AS nr_qtd_min,
                                                                0 AS nr_qtd_max,
                                                                0 AS nr_ordem_exibicao,
	                                                            bol_ativo
                                                            FROM tab_produto_adicional
                                                            WHERE id_loja = @id_loja
                                                            AND bol_excluido = 0");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);
                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                //transforma a entidade em objeto
                if (sqlConn.Reader.HasRows) 
                    listaProdutoAdicionalEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException("Nenhum produto adicional encontrado.");

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
                                                AND bol_excluido = 0";

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if(sqlConn.Reader != null)
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
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar os produtos adicionais para a loja com id " + idLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        public override void Adicionar(DadosProdutoAdicional produtoAdicional)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_produto_adicional(id_loja, nm_adicional, nm_descricao, bol_ativo)
                                                              VALUES(@id_loja, @nm_adicional, @nm_descricao, @bol_ativo);");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", produtoAdicional.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_adicional", produtoAdicional.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", produtoAdicional.Descricao ?? "");
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", produtoAdicional.Ativo);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = produtoAdicional.IdLoja, Mensagem = "Erro ao adicionar produto adicional" + produtoAdicional.IdLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }

        }

        public override void Excluir(DadosProdutoAdicional produtoAdicional)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional", produtoAdicional.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_produto_adicional
                                                                SET bol_excluido = 1, bol_ativo = 0
                                                            WHERE id_produto_adicional = @id_produto_adicional;");

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = produtoAdicional.IdLoja, Mensagem = "Erro ao excluir produto adicional" + produtoAdicional.IdLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Desativar(DadosProdutoAdicional produtoAdicional)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", produtoAdicional.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional", produtoAdicional.Id);

                sqlConn.Command.CommandText = @"DECLARE @ativo INT;
                                                SET @ativo = (SELECT bol_ativo FROM tab_produto_adicional WHERE id_produto_adicional = @id_produto_adicional);

                                                UPDATE tab_produto_adicional
	                                                SET bol_ativo = CASE WHEN @ativo = 1 THEN 0 ELSE 1 END
                                                WHERE id_produto_adicional = @id_produto_adicional;";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = produtoAdicional.IdLoja, Mensagem = "Erro ao desativar produto adicional" + produtoAdicional.IdLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Atualizar(DadosProdutoAdicional produtoAdicional)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional", produtoAdicional.Id);
                sqlConn.Command.Parameters.AddWithValue("@nm_adicional", produtoAdicional.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", produtoAdicional.Descricao ?? "");
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", produtoAdicional.Ativo);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_produto_adicional
	                                                            SET nm_adicional = @nm_adicional,
		                                                            nm_descricao = @nm_descricao,
		                                                            bol_ativo = @bol_ativo
                                                            WHERE id_produto_adicional = @id_produto_adicional");

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sqlConn.Rollback();
                logDAO.Adicionar(new Log { IdLoja = produtoAdicional.IdLoja, Mensagem = "Erro ao atualizar os dados do produto adicional" + produtoAdicional.IdLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        #region itens adicionais

        public void AdicionarItem(DadosProdutoAdicionalItem item)
        {
            try
            {
                sqlConn.StartConnection();

                //busca o produtos
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_produto_adicional_item(id_produto_adicional, nm_adicional_item, nm_descricao_item, vlr_adicional_item, bol_ativo)
                                                            VALUES(@id_produto_adicional, @nm_adicional_item, @nm_descricao_item, @vlr_adicional_item, @bol_ativo);");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional", item.IdProdutoAdicional);
                sqlConn.Command.Parameters.AddWithValue("@nm_adicional_item", item.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao_item", item.Descricao ?? "");
                sqlConn.Command.Parameters.AddWithValue("@vlr_adicional_item", item.Valor);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", item.Ativo);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = item.IdLoja, Mensagem = "Erro ao adicionar item para o produto adicional" + item.IdLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }

        }

        public void ExcluirItem(DadosProdutoAdicionalItem item)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional_item", item.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_produto_adicional_item
                                                                SET bol_excluido = 1, bol_ativo = 0
                                                            WHERE id_produto_adicional_item = @id_produto_adicional_item;");

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = item.IdLoja, Mensagem = "Erro ao excluir item para o produto adicional" + item.IdLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public void DesativarItem(DadosProdutoAdicionalItem item)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", item.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional_item", item.Id);

                sqlConn.Command.CommandText = @"DECLARE @ativo INT;
                                                SET @ativo = (SELECT bol_ativo FROM tab_produto_adicional_item WHERE id_produto_adicional_item = @id_produto_adicional_item);

                                                UPDATE tab_produto_adicional_item
	                                                SET bol_ativo = CASE WHEN @ativo = 1 THEN 0 ELSE 1 END
                                                WHERE id_produto_adicional_item = @id_produto_adicional_item;";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = item.IdLoja, Mensagem = "Erro ao desativar item para o produto adicional" + item.IdLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public DadosProdutoAdicionalItem BuscarItemPorId(int id, int idLoja)
        {
            List<DadosProdutoAdicionalItemEntidade> listaItemEntidade = new List<DadosProdutoAdicionalItemEntidade>();
            List<DadosProdutoAdicionalItem> listaItem = new List<DadosProdutoAdicionalItem>();

            try
            {
                sqlConn.StartConnection();


                #region busca o produto adicional pelo id
                //busca o produtos
                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_produto_adicional_item,
	                                                            id_produto_adicional,
	                                                            nm_adicional_item,
	                                                            nm_descricao_item,
	                                                            vlr_adicional_item,
	                                                            bol_ativo
                                                            FROM tab_produto_adicional_item
                                                            WHERE id_produto_adicional_item = @id_produto_adicional_item
                                                            AND bol_excluido = 0;");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional_item", id);
                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                //transforma a entidade em objeto
                if(sqlConn.Reader.HasRows)
                    listaItemEntidade = new ModuloClasse().PreencheClassePorDataReader<DadosProdutoAdicionalItemEntidade>(sqlConn.Reader);

                if (listaItemEntidade.Count == 0)
                    throw new KeyNotFoundException();

                foreach (var itemEntidade in listaItemEntidade)
                {
                    listaItem.Add(itemEntidade.ToProdutoAdicionalItem());
                }

                #endregion

                return listaItem.FirstOrDefault();
            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Item adicional nao encontrado com id " + id, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar item adicional com id:" + id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.Command.Parameters.Clear();
                sqlConn.CloseConnection();
            }
        }

        public void AtualizarItem(DadosProdutoAdicionalItem item)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region atualiza os dados do produto

                sqlConn.Command.Parameters.AddWithValue("@id_produto_adicional_item", item.Id);
                sqlConn.Command.Parameters.AddWithValue("@nm_adicional_item", item.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao_item", item.Descricao ?? "");
                sqlConn.Command.Parameters.AddWithValue("@vlr_adicional_item", item.Valor);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", item.Ativo);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_produto_adicional_item
	                                                            SET nm_adicional_item = @nm_adicional_item,
		                                                            nm_descricao_item = @nm_descricao_item,
		                                                            vlr_adicional_item = @vlr_adicional_item,
		                                                            bol_ativo = @bol_ativo
                                                            WHERE id_produto_adicional_item = @id_produto_adicional_item;");

                sqlConn.Command.ExecuteNonQuery();

                #endregion

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = item.IdLoja, Mensagem = "Erro ao atualizar item adicional com id:" + item.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
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