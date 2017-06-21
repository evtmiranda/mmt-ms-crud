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
    using ClassesMarmitex.Utils;

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
        [Route("api/Usuario/BuscarUsuarioLoja/{id}")]
        public HttpResponseMessage BuscarUsuarioLoja(int id)
        {
            try
            {
                UsuarioLoja usuarioLoja = new UsuarioLoja();
                usuarioLoja = usuarioDAO.BuscarUsuarioLoja(id);
                return Request.CreateResponse(HttpStatusCode.OK, usuarioLoja);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = "Usuário não encontrado";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            catch (Exception)
            {
                string mensagem = "Não foi consultar o usuário. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/Usuario/BuscarUsuarioParceiro/{id}")]
        public HttpResponseMessage BuscarUsuarioParceiro(int id)
        {
            try
            {
                UsuarioParceiro usuarioParceiro = new UsuarioParceiro();
                usuarioParceiro = usuarioDAO.BuscarUsuarioParceiro(id);
                return Request.CreateResponse(HttpStatusCode.OK, usuarioParceiro);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = "Usuário não encontrado";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            catch (Exception)
            {
                string mensagem = "Não foi consultar o usuário. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/usuario/buscarPorEmail/{tipoUsuario}/{dominioLoja}")]
        public HttpResponseMessage BuscarUsuarioPorEmail([FromBody] Usuario usuario, [FromUri] TipoUsuario tipoUsuario, [FromUri] string dominioLoja)
        {
            try
            {
                if (tipoUsuario == TipoUsuario.Loja)
                {
                    UsuarioLoja usuarioLoja;

                    usuarioLoja = usuarioDAO.BuscarUsuarioLojaPorEmail(usuario.Email, dominioLoja);

                    return Request.CreateResponse(HttpStatusCode.OK, usuarioLoja);
                }
                else
                {
                    UsuarioParceiro usuarioParceiro;

                    usuarioParceiro = usuarioDAO.BuscarUsuarioParceiroPorEmail(usuario.Email, dominioLoja);

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

        [HttpPost]
        [Route("api/Usuario/Cadastrar/UsuarioLoja/{dominioLoja}")]
        public HttpResponseMessage CadastrarUsuarioLoja([FromBody] UsuarioLoja usuario, [FromUri] string dominioLoja)
        {
            try
            {
                UsuarioLoja usuarioLoja = new UsuarioLoja();

                //verifica se já existe algum usuário com este e-mail
                usuarioLoja = usuarioDAO.BuscarUsuarioLojaPorEmail(usuario.Email, dominioLoja);

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
        [Route("api/Usuario/Cadastrar/UsuarioParceiro/{dominioLoja}")]
        public HttpResponseMessage CadastrarUsuarioParceiro([FromBody] UsuarioParceiro usuario, [FromUri] string dominioLoja)
        {
            try
            {
                UsuarioParceiro usuarioParceiro = new UsuarioParceiro();
                //verifica se já existe algum usuário com este e-mail
                try
                {
                    usuarioParceiro = usuarioDAO.BuscarUsuarioParceiroPorEmail(usuario.Email, dominioLoja);
                }
                catch (KeyNotFoundException)
                {
                    //não faz nada
                }
                catch (Exception)
                {
                    throw;
                }
                

                if (usuarioParceiro.Id != 0)
                    throw new UsuarioJaExisteException("Já existe um usuário cadastrado com este e-mail. Se esqueceu a senha, é possível enviar uma nova para o seu e-mail clicando no link abaixo...");

                //busca o parceiro
                int idParceiro = usuarioDAO.BuscarIdParceiro(usuario.CodigoParceiro);

                if (idParceiro == 0)
                    throw new ParceiroNaoCadastradoException("Código da empresa não é válido");

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
            catch (ParceiroNaoCadastradoException eneEx)
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
        [Route("api/Usuario/AtualizarUsuarioLoja")]
        public HttpResponseMessage AtualizarUsuarioLoja([FromBody] UsuarioLoja usuarioLoja)
        {
            try
            {
                usuarioDAO.AtualizarUsuarioLoja(usuarioLoja);
                return Request.CreateResponse(HttpStatusCode.OK, usuarioLoja);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o usuario. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Usuario/AtualizarUsuarioParceiro")]
        public HttpResponseMessage AtualizarUsuarioParceiro([FromBody] UsuarioParceiro usuarioParceiro)
        {
            try
            {
                usuarioDAO.AtualizarUsuarioParceiro(usuarioParceiro);
                return Request.CreateResponse(HttpStatusCode.OK, usuarioParceiro);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o usuario. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/usuario/listar/{tipoUsuario}/{idLoja}")]
        public HttpResponseMessage ListarUsuarios([FromUri] TipoUsuario tipoUsuario, [FromUri] int idLoja)
        {
            try
            {
                List<UsuarioLoja> listaUsuarioLoja = new List<UsuarioLoja>();
                List<UsuarioParceiro> listaUsuarioParceiro = new List<UsuarioParceiro>();

                if (tipoUsuario == TipoUsuario.Loja)
                {
                    listaUsuarioLoja = usuarioDAO.ListarUsuariosLoja(idLoja);
                    return Request.CreateResponse(HttpStatusCode.OK, listaUsuarioLoja);
                }
                else
                    return null;
            }
            //se ocorrer algum erro no processamento
            catch (Exception ex)
            {
                //retorna mensagem de erro e status de erro
                string mensagem = string.Format("Não foi possivel listar os usuarios. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

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

        [HttpPost]
        [Route("api/Usuario/ExcluirUsuarioLoja")]
        public HttpResponseMessage ExcluirUsuarioLoja([FromBody] UsuarioLoja usuarioLoja)
        {
            try
            {
                usuarioDAO.ExcluirUsuarioLoja(usuarioLoja);
                return Request.CreateResponse(HttpStatusCode.OK, usuarioLoja);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o usuário. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Usuario/DesativarUsuarioLoja")]
        public HttpResponseMessage DesativarUsuarioLoja([FromBody] UsuarioLoja usuarioLoja)
        {
            try
            {
                usuarioDAO.DesativarUsuarioLoja(usuarioLoja);
                return Request.CreateResponse(HttpStatusCode.OK, usuarioLoja);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar o usuário. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
