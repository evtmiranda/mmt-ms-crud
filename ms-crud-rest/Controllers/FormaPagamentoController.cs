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

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                return response;
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar a forma de pagamento. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/FormaPagamento/{id}")]
        public HttpResponseMessage Buscar(int id)
        {
            try
            {
                FormaDePagamento pagamento = new FormaDePagamento();

                pagamento = formaPagamentoDAO.BuscarPorId(id);

                if (pagamento == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, pagamento);
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
                string mensagem = "Não foi possível consultar a forma de pagamento. Por favor, tente novamente ou entre em contato com o administrador do sistema";
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
            catch (PagamentoNaoEncontradoException pneEx)
            {
                string mensagem = pneEx.Message;
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            catch (Exception)
            {
                string mensagem = string.Format("ocorreu um problema ao buscar as formas de pagamento. por favor, tente atualizar a página ou acessar dentro de alguns minutos...");
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
                string mensagem = "Não foi possível atualizar a forma de pagamento. Por favor, tente novamente ou entre em contato com o administrador do sistema";
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
                string mensagem = "Não foi possível excluir a forma de pagamento. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

    }
}
