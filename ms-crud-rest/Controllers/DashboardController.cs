using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassesMarmitex;
using ClassesMarmitex.Utils;
using ms_crud_rest.DAO;

namespace ms_crud_rest.Controllers
{
    public class DashboardController : ApiController
    {
        private DashboardDAO dashboardDAO;
        private LogDAO logDAO;

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public DashboardController(DashboardDAO dashboardDAO, LogDAO logDAO)
        {
            this.dashboardDAO = dashboardDAO;
            this.logDAO = logDAO;
        }

        [HttpGet]
        [Route("api/Dashboard/{idLoja}")]
        public HttpResponseMessage Buscar(int idLoja)
        {
            try
            {
                Dashboard dashboard = new Dashboard();

                dashboard = dashboardDAO.BuscarPorId(idLoja);

                return Request.CreateResponse(HttpStatusCode.OK, dashboard);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar o dashboard. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
