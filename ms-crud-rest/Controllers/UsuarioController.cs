using ms_crud_rest.DAO;
using ClassesMarmitex;
using System.Web.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System;

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

        public HttpResponseMessage Get(int id)
        {
            try
            {
                Usuario usuario = usuarioDAO.BuscarPorId(id);
                return Request.CreateResponse(HttpStatusCode.OK, usuario);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("O usuario {0} não foi encontrado", id);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
        }

        public HttpResponseMessage Post([FromBody] Usuario usuario)
        {
            try
            {
                usuarioDAO.Adicionar(usuario);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                string location = Url.Link("DefaultApi", new { controller = "usuario", id = usuario.Id });
                response.Headers.Location = new Uri(location);

                return response;
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o usuario. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                usuarioDAO.ExcluirPorId(id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o usuario. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        public HttpResponseMessage Patch([FromBody] Usuario usuario, [FromUri] int id)
        {
            try
            {
                Usuario usuarioAtual = usuarioDAO.BuscarPorId(id);

                if (usuarioAtual == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                usuarioAtual.Email = usuario.Email;
                usuarioDAO.Atualizar(usuarioAtual);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel atualizar o usuario. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        //retorna todos os usuários existentes
        [HttpGet]
        [Route("api/usuario/listar")]
        public HttpResponseMessage ListarUsuarios()
        {
            try
            {
                IList<Usuario> usuarios = usuarioDAO.Buscar();
                return Request.CreateResponse(HttpStatusCode.OK, usuarios);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("nao foram encontrados usuarios");
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
        }
    }
}
