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

        public void AutenticarUsuarioEmpresa(Usuario usuario)
        {
            try
            {
                int qtdUsuario = 0;

                using (sqlConn) {

                    sqlConn.Open();

                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.Connection = sqlConn;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = string.Format("SELECT COUNT(1) FROM tab_usuario_empresa where email = @email and senha = @senha");

                    sqlCommand.Parameters.AddWithValue("@email", usuario.Email);
                    sqlCommand.Parameters.AddWithValue("@senha", usuario.Senha);

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

        public void AutenticarUsuarioParceiro(Usuario usuario)
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
                    sqlCommand.CommandText = string.Format("SELECT COUNT(1) FROM tab_usuario_parceiro where email = @email and senha = @senha");

                    sqlCommand.Parameters.AddWithValue("@email", usuario.Email);
                    sqlCommand.Parameters.AddWithValue("@senha", usuario.Senha);

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


    }
}
