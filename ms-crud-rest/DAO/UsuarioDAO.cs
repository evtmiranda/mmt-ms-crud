using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using NHibernate;
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

        public void AutenticarUsuario(Usuario usuario)
        {
            try
            {
                int qtdUsuario = 0;

                using (sqlConn) { 
                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = string.Format("SELECT COUNT(1) FROM usuario where email = @email and senha = @senha");

                    SqlParameter pEmail = new SqlParameter();
                    pEmail.ParameterName = "@email";
                    pEmail.Value = usuario.Email;

                    SqlParameter pSenha = new SqlParameter();
                    pSenha.ParameterName = "@senha";
                    pSenha.Value = usuario.Senha;

                    sqlCommand.Parameters.Add(pEmail);
                    sqlCommand.Parameters.Add(pSenha);

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
