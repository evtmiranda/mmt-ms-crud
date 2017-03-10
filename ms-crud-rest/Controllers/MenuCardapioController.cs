using ClassesMarmitex;
using ms_crud_rest.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ms_crud_rest.Controllers
{
    public class MenuCardapioController : ApiController
    {
        private MenuCardapioDAO cardapioDAO;
        private LogDAO logDAO;

        //construtor do controller, recebe um usuarioDAO e um logDAO, que por sua vez recebe uma ISession.
        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public MenuCardapioController(MenuCardapioDAO cardapioDAO, LogDAO logDAO)
        {
            this.cardapioDAO = cardapioDAO;
            this.logDAO = logDAO;
        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                MenuCardapio menuCardapio = cardapioDAO.BuscarPorId(id);
                if (menuCardapio == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, menuCardapio);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("O menu de cardápio {0} não foi encontrado", id);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
        }

        public HttpResponseMessage Post([FromBody] MenuCardapio cardapio)
        {
            try
            {
                cardapioDAO.Adicionar(cardapio);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                string location = Url.Link("DefaultApi", new { controller = "MenuCardapio", id = cardapio.Id });
                response.Headers.Location = new Uri(location);

                return response;
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o cardápio. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                //verifica se o id existe antes de excluir
                HttpResponseMessage retornoGet = Get(id);

                if (retornoGet.StatusCode == HttpStatusCode.OK)
                    cardapioDAO.ExcluirPorId(id);
                else
                    return retornoGet;

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel deletar o cardápio. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        public HttpResponseMessage Patch([FromBody] MenuCardapio cardapio, [FromUri] int id)
        {
            try
            {
                MenuCardapio cardapioAtual = cardapioDAO.BuscarPorId(id);

                if (cardapioAtual == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                cardapioAtual.Nome = cardapio.Nome;
                cardapioAtual.Ativo = cardapio.Ativo;

                cardapioDAO.Atualizar(cardapioAtual);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel atualizar o cardápio. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        //retorna todos os cardápios existentes
        [HttpGet]
        [Route("api/menucardapio/listar")]
        public HttpResponseMessage ListarCardapios()
        {
            try
            {
                IList<MenuCardapio> usuarios = cardapioDAO.Listar();
                return Request.CreateResponse(HttpStatusCode.OK, usuarios);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("nao foram encontrados cardápios");
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
        }

    }
}
