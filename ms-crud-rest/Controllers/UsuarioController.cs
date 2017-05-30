namespace ms_crud_rest.Controllers
{
    using DAO;
    using Exceptions;
    using ClassesMarmitex;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net;
    using System;

    public class UsuarioController : ApiController
    {
        public UsuarioDAO usuarioDAO;
        public LogDAO logDAO;

        //construtor do controller, recebe um usuarioDAO e um logDAO, que por sua vez recebe uma ISession.
        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public UsuarioController(UsuarioDAO usuarioDAO, LogDAO logDAO)
        {
            this.usuarioDAO = usuarioDAO;
            this.logDAO = logDAO;
        }

        [HttpGet]
        public HttpResponseMessage BuscarUsuario(int id)
        {
            try
            {
                Usuario usuario = null;

                if (usuario == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, usuario);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
        }

        /// <summary>
        /// Cadastro de um usuário do tipo loja
        /// </summary>
        /// <param name="usuario">objeto com todos os dados do usuário</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Usuario/Cadastrar/UsuarioLoja")]
        public HttpResponseMessage CadastrarUsuarioLoja([FromBody] UsuarioLoja usuario)
        {
            try
            {
                UsuarioLoja usuarioLoja = new UsuarioLoja();

                //verifica se já existe algum usuário com este e-mail
                usuarioLoja = usuarioDAO.BuscarUsuarioLojaPorEmail(usuario.Email);

                if (usuarioLoja != null)
                    throw new UsuarioJaExisteException();

                int idUsuarioCadastrado = usuarioDAO.CadastrarUsuarioLoja(usuario);

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (UsuarioJaExisteException eneEx)
            {
                string mensagem = eneEx.Message;
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, error);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("Não foi possivel cadastrar o usuario. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Usuario/Cadastrar/UsuarioParceiro")]
        public HttpResponseMessage CadastrarUsuarioParceiro([FromBody] UsuarioParceiro usuario)
        {
            try
            {
                UsuarioParceiro usuarioParceiro = new UsuarioParceiro();
                //verifica se já existe algum usuário com este e-mail
                usuarioParceiro = usuarioDAO.BuscarUsuarioParceiroPorEmail(usuario.Email);

                if (usuarioParceiro.Id != 0)
                    throw new UsuarioJaExisteException();

                //busca o parceiro
                int idParceiro = usuarioDAO.BuscarIdParceiro(usuario.CodigoParceiro);

                usuario.IdParceiro = idParceiro;

                int idUsuarioCadastrado = usuarioDAO.CadastrarUsuarioParceiro(usuario);

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (UsuarioJaExisteException eneEx)
            {
                string mensagem = eneEx.Message;
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, error);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("Não foi possivel cadastrar o usuario. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpDelete]
        public HttpResponseMessage ExcluirUsuario([FromUri] int id)
        {
            try
            {
                //usuarioDAO.ExcluirPorId(id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o usuario. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPatch]
        public HttpResponseMessage AtualizarUsuario([FromBody] Usuario usuario, [FromUri] int id)
        {
            try
            {
                Usuario usuarioAtual = usuarioDAO.BuscarPorId(id, usuario.IdLoja);

                if (usuarioAtual == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                usuarioAtual.Email = usuario.Email;
                usuarioDAO.Atualizar(usuarioAtual);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("Não foi possivel atualizar o usuario. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        //retorna todos os usuários existentes
        [HttpGet]
        [Route("api/usuario/listar/{idLoja}")]
        public HttpResponseMessage ListarUsuarios(TipoUsuario tipoUsuario, [FromUri] int idLoja)
        {
            try
            {
                IList<Usuario> usuarios = usuarioDAO.Listar(idLoja);
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
        [Route("api/Usuario/Autenticar/{tipoUsuario}/{dominioLoja}")]
        public HttpResponseMessage AutenticarUsuario([FromBody] Usuario usuario, [FromUri] TipoUsuario tipoUsuario, [FromUri] string dominioLoja)
        {
            //bloco de tratamento de erros
            try
            {
                //faz a autenticação do usuário
                //se for usuario loja
                if (tipoUsuario == TipoUsuario.Loja)
                    usuarioDAO.AutenticarUsuarioLoja(usuario, dominioLoja);
                else
                    usuarioDAO.AutenticarUsuarioParceiro(usuario, dominioLoja);

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
                //retorna mensagem de erro e status de erro
                string mensagem = string.Format("Não foi possivel autenticar o usuario. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Busca os dados de um usuário através do seu e-mail
        /// </summary>
        /// <param name="usuario">Dados do usuário</param>
        /// <param name="tipoUsuario">Define se é usuario de loja ou de parceiro</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/usuario/buscarPorEmail/{tipoUsuario}")]
        public HttpResponseMessage BuscarUsuarioPorEmail([FromBody] Usuario usuario, [FromUri] TipoUsuario tipoUsuario)
        {
            try
            {
                if (tipoUsuario == TipoUsuario.Loja)
                {
                    UsuarioLoja usuarioLoja;

                    usuarioLoja = usuarioDAO.BuscarUsuarioLojaPorEmail(usuario.Email);

                    return Request.CreateResponse(HttpStatusCode.OK, usuarioLoja);
                }
                else
                {
                    UsuarioParceiro usuarioParceiro;

                    usuarioParceiro = usuarioDAO.BuscarUsuarioParceiroPorEmail(usuario.Email);

                    return Request.CreateResponse(HttpStatusCode.OK, usuarioParceiro);
                }
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("O usuario com e-mail {0} não foi encontrado", usuario.Email);
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            catch (Exception)
            {
                string mensagem = string.Format("Estamos com dificuldade em consultar os dados. Por favor, tente novamente");
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
