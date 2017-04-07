using ClassesMarmitex;
using ms_crud_rest.DAO;
using ms_crud_rest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ms_crud_rest.Controllers
{
    public class PedidoController : ApiController
    {
        public PedidoDAO pedidoDAO;
        public LogDAO logDAO;
        public FormaPagamentoDAO pagamentoDAO;

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public PedidoController(PedidoDAO pedidoDAO, LogDAO logDAO, FormaPagamentoDAO pagamentoDAO)
        {
            this.pedidoDAO = pedidoDAO;
            this.logDAO = logDAO;
            this.pagamentoDAO = pagamentoDAO;
        }

        [HttpPost]
        [Route("api/pedido/cadastrar")]
        public HttpResponseMessage CadastrarPedido([FromBody] Pedido pedido)
        {
            try
            {
                //preenchee a forma de pagamento
                for (int i = 0; i < pedido.ListaFormaPagamento.Count; i++)
                {
                    pedido.ListaFormaPagamento[i] = pagamentoDAO.BuscarPorNome(pedido.ListaFormaPagamento[i].Nome, pedido.Cliente.IdParceiro);
                }

                pedidoDAO.Adicionar(pedido);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                return response;
            }
            catch (PedidoNaoCadastradoClienteException pncEx)
            {
                string mensagem = pncEx.Message;
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotModified, error);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o pedido. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
