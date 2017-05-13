using ClassesMarmitex;
using ms_crud_rest.DAO;
using ms_crud_rest.Exceptions;
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
        private LojaDAO lojaDAO;

        //construtor do controller, recebe um usuarioDAO e um logDAO, que por sua vez recebe uma ISession.
        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public MenuCardapioController(MenuCardapioDAO cardapioDAO, LojaDAO lojaDAO, LogDAO logDAO)
        {
            this.cardapioDAO = cardapioDAO;
            this.logDAO = logDAO;
            this.lojaDAO = lojaDAO;
        }

        [Route("api/Cardapio/BuscarCardapio/{id}")]
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

        [Route("api/MenuCardapio/Cadastrar/{idLoja}")]
        public HttpResponseMessage Post([FromUri] int idLoja, [FromBody] MenuCardapio cardapio)
        {
            try
            {
                cardapioDAO.AdicionarCardapio(idLoja, cardapio);

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

        /// <summary>
        /// Atualiza os dados de um cardápio
        /// </summary>
        /// <param name="cardapio">cardápio que será atualizado</param>
        /// <returns></returns>
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
                string mensagem = "Não foi possível atualizar o cardapio. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }


        /// <summary>
        /// Inativa um cardapio
        /// </summary>
        /// <param name="cardapio">cardapio que será atualizado</param>
        /// <returns></returns>
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
                string mensagem = "Não foi possível excluir o cardapio. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Retorna todos os cardápios existentes de uma determinada loja
        /// </summary>
        /// <param name="idLoja"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/menucardapio/listar/{idLoja}")]
        public HttpResponseMessage ListarCardapios(int idLoja)
        {
            try
            {
                IList<MenuCardapio> cardapios = cardapioDAO.Listar(idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, cardapios);
            }
            catch (NenhumCardapioEncontradoException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = string.Format("ocorreu um problema ao buscar os cardápios. por favor, tente atualizar a página ou acessar dentro de alguns minutos...");
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

    }
}
