using ClassesMarmitex;
using NHibernate;

namespace ms_crud_rest.DAO
{
    public class UsuarioDAO : GenericDAO<Usuario>
    {
        //recebe uma sessão e atribui à sessão da classe
        private ISession session;
        public UsuarioDAO(ISession session)
        {
            this.session = session;
        }

        public bool AutenticarUsuario(Usuario usuario)
        {
            //executa o select e retorna uma lista de posts
            string hql = string.Format("select u from Usuario u where u.Email = '{0}' and u.Senha = '{1}'", usuario.Email, usuario.Senha);
            IQuery query = session.CreateQuery(hql);
            var result = query.List<Usuario>();

            if (result.Count >= 1)
                return true;
            else
                return false;
        }
    }
}
