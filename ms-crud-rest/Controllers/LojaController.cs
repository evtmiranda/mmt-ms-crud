using ClassesMarmitex;
using ms_crud_rest.DAO;
using System;
using System.Collections.Generic;
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

        [HttpGet]
        [Route("api/Loja/BuscarLoja/{dominio}")]
        public HttpResponseMessage BuscarLoja(string dominio)
        {
            try
            {
                Loja loja = lojaDAO.BuscarLoja(dominio);

                return Request.CreateResponse(HttpStatusCode.OK, loja);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = "Nenhuma loja encontrada para este dominio. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar a loja. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
