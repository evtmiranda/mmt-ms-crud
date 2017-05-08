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

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public PedidoController(PedidoDAO pedidoDAO, LogDAO logDAO, FormaPagamentoDAO pagamentoDAO)
        {
            this.pedidoDAO = pedidoDAO;
            this.logDAO = logDAO;
            this.pagamentoDAO = pagamentoDAO;
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

                //formata o campo troco para decimal
                if (string.IsNullOrEmpty(pedido.Troco))
                    pedido.Troco = "0.00";

                //adiciona o pedido
                pedido.Id = pedidoDAO.AdicionarPedido(pedido);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, pedido);

                return response;
            }
            catch (PedidoNaoCadastradoClienteException pncEx)
            {
                //string mensagem = pncEx.Message;
                //HttpError error = new HttpError(mensagem);
                //return Request.CreateResponse(HttpStatusCode.NotModified, error);

                string mensagem = string.Format("nao foi possivel cadastrar o pedido. erro: {0}", pncEx);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o pedido. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Busca todos os pedidos de uma determinada loja
        /// </summary>
        /// <param name="idLoja">Id da loja</param>
        /// <param name="ehDoDia">Diz se quer buscar os pedidos do dia</param>
        /// <param name="ehPedidoFila">Diz se quer buscar os pedidos que estão na fila de entrega</param>
        /// <param name="ehPedidoAndamento">Diz se quer buscar os pedidos que estão em andamento</param>
        /// <param name="ehPedidoEntregue">Diz se quer buscar os pedidos que já foram entregues</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Pedido/BuscarPedidos/{idLoja}/{ehDoDia}/{ehPedidoFila}/{ehPedidoAndamento}/{ehPedidoEntregue}")]
        public HttpResponseMessage BuscarPedidos(int idLoja, bool ehDoDia, bool ehPedidoFila = false, bool ehPedidoAndamento = false, bool ehPedidoEntregue = false)
        {
            List<Pedido> listaPedidosCliente = new List<Pedido>();

            try
            {
                //busca os pedidos
                listaPedidosCliente = pedidoDAO.ConsultarPedidosLoja(idLoja, ehDoDia, ehPedidoFila, ehPedidoAndamento, ehPedidoEntregue);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, listaPedidosCliente);

                return response;
            }
            catch (LojaNaoPossuiPedidosException)
            {
                //string mensagem = cnpEx.Message;
                //HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel consultar os pedidos. erro: {0}", ex);
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
        [Route("api/Pedido/BuscarHistorico/{idUsuarioParceiro}")]
        public HttpResponseMessage BuscarHistoricoPedidos(int idUsuarioParceiro)
        {
            List<Pedido> listaPedidosCliente = new List<Pedido>();
            
            try
            {
                //busca os pedidos
                listaPedidosCliente = pedidoDAO.ConsultarPedidosCliente(idUsuarioParceiro);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, listaPedidosCliente);

                return response;
            }
            catch (ClienteNuncaFezPedidosException)
            {
                //string mensagem = cnpEx.Message;
                //HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel consultar os pedidos. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [Route("api/Pedido/AtualizarStatusPedido")]
        public HttpResponseMessage AtualizarStatusPedido([FromBody] DadosAtualizarStatusPedido dadosPedido)
        {
            Pedido pedido = new Pedido();
            try
            {
                Pedido pedidoAtual = pedidoDAO.BuscarPorId(dadosPedido.IdPedido);

                pedidoAtual.PedidoStatus = new PedidoStatus()
                {
                    IdStatus = dadosPedido.IdStatusPedido
                };

                pedidoDAO.AtualizarStatusPedido(pedidoAtual);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(PedidoNaoEncontradoException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel atualizar o pedido. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

    }
}
