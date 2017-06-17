using ClassesMarmitex;
using ms_crud_rest.DAO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassesMarmitex.Utils;

namespace ms_crud_rest.Controllers
{
    public class ParceiroController : ApiController
    {
        public ParceiroDAO parceiroDAO;
        public LogDAO logDAO;

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public ParceiroController(ParceiroDAO parceiroDAO, LogDAO logDAO)
        {
            this.parceiroDAO = parceiroDAO;
            this.logDAO = logDAO;
        }

        [HttpPost]
        [Route("api/Parceiro/Adicionar/{idLoja}")]
        public HttpResponseMessage AdicionarParceiro([FromUri] int idLoja, [FromBody] Parceiro parceiro)
        {
            try
            {
                parceiroDAO.AdicionarParceiro(idLoja, parceiro);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível cadastrar o parceiro. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/Parceiro/BuscarParceiro/{id}/{idLoja}")]
        public HttpResponseMessage BuscarParceiro(int id, int idLoja)
        {
            try
            {
                Parceiro parceiro = new Parceiro();
                parceiro = parceiroDAO.BuscarParceiro(id, idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, parceiro);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = "Parceiro não encontrado";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            catch (Exception)
            {
                string mensagem = "Não foi consultar o parceiro. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/Parceiro/BuscarParceiroPorLoja/{idLoja}")]
        public HttpResponseMessage BuscarParceiroPorLoja(int idLoja)
        {
            try
            {
                List<Parceiro> listaParceiros = new List<Parceiro>();
                listaParceiros = parceiroDAO.BuscarParceiroPorLoja(idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, listaParceiros);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar os parceiros. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Parceiro/Atualizar")]
        public HttpResponseMessage AtualizarParceiro([FromBody] Parceiro parceiro)
        {
            try
            {
                parceiroDAO.Atualizar(parceiro);
                return Request.CreateResponse(HttpStatusCode.OK, parceiro);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o parceiro. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Parceiro/Excluir")]
        public HttpResponseMessage ExcluirParceiro([FromBody] Parceiro parceiro)
        {
            try
            {
                parceiroDAO.Excluir(parceiro);
                return Request.CreateResponse(HttpStatusCode.OK, parceiro);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o parceiro. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Parceiro/Desativar")]
        public HttpResponseMessage Desativar([FromBody] Parceiro parceiro)
        {
            try
            {
                parceiroDAO.Desativar(parceiro);
                return Request.CreateResponse(HttpStatusCode.OK, parceiro);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar o parceiro. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
