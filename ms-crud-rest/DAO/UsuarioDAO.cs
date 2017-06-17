using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;
using ClassesMarmitex.Utils;

namespace ms_crud_rest.DAO
{
    public class UsuarioDAO : GenericDAO<Usuario>
    {
        public UsuarioDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        /// <summary>
        /// Faz o cadastro de um usuário no banco de dados
        /// </summary>
        /// <param name="usuario">dados do usuário</param>
        public int CadastrarUsuarioLoja(UsuarioLoja usuario)
        {
            try
            {
                int retorno = 0;

                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_usuario_loja(id_loja, nm_usuario, nm_apelido, nm_email, nm_senha, nr_nivel_permissao, bol_ativo)
                                                            VALUES(@id_loja, @nm_usuario, @nm_apelido, @nm_email, @nm_senha, @nr_nivel_permissao, @bol_ativo);");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", usuario.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_usuario", usuario.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_apelido", usuario.Apelido ?? usuario.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_email", usuario.Email);
                sqlConn.Command.Parameters.AddWithValue("@nm_senha", usuario.Senha);
                sqlConn.Command.Parameters.AddWithValue("@nr_nivel_permissao", usuario.NivelPermissao);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", usuario.Ativo);

                var varRetorno = sqlConn.Command.ExecuteScalar();

                retorno = Convert.ToInt32(varRetorno);

                return retorno;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = usuario.IdLoja, Mensagem = "Erro ao cadastrar usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Faz o cadastro de um usuário no banco de dados
        /// </summary>
        /// <param name="usuario">dados do usuário</param>
        /// <param name="chaveParceiro">chave do parceiro</param>
        public int CadastrarUsuarioParceiro(UsuarioParceiro usuario)
        {
            try
            {
                int retorno = 0;

                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_usuario_parceiro(id_parceiro, nm_usuario, nm_apelido, nm_email, nm_celular, nm_senha)
                                                            VALUES(@id_parceiro, @nm_usuario, @nm_apelido, @nm_email, @nm_celular, @nm_senha); SELECT @@IDENTITY;");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", usuario.IdParceiro);
                sqlConn.Command.Parameters.AddWithValue("@nm_usuario", usuario.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_apelido", usuario.Apelido);
                sqlConn.Command.Parameters.AddWithValue("@nm_email", usuario.Email);
                sqlConn.Command.Parameters.AddWithValue("@nm_celular", usuario.NumeroCelular);
                sqlConn.Command.Parameters.AddWithValue("@nm_senha", usuario.Senha);

                var varRetorno = sqlConn.Command.ExecuteScalar();

                retorno = Convert.ToInt32(varRetorno);

                return retorno;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = usuario.IdLoja, Mensagem = "erro ao cadastrar usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public void AtualizarUsuarioLoja(UsuarioLoja usuarioLoja)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@nm_usuario", usuarioLoja.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_apelido", usuarioLoja.Apelido ?? usuarioLoja.Nome);
                sqlConn.Command.Parameters.AddWithValue("@nm_email", usuarioLoja.Email);
                sqlConn.Command.Parameters.AddWithValue("@nm_senha", usuarioLoja.Senha);
                sqlConn.Command.Parameters.AddWithValue("@id_usuario_loja", usuarioLoja.Id);


                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_usuario_loja
	                                                            SET nm_usuario = @nm_usuario,
		                                                            nm_apelido = @nm_apelido,
                                                                    nm_email = @nm_email,
		                                                            nm_senha = @nm_senha
                                                            WHERE id_usuario_loja = @id_usuario_loja;");

                sqlConn.Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = usuarioLoja.IdLoja, Mensagem = "Erro ao atualizar os dados do usuario: " + usuarioLoja.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public void AtualizarUsuarioParceiro(UsuarioParceiro usuarioParceiro)
        {
            //try
            //{
            //    sqlConn.StartConnection();

            //    sqlConn.Command.CommandType = System.Data.CommandType.Text;

            //    sqlConn.Command.Parameters.Clear();
            //    sqlConn.Command.Parameters.AddWithValue("@nm_usuario", usuarioLoja.Nome);
            //    sqlConn.Command.Parameters.AddWithValue("@nm_apelido", usuarioLoja.Apelido ?? usuarioLoja.Nome);
            //    sqlConn.Command.Parameters.AddWithValue("@nm_email", usuarioLoja.Email);
            //    sqlConn.Command.Parameters.AddWithValue("@nm_senha", usuarioLoja.Senha);
            //    sqlConn.Command.Parameters.AddWithValue("@id_usuario_loja", usuarioLoja.Id);


            //    sqlConn.Command.CommandText = string.Format(@"UPDATE tab_usuario_loja
	           //                                                 SET nm_usuario = @nm_usuario,
		          //                                                  nm_apelido = @nm_apelido,
            //                                                        nm_email = @nm_email,
		          //                                                  nm_senha = @nm_senha
            //                                                WHERE id_usuario_loja = @id_usuario_loja;");

            //    sqlConn.Command.ExecuteNonQuery();

            //}
            //catch (Exception ex)
            //{
            //    logDAO.Adicionar(new Log { IdLoja = usuarioLoja.IdLoja, Mensagem = "Erro ao atualizar os dados do usuario: " + usuarioLoja.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
            //    throw ex;
            //}
            //finally
            //{
            //    sqlConn.CloseConnection();
            //}
        }

        /// <summary>
        /// Autentica um usuário do tipo parceiro
        /// </summary>
        /// <param name="usuario">Dados do usuário para autenticacao</param>
        /// <param name="dominio">Dominio da loja que o usuário pertence</param>
        public void AutenticarUsuarioParceiro(Usuario usuario, string dominio)
        {
            try
            {
                int qtdUsuario = 0;

                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                                COUNT(1) 
                                                                FROM tab_usuario_parceiro AS tup
                                                                INNER JOIN tab_parceiro AS tp
                                                                    ON tup.id_parceiro = tp.id_parceiro
                                                                INNER JOIN tab_loja AS tl
                                                                    ON tl.id_loja = tp.id_loja
                                                                WHERE tup.nm_email = @email 
                                                                AND tup.nm_senha = @senha 
                                                                AND tl.nm_dominio_loja = @nm_dominio_loja
                                                                AND tup.bol_excluido = 0
                                                                AND tp.bol_excluido = 0
                                                                AND tl.bol_excluido = 0
                                                                AND tup.bol_ativo = 1
                                                                AND tp.bol_ativo = 1
                                                                AND tl.bol_ativo = 1");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@email", usuario.Email);
                sqlConn.Command.Parameters.AddWithValue("@senha", usuario.Senha);
                sqlConn.Command.Parameters.AddWithValue("@nm_dominio_loja", dominio.Replace("'", ""));

                qtdUsuario = Convert.ToInt32(sqlConn.Command.ExecuteScalar());

                if (qtdUsuario == 0)
                    throw new UsuarioNaoAutenticadoException();

                //insere log de acesso
                try
                {
                    sqlConn.Command.CommandText = @"INSERT INTO tab_acessos_usuario_parceiro(id_usuario_parceiro)
                                                    SELECT
                                                       id_usuario_parceiro
                                                    FROM tab_usuario_parceiro
                                                    WHERE nm_email = @email";

                    sqlConn.Command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logDAO.Adicionar(new Log { IdLoja = usuario.IdLoja, Mensagem = "Erro ao inserir log de acesso usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                }
            }
            catch (UsuarioNaoAutenticadoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = usuario.IdLoja, Mensagem = "Erro ao autenticar usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Autentica um usuário do tipo loja
        /// </summary>
        /// <param name="usuario">Dados do usuário para autenticacao</param>
        /// <param name="dominioLoja">Loja que o usuário pertence</param>
        public void AutenticarUsuarioLoja(Usuario usuario, string dominioLoja)
        {
            try
            {
                int qtdUsuario = 0;

                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT 
                                                                COUNT(1) 
                                                             FROM tab_usuario_loja AS tul
                                                             INNER JOIN tab_loja AS tl
                                                                ON tul.id_loja = tl.id_loja
                                                             WHERE nm_email = @email 
                                                             AND nm_senha = @senha 
                                                             AND tl.nm_dominio_loja = @nm_dominio_loja
                                                             AND tul.bol_excluido = 0
                                                             AND tl.bol_excluido = 0
                                                             AND tul.bol_ativo = 1
                                                             AND tl.bol_ativo = 1");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@email", usuario.Email);
                sqlConn.Command.Parameters.AddWithValue("@senha", usuario.Senha);
                sqlConn.Command.Parameters.AddWithValue("@nm_dominio_loja", dominioLoja.Replace("'", ""));

                qtdUsuario = Convert.ToInt32(sqlConn.Command.ExecuteScalar());

                if (qtdUsuario == 0)
                    throw new UsuarioNaoAutenticadoException();

                //insere log de acesso
                try
                {
                    sqlConn.Command.CommandText = @"INSERT INTO tab_acessos_usuario_loja(id_usuario_loja)
                                                    SELECT
                                                       id_usuario_loja
                                                    FROM tab_usuario_loja
                                                    WHERE nm_email = @email";

                    sqlConn.Command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logDAO.Adicionar(new Log { IdLoja = usuario.IdLoja, Mensagem = "Erro ao inserir log de acesso usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                }
            }
            catch (UsuarioNaoAutenticadoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = usuario.IdLoja, Mensagem = "Erro ao autenticar usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        /// <summary>
        /// Faz a busca de um usuário de loja através do id
        /// </summary>
        /// <param name="id">id do usuário</param>
        /// <returns>UsuarioLoja</returns>
        public UsuarioLoja BuscarUsuarioLoja(int idUsuario)
        {
            UsuarioLoja usuarioLoja;
            List<UsuarioLojaEntidade> listaUsuarioLojaEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
                                                                tul.id_usuario_loja,	
                                                                tul.id_loja,
                                                                tl.nm_loja,
                                                                tul.nm_usuario,
                                                                tul.nm_apelido,
                                                                tul.nm_email,
                                                                tul.nm_senha,
                                                                tul.nr_nivel_permissao,
                                                                tul.bol_ativo,
                                                                tl.url_imagem
                                                            FROM tab_usuario_loja AS tul
                                                            INNER JOIN tab_loja AS tl
                                                            ON tul.id_loja = tl.id_loja
                                                            WHERE tul.id_usuario_loja = @id_usuario_loja
                                                            AND tul.bol_excluido = 0
                                                            AND tl.bol_excluido = 0");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_usuario_loja", idUsuario);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaUsuarioLojaEntidade = new ModuloClasse().PreencheClassePorDataReader<UsuarioLojaEntidade>(sqlConn.Reader);

                usuarioLoja = listaUsuarioLojaEntidade[0].ToUsuarioLoja();

                return usuarioLoja;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar os dados do usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        /// <summary>
        /// Faz a busca de um usuário de loja através do e-mail
        /// </summary>
        /// <param name="email">email do usuário</param>
        /// <returns>UsuarioLoja</returns>
        public UsuarioLoja BuscarUsuarioLojaPorEmail(string email, string dominioLoja)
        {
            UsuarioLoja usuarioLoja = new UsuarioLoja();
            List<UsuarioLojaEntidade> listaUsuarioLojaEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
                                                                tul.id_usuario_loja,	
                                                                tul.id_loja,
                                                                tl.nm_loja,
                                                                tul.nm_usuario,
                                                                tul.nm_apelido,
                                                                tul.nm_email,
                                                                tul.nm_senha,
                                                                tul.nr_nivel_permissao,
                                                                tul.bol_ativo,
                                                                tl.url_imagem
                                                            FROM tab_usuario_loja AS tul
                                                            INNER JOIN tab_loja AS tl
                                                            ON tul.id_loja = tl.id_loja
                                                            WHERE tl.nm_dominio_loja = @dominioLoja
                                                            AND nm_email = @email
                                                            AND tul.bol_excluido = 0
                                                            AND tl.bol_excluido = 0");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@email", email);
                sqlConn.Command.Parameters.AddWithValue("@dominioLoja", dominioLoja.Replace("'", ""));

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaUsuarioLojaEntidade = new ModuloClasse().PreencheClassePorDataReader<UsuarioLojaEntidade>(sqlConn.Reader);

                if (listaUsuarioLojaEntidade.Count > 1)
                    usuarioLoja = listaUsuarioLojaEntidade[0].ToUsuarioLoja();
                else
                    usuarioLoja = null;

                return usuarioLoja;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar os dados do usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        public List<UsuarioLoja> ListarUsuariosLoja(int idLoja)
        {
            List<UsuarioLoja> listaUsuarioLoja = new List<UsuarioLoja>();
            List<UsuarioLojaEntidade> listaUsuarioLojaEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
                                                                tul.id_usuario_loja,	
                                                                tul.id_loja,
                                                                tl.nm_loja,
                                                                tul.nm_usuario,
                                                                tul.nm_apelido,
                                                                tul.nm_email,
                                                                tul.nm_senha,
                                                                tul.nr_nivel_permissao,
                                                                tul.bol_ativo,
                                                                tl.url_imagem
                                                            FROM tab_usuario_loja AS tul
                                                            INNER JOIN tab_loja AS tl
                                                            ON tul.id_loja = tl.id_loja
                                                            WHERE tl.id_loja = @id_loja
                                                            AND tul.bol_excluido = 0
                                                            AND tl.bol_excluido = 0");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaUsuarioLojaEntidade = new ModuloClasse().PreencheClassePorDataReader<UsuarioLojaEntidade>(sqlConn.Reader);

                foreach (var usuarioEntidade in listaUsuarioLojaEntidade)
                {
                    listaUsuarioLoja.Add(usuarioEntidade.ToUsuarioLoja());
                }

                return listaUsuarioLoja;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao listar os usuários da loja: " + idLoja, Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        /// <summary>
        /// faz a busca de um usuário de parceiros através do id
        /// </summary>
        /// <param name="id">id do usuário</param>
        /// <returns>UsuarioParceiro</returns>
        public UsuarioParceiro BuscarUsuarioParceiro(int idUsuario)
        {
            UsuarioParceiro usuarioParceiro = new UsuarioParceiro();
            List<UsuarioParceiroEntidade> listaUsuarioParceiroEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_usuario_parceiro,
	                                                            tl.id_loja,
                                                                tl.nm_loja,
	                                                            tup.id_parceiro,
	                                                            nm_usuario,	
	                                                            nm_apelido,
	                                                            nm_email,
	                                                            nm_celular,
	                                                            nm_senha,
	                                                            tup.bol_ativo,
	                                                            concat(nm_logradouro, ', ', nm_numero_endereco, ' - ', nm_bairro, ', ', nm_cidade) AS endereco,
                                                                tp.vlr_taxa_entrega
                                                            FROM tab_usuario_parceiro AS tup
                                                            INNER JOIN tab_parceiro AS tp
                                                            ON tup.id_parceiro = tp.id_parceiro
                                                            INNER JOIN tab_loja AS tl
                                                            ON tl.id_loja = tp.id_loja
                                                            INNER JOIN tab_endereco AS te
                                                            ON te.id_endereco = tp.id_endereco
                                                            WHERE id_usuario_parceiro = @id_usuario_parceiro
                                                            AND tup.bol_excluido = 0
                                                            AND tp.bol_excluido = 0");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_usuario_parceiro", idUsuario);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaUsuarioParceiroEntidade = new ModuloClasse().PreencheClassePorDataReader<UsuarioParceiroEntidade>(sqlConn.Reader);

                if (listaUsuarioParceiroEntidade.Count > 0)
                    usuarioParceiro = listaUsuarioParceiroEntidade[0].ToUsuarioParceiro();

                return usuarioParceiro;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar os dados do usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        /// <summary>
        /// faz a busca de um usuário de parceiros através do e-mail
        /// </summary>
        /// <param name="email">email do usuário</param>
        /// <returns>UsuarioParceiro</returns>
        public UsuarioParceiro BuscarUsuarioParceiroPorEmail(string email, string dominioLoja)
        {
            UsuarioParceiro usuarioParceiro = new UsuarioParceiro();
            List<UsuarioParceiroEntidade> listaUsuarioParceiroEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_usuario_parceiro,
	                                                            tl.id_loja,
                                                                tl.nm_loja,
	                                                            tup.id_parceiro,
	                                                            nm_usuario,	
	                                                            nm_apelido,
	                                                            nm_email,
	                                                            nm_celular,
	                                                            nm_senha,
	                                                            tup.bol_ativo,
	                                                            concat(nm_logradouro, ', ', nm_numero_endereco, ' - ', nm_bairro, ', ', nm_cidade) AS endereco,
                                                                tp.vlr_taxa_entrega
                                                            FROM tab_usuario_parceiro AS tup
                                                            INNER JOIN tab_parceiro AS tp
                                                            ON tup.id_parceiro = tp.id_parceiro
                                                            INNER JOIN tab_loja AS tl
                                                            ON tl.id_loja = tp.id_loja
                                                            INNER JOIN tab_endereco AS te
                                                            ON te.id_endereco = tp.id_endereco
                                                            WHERE nm_email = @email
                                                            AND tl.nm_dominio_loja = @dominioLoja
                                                            AND tup.bol_excluido = 0
                                                            AND tp.bol_excluido = 0");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@email", email);
                sqlConn.Command.Parameters.AddWithValue("@dominioLoja", dominioLoja.Replace("'", ""));

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaUsuarioParceiroEntidade = new ModuloClasse().PreencheClassePorDataReader<UsuarioParceiroEntidade>(sqlConn.Reader);

                if (listaUsuarioParceiroEntidade.Count > 0)
                    usuarioParceiro = listaUsuarioParceiroEntidade[0].ToUsuarioParceiro();

                return usuarioParceiro;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar os dados do usuário", Descricao = ex.Message, StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        public int BuscarIdParceiro(string sCodigoParceiro)
        {
            try
            {
                int retorno = 0;

                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT 
                                                                id_parceiro 
                                                            FROM tab_parceiro
                                                            WHERE nm_codigo = @codigoParceiro
                                                            AND bol_excluido = 0");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@codigoParceiro", sCodigoParceiro);

                retorno = Convert.ToInt32(sqlConn.Command.ExecuteScalar());

                return retorno;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = 0, Mensagem = "Erro ao consultar parceiro", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }

        }

        public void ExcluirUsuarioLoja(UsuarioLoja usuarioLoja)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", usuarioLoja.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_usuario_loja", usuarioLoja.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_usuario_loja
                                                                SET bol_excluido = 1, bol_ativo = 0
                                                            WHERE id_usuario_loja = @id_usuario_loja;");

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = usuarioLoja.IdLoja, Mensagem = "Erro ao excluir o usuario: " + usuarioLoja.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public void DesativarUsuarioLoja(UsuarioLoja usuarioLoja)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", usuarioLoja.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_usuario_loja", usuarioLoja.Id);

                sqlConn.Command.CommandText = @"DECLARE @ativo INT;
                                                SET @ativo = (SELECT bol_ativo FROM tab_usuario_loja WHERE id_usuario_loja = @id_usuario_loja);

                                                UPDATE tab_usuario_loja
	                                                SET bol_ativo = CASE WHEN @ativo = 1 THEN 0 ELSE 1 END
                                                WHERE id_usuario_loja = @id_usuario_loja;";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = usuarioLoja.IdLoja, Mensagem = "Erro ao desativar o usuario: " + usuarioLoja.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public void ExcluirUsuarioParceiro(UsuarioParceiro usuarioParceiro)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", usuarioParceiro.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_usuario_parceiro", usuarioParceiro.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_usuario_parceiro
                                                                SET bol_excluido = 1, bol_ativo = 0
                                                            WHERE id_usuario_parceiro = @id_usuario_parceiro;");

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = usuarioParceiro.IdLoja, Mensagem = "Erro ao excluir o usuario: " + usuarioParceiro.Id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
    }
}
