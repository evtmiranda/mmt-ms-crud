using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using NHibernate;

namespace ms_crud_rest.DAO
{
    public class UsuarioDAO : GenericDAO<Usuario>
    {
        //recebe uma sessão e atribui à sessão da classe
        //recebe também um logDAO
        private ISession session;
        private LogDAO logDAO;

        public UsuarioDAO(ISession session, LogDAO logDAO)
        {
            this.session = session;
            this.logDAO = logDAO;
        }

        public void AutenticarUsuario(Usuario usuario)
        {
            try
            {
                //executa o select e retorna uma lista de posts
                string hql = string.Format("select u from Usuario u where u.Email = '{0}' and u.Senha = '{1}'", usuario.Email, usuario.Senha);
                IQuery query = session.CreateQuery(hql);
                var result = query.List<Usuario>();

                //verifica se o retorno foi positivo
                if (result.Count == 0)
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
