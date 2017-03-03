using ms_crud_rest.DAO;
using ClassesMarmitex;
using System.Web.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System;
using ms_crud_rest.Exceptions;

namespace ms_crud_rest.Controllers
{
    public class UsuarioController : ApiController
    {
        private UsuarioDAO usuarioDAO;
        private LogDAO logDAO;

        //construtor do controller, recebe um usuarioDAO e um logDAO, que por sua vez recebe uma ISession.
        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public UsuarioController(UsuarioDAO usuarioDAO, LogDAO logDAO)
        {
            this.usuarioDAO = usuarioDAO;
            this.logDAO = logDAO;
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

        //método para autenticação de usuário
        [HttpPost]
        [Route("api/usuario/autenticar")]
        public HttpResponseMessage AutenticarUsuario([FromBody] Usuario usuario)
        {
            //bloco de tratamento de erros
            try
            {
                //faz a autenticação do usuário
                usuarioDAO.AutenticarUsuario(usuario);
                //se for autenticado, retorna mensagem de aceito
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            //se o usuário não for autenticado, retorna mensagem de não autorizado
            catch (UsuarioNaoAutenticadoException)
            {
                string error = "Usuário ou senha inválida";
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, error);
            }
            //se ocorrer algum erro no processamento
            catch (Exception ex)
            {
                //grava log do erro
                logDAO.Adicionar(new Log { Mensagem = "Erro ao autenticar usuário", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                //retorna mensagem de erro e status de erro
                string mensagem = string.Format("nao foi possivel autenticar o usuario. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
