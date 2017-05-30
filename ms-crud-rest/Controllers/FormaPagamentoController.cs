using ClassesMarmitex;
using ms_crud_rest.DAO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ms_crud_rest.Controllers
{
    public class FormaPagamentoController : ApiController
    {
        private FormaPagamentoDAO formaPagamentoDAO;
        private LogDAO logDAO;

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public FormaPagamentoController(FormaPagamentoDAO formaPagamentoDAO, LogDAO logDAO)
        {
            this.formaPagamentoDAO = formaPagamentoDAO;
            this.logDAO = logDAO;
        }

        [HttpPost]
        [Route("api/FormaPagamento/Adicionar")]
        public HttpResponseMessage Adicionar([FromBody] FormaDePagamento formaPagamento)
        {
            try
            {
                formaPagamentoDAO.Adicionar(formaPagamento);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível cadastrar a forma de pagamento. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/FormaPagamento/{id}/{idLoja}")]
        public HttpResponseMessage Buscar(int id, int idLoja)
        {
            try
            {
                FormaDePagamento pagamento = new FormaDePagamento();

                pagamento = formaPagamentoDAO.BuscarPorId(id, idLoja);

                return Request.CreateResponse(HttpStatusCode.OK, pagamento);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar a forma de pagamento. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/FormaPagamento/Listar/{idLoja}")]
        public HttpResponseMessage ListarFormasPagamento(int idLoja)
        {
            try
            {
                IList<FormaDePagamento> formasPagamento = formaPagamentoDAO.Listar(idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, formasPagamento);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar as formas de pagamento. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/FormaPagamento/Atualizar")]
        public HttpResponseMessage Atualizar([FromBody] FormaDePagamento formaPagamento)
        {
            try
            {
                formaPagamentoDAO.Atualizar(formaPagamento);
                return Request.CreateResponse(HttpStatusCode.OK, formaPagamento);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar a forma de pagamento. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/FormaPagamento/Excluir")]
        public HttpResponseMessage Excluir([FromBody] FormaDePagamento formaPagamento)
        {
            try
            {
                formaPagamentoDAO.Excluir(formaPagamento);
                return Request.CreateResponse(HttpStatusCode.OK, formaPagamento);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir a forma de pagamento. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/FormaPagamento/Desativar")]
        public HttpResponseMessage Desativar([FromBody] FormaDePagamento formaPagamento)
        {
            try
            {
                formaPagamentoDAO.Desativar(formaPagamento);
                return Request.CreateResponse(HttpStatusCode.OK, formaPagamento);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar a forma de pagamento. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

    }
}
