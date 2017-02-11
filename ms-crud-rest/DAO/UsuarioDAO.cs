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
    }
}
