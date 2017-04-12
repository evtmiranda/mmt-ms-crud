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

        /// <summary>
        /// Retorna uma lista de forma de pagamento para a loja enviada como parâmetro
        /// </summary>
        /// <param name="idLoja">Id da loja para a qual se quer as formas de pagamento</param>
        /// <returns></returns>
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

        //public FormaDePagamento BuscarFormaPagamento(string nomeFormaPagamento, int idParceiro)
        //{
        //    return formaPagamentoDAO.BuscarPorNome(nomeFormaPagamento, idParceiro);
        //}
    }
}
