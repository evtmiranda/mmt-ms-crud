using ClassesMarmitex;
using ms_crud_rest.DAO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ms_crud_rest.Controllers
{
    public class MenuCardapioController : ApiController
    {
        private MenuCardapioDAO cardapioDAO;
        private LogDAO logDAO;
        private LojaDAO lojaDAO;

        //construtor do controller, recebe um usuarioDAO e um logDAO, que por sua vez recebe uma ISession.
        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public MenuCardapioController(MenuCardapioDAO cardapioDAO, LojaDAO lojaDAO, LogDAO logDAO)
        {
            this.cardapioDAO = cardapioDAO;
            this.logDAO = logDAO;
            this.lojaDAO = lojaDAO;
        }

        [Route("api/Cardapio/BuscarCardapio/{id}/{idLoja}")]
        public HttpResponseMessage Get(int id, int idLoja)
        {
            try
            {
                MenuCardapio menuCardapio = cardapioDAO.BuscarPorId(id, idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, menuCardapio);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("O menu de cardápio {0} não foi encontrado", id);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            catch(Exception)
            {
                string mensagem = "Não foi possível consultar o cardápio. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [Route("api/MenuCardapio/Cadastrar")]
        public HttpResponseMessage Post([FromBody] MenuCardapio cardapio)
        {
            try
            {
                cardapioDAO.AdicionarCardapio(cardapio);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível cadastrar o cardápio. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/MenuCardapio/Atualizar")]
        public HttpResponseMessage AtualizarCardapio([FromBody] MenuCardapio cardapio)
        {
            try
            {
                cardapioDAO.Atualizar(cardapio);
                return Request.CreateResponse(HttpStatusCode.OK, cardapio);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o cardápio. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/MenuCardapio/Excluir")]
        public HttpResponseMessage ExcluirCardapio([FromBody] MenuCardapio cardapio)
        {
            try
            {
                cardapioDAO.Excluir(cardapio);
                return Request.CreateResponse(HttpStatusCode.OK, cardapio);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o cardápio. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/MenuCardapio/Desativar")]
        public HttpResponseMessage DesativarCardapio([FromBody] MenuCardapio cardapio)
        {
            try
            {
                cardapioDAO.Excluir(cardapio);
                return Request.CreateResponse(HttpStatusCode.OK, cardapio);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar o cardápio. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/menucardapio/listar/{idLoja}")]
        public HttpResponseMessage ListarCardapios(int idLoja)
        {
            try
            {
                IList<MenuCardapio> cardapios = cardapioDAO.Listar(idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, cardapios);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar os cardápios. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

    }
}
