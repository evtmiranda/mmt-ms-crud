using ClassesMarmitex;
using ms_crud_rest.DAO;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        [Route("api/Parceiro/Adicionar/{urlLoja}")]
        public HttpResponseMessage AdicionarParceiro([FromUri] string urlLoja, [FromBody] Parceiro parceiro)
        {
            try
            {
                //verifica se já existe um parceiro com este nome
                Parceiro parceiroExiste = new Parceiro();
                parceiroExiste = parceiroDAO.BuscarParceiro(parceiro.Nome);

                if (parceiroExiste.Id != 0)
                    throw new ParceiroJaExisteException();

                parceiroDAO.AdicionarParceiro(urlLoja, parceiro);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                return response;
            }
            catch (ParceiroJaExisteException pjeEx)
            {
                string mensagem = pjeEx.Message;
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, error);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o parceiro. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Faz a busca de um parceiro pelo id parceiro
        /// </summary>
        /// <param name="idParceiro">id do parceiro</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Parceiro/BuscarParceiro/{id}")]
        public HttpResponseMessage BuscarParceiro(int id)
        {
            try
            {
                Parceiro parceiro = new Parceiro();

                parceiro = parceiroDAO.BuscarParceiro(id);

                if (parceiro == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, parceiro);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (ParceiroNaoEncontradoException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar os parceiros. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Faz a busca dos parceiros de uma determinada loja
        /// </summary>
        /// <param name="idLoja">id do parceiro</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Parceiro/BuscarParceiroPorLoja/{idLoja}")]
        public HttpResponseMessage BuscarParceiroPorLoja(int idLoja)
        {
            try
            {
                List<Parceiro> listaParceiros = new List<Parceiro>();

                listaParceiros = parceiroDAO.BuscarParceiroPorLoja(idLoja);

                if (listaParceiros == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, listaParceiros);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (ParceiroNaoEncontradoException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar os parceiros. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
