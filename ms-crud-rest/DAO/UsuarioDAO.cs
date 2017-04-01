using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System.Data.SqlClient;

namespace ms_crud_rest.DAO
{
    public class UsuarioDAO : GenericDAO<Usuario>
    {
        //recebe uma conexão e atribui à sessão da classe
        //recebe também um logDAO
        private SqlConnection sqlConn;
        private LogDAO logDAO;

        public UsuarioDAO(SqlConnection sqlConn, LogDAO logDAO)
        {
            this.sqlConn = sqlConn;
            this.logDAO = logDAO;
        }

        public void AutenticarUsuarioLoja(Usuario usuario, string dominioRede)
        {
            try
            {
                int qtdUsuario = 0;

                using (sqlConn) {

                    sqlConn.Open();

                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.Connection = sqlConn;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = string.Format(@"SELECT
	                                                            COUNT(1) 
                                                            FROM tab_usuario_parceiro AS tup
                                                            INNER JOIN tab_parceiro AS tp
                                                                ON tup.id_parceiro = tp.id_parceiro
                                                            INNER JOIN tab_loja AS tl
                                                                ON tl.id_loja = tp.id_loja
                                                            INNER JOIN tab_rede AS tr
                                                                ON tr.id_rede = tl.id_rede
                                                            WHERE tup.nm_email = @email 
                                                            AND tup.nm_senha = @senha 
                                                            AND tr.nm_dominio_rede = @dominio_rede
                                                            AND tup.bol_ativo = 1");

                    sqlCommand.Parameters.AddWithValue("@email", usuario.Email);
                    sqlCommand.Parameters.AddWithValue("@senha", usuario.Senha);
                    sqlCommand.Parameters.AddWithValue("@dominio_rede", dominioRede.Replace("'",""));

                    qtdUsuario = (int)sqlCommand.ExecuteScalar();
                }

                //verifica se o retorno foi positivo
                if (qtdUsuario == 0)
                    throw new UsuarioNaoAutenticadoException();
            }
            catch (System.Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao autenticar usuário", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
        }

        public void AutenticarUsuarioParceiro(Usuario usuario, string dominioRede)
        {
            try
            {
                int qtdUsuario = 0;

                using (sqlConn)
                {

                    sqlConn.Open();

                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.Connection = sqlConn;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = string.Format(@"SELECT 
                                                                COUNT(1) 
                                                             FROM tab_usuario_loja 
                                                             INNER JOIN tab_rede
                                                             ON tab_rede.id_rede = tab_usuario_loja.id_rede
                                                             WHERE nm_email = @email 
                                                             AND nm_senha = @senha 
                                                             AND nm_dominio_rede = @dominio_rede");

                    sqlCommand.Parameters.AddWithValue("@email", usuario.Email);
                    sqlCommand.Parameters.AddWithValue("@senha", usuario.Senha);
                    sqlCommand.Parameters.AddWithValue("@dominio_rede", dominioRede.Replace("'", ""));

                    qtdUsuario = (int)sqlCommand.ExecuteScalar();
                }

                //verifica se o retorno foi positivo
                if (qtdUsuario == 0)
                    throw new UsuarioNaoAutenticadoException();
            }
            catch (System.Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao autenticar usuário", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
        }

        /// <summary>
        /// Faz a busca de um usuário de loja através do e-mail
        /// </summary>
        /// <param name="usuario">Dados do usuario para consulta</param>
        /// <returns></returns>
        public UsuarioLoja BuscarUsuarioLojaPorEmail(Usuario usuario)
        {
            UsuarioLoja usuarioLoja;
            UsuarioLojaEntidade usuarioLojaEntidade;

            try
            {
                using (sqlConn)
                {
                    sqlConn.Open();

                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.Connection = sqlConn;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = string.Format(@"SELECT
                                                                id_usuario_loja,	
                                                                id_loja,
                                                                id_rede,	
                                                                nm_nome,
                                                                nm_apelido,
                                                                nm_email,
                                                                nm_senha,
                                                                nr_nivel_permissao,
                                                                bol_ativo  
                                                            FROM tab_usuario_loja
                                                            WHERE nm_email = @email");

                    sqlCommand.Parameters.AddWithValue("@email", usuario.Email);

                    SqlDataReader reader;
                    reader = sqlCommand.ExecuteReader();

                    usuarioLojaEntidade = new ModuloClasse().PreencheClassePorDataReader<UsuarioLojaEntidade>(reader)[0];

                    usuarioLoja = usuarioLojaEntidade.ToUsuarioLoja();

                    //fecha o reader
                    if (!reader.IsClosed)
                        reader.Close();

                    return usuarioLoja;
                }
            }
            catch (System.Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar os dados do usuário", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
        }

        public UsuarioParceiro BuscarUsuarioParceiroPorEmail(Usuario usuario)
        {
            UsuarioParceiro usuarioParceiro;
            UsuarioParceiroEntidade usuarioParceiroEntidade;

            try
            {
                using (sqlConn)
                {
                    sqlConn.Open();

                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.Connection = sqlConn;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = string.Format(@"SELECT
                                                                id_usuario_parceiro,	
                                                                id_parceiro,
                                                                nm_nome,	
                                                                nm_apelido,
                                                                nm_email,
                                                                nm_senha,
                                                                bol_ativo  
                                                            FROM tab_usuario_parceiro 
                                                            WHERE nm_email = @email");

                    sqlCommand.Parameters.AddWithValue("@email", usuario.Email);

                    SqlDataReader reader;
                    reader = sqlCommand.ExecuteReader();

                    usuarioParceiroEntidade = new ModuloClasse().PreencheClassePorDataReader<UsuarioParceiroEntidade>(reader)[0];

                    usuarioParceiro = usuarioParceiroEntidade.ToUsuarioParceiro();

                    //fecha o reader
                    if (!reader.IsClosed)
                        reader.Close();

                    return usuarioParceiro;
                }
            }
            catch (System.Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar os dados do usuário", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
        }
    }
}
