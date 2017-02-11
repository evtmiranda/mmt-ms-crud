using ms_crud_rest.DAO;
using ClassesMarmitex;
using System.Web.Http;
using System.Collections.Generic;

namespace ms_crud_rest.Controllers
{
    public class UsuarioController : ApiController
    {
        private UsuarioDAO usuarioDAO;

        //construtor do controller, recebe um usuarioDAO, que por sua vez recebe uma ISession.
        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public UsuarioController(UsuarioDAO usuarioDAO)
        {
            this.usuarioDAO = usuarioDAO;
        }

        [HttpGet]
        public IList<Usuario> ConsultarUsuario()
        {
            return usuarioDAO.Buscar();
        }
    }
}
