using ClassesMarmitex;
using ms_crud_rest.DAO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ms_crud_rest.Controllers
{
    public class BrindeController : ApiController
    {
        private BrindeDAO brindeDAO;
        private LogDAO logDAO;

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public BrindeController(BrindeDAO brindeDAO, LogDAO logDAO)
        {
            this.brindeDAO = brindeDAO;
            this.logDAO = logDAO;
        }

        #region brindes

        [HttpPost]
        [Route("api/Brinde/Adicionar")]
        public HttpResponseMessage Adicionar([FromBody] Brinde brinde)
        {
            try
            {
                brindeDAO.Adicionar(brinde);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível cadastrar o brinde. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/Brinde/{id}/{idLoja}")]
        public HttpResponseMessage Buscar(int id, int idLoja)
        {
            try
            {
                Brinde brinde = new Brinde();

                brinde = brindeDAO.BuscarPorId(id, idLoja);

                return Request.CreateResponse(HttpStatusCode.OK, brinde);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar o brinde. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/Brinde/ListarPorLoja/{idLoja}")]
        public HttpResponseMessage ListarBrindesPorLoja(int idLoja)
        {
            try
            {
                IList<Brinde> brindes = brindeDAO.Listar(idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, brindes);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar os brindes. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Brinde/Atualizar")]
        public HttpResponseMessage Atualizar([FromBody] Brinde brinde)
        {
            try
            {
                brindeDAO.Atualizar(brinde);
                return Request.CreateResponse(HttpStatusCode.OK, brinde);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o brinde. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Brinde/Excluir")]
        public HttpResponseMessage Excluir([FromBody] Brinde brinde)
        {
            try
            {
                brindeDAO.Excluir(brinde);
                return Request.CreateResponse(HttpStatusCode.OK, brinde);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o brinde. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Brinde/Desativar")]
        public HttpResponseMessage Desativar([FromBody] Brinde brinde)
        {
            try
            {
                brindeDAO.Desativar(brinde);
                return Request.CreateResponse(HttpStatusCode.OK, brinde);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar o brinde. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #endregion

        #region brindes parceiros

        [HttpPost]
        [Route("api/BrindeParceiro/Adicionar")]
        public HttpResponseMessage AdicionarBrindeParceiro([FromBody] BrindeParceiro brinde)
        {
            try
            {
                brindeDAO.AdicionarBrindeParceiro(brinde);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível cadastrar o brinde para o parceiro. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/BrindeParceiro/Excluir")]
        public HttpResponseMessage ExcluirBrindeParceiro([FromBody] BrindeParceiro brinde)
        {
            try
            {
                brindeDAO.ExcluirBrindeParceiro(brinde);
                return Request.CreateResponse(HttpStatusCode.OK, brinde);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o brinde do parceiro. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/BrindeParceiro/Desativar")]
        public HttpResponseMessage DesativarBrindeParceiro([FromBody] BrindeParceiro brinde)
        {
            try
            {
                brindeDAO.DesativarBrindeParceiro(brinde);
                return Request.CreateResponse(HttpStatusCode.OK, brinde);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar o brinde do parceiro. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/BrindeParceiro/ListarPorParceiro/{idParceiro}/{idLoja}")]
        public HttpResponseMessage ListarBrindesPorParceiro(int idParceiro, int idLoja)
        {
            try
            {
                IList<Brinde> brindes = brindeDAO.ListarPorParceiro(idParceiro, idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, brindes);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar os brindes do parceiro. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #endregion
    }
}
