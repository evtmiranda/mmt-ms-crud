using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassesMarmitex;
using ClassesMarmitex.Utils;
using ms_crud_rest.DAO;
using RestSharp;

namespace ms_crud_rest.Controllers
{
    public class EmailController : ApiController
    {
        public EmailDAO emailDAO;
        public LogDAO logDAO;

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public EmailController(EmailDAO emailDAO, LogDAO logDAO)
        {
            this.emailDAO = emailDAO;
            this.logDAO = logDAO;
        }

        [HttpPost]
        [Route("api/Email/EnviarEmailUnitario/")]
        public HttpResponseMessage EnviarEmailUnitario([FromBody] DadosEnvioEmailUnitario emailUnitario)
        {
            try
            {
                IRestResponse response = new RestResponse();

                response = emailDAO.EnviarEmailUnitario(emailUnitario);

                return Request.CreateResponse(response.StatusCode);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível enviar o e-mail. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
