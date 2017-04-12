using ClassesMarmitex;
using ms_crud_rest.DAO;
using ms_crud_rest.Exceptions;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ms_crud_rest.Controllers
{
    public class LojaController : ApiController
    {
        private LojaDAO lojaDAO;
        private LogDAO logDAO;

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public LojaController(LojaDAO lojaDAO, LogDAO logDAO)
        {
            this.lojaDAO = lojaDAO;
            this.logDAO = logDAO;
        }

        /// <summary>
        /// Busca uma loja pelo dominio da url
        /// </summary>
        /// <param name="dominio">dominio da url</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Loja/BuscarLoja/{dominio}")]
        public HttpResponseMessage BuscarLoja(string dominio)
        {
            try
            {
                Loja loja = lojaDAO.BuscarLoja(dominio);

                if (loja == null)
                    throw new LojaNaoEncontradaException();

                return Request.CreateResponse(HttpStatusCode.OK, loja);
            }
            catch (LojaNaoEncontradaException)
            {
                string mensagem = string.Format("nenhuma loja encontrada para o dominio {0}", dominio);
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.NoContent, error);
            }
            catch(Exception ex)
            {
                string mensagem = string.Format("ocorreu um erro ao buscar a loja. Erro: {0}", string.IsNullOrEmpty(ex.Message) ? "a exception não retornou mensagem" : ex.Message);
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
