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
    public class PedidoController : ApiController
    {
        public PedidoDAO pedidoDAO;
        public LogDAO logDAO;
        public FormaPagamentoDAO pagamentoDAO;
        public PedidoClienteDAO pedidoClienteDAO;

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public PedidoController(PedidoDAO pedidoDAO, LogDAO logDAO, FormaPagamentoDAO pagamentoDAO, PedidoClienteDAO pedidoClienteDAO)
        {
            this.pedidoDAO = pedidoDAO;
            this.logDAO = logDAO;
            this.pagamentoDAO = pagamentoDAO;
            this.pedidoClienteDAO = pedidoClienteDAO;
        }

        /// <summary>
        /// Faz o cadastro de um pedido
        /// </summary>
        /// <param name="pedido">objeto com todos os dados do pedido</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Pedido/Cadastrar")]
        public HttpResponseMessage CadastrarPedido([FromBody] Pedido pedido)
        {
            try
            {
                //preenchee a forma de pagamento
                for (int i = 0; i < pedido.ListaFormaPagamento.Count; i++)
                {
                    if (pedido.ListaFormaPagamento[i].Nome == null)
                        continue;

                    pedido.ListaFormaPagamento[i] = pagamentoDAO.BuscarPorNome(pedido.ListaFormaPagamento[i].Nome, pedido.Cliente.IdParceiro);
                }

                //remove as formas de pagamento com id = 0
                pedido.ListaFormaPagamento.RemoveAll(p => p.Id == 0);

                //adiciona o pedido
                pedido.Id = pedidoDAO.AdicionarPedido(pedido);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, pedido);

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

        /// <summary>
        /// Busca todos os pedidos de um determinado cliente
        /// </summary>
        /// <param name="idUsuarioParceiro">id do cliente para consultar os pedidos</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Pedido/BuscarHistorico")]
        public HttpResponseMessage BuscarHistoricoPedidos([FromUri] int idUsuarioParceiro)
        {
            List<PedidoCliente> listaPedidosCliente = new List<PedidoCliente>();
            
            try
            {
                //busca os pedidos
                listaPedidosCliente = pedidoClienteDAO.Listar(idUsuarioParceiro);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, listaPedidosCliente);

                return response;
            }
            catch (ClienteNuncaFezPedidosException cnpEx)
            {
                string mensagem = cnpEx.Message;
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotModified, error);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel consultar os pedidos. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
