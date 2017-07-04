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

                    pedido.ListaFormaPagamento[i] = pagamentoDAO.BuscarPorNome(pedido.ListaFormaPagamento[i].Nome, pedido.Cliente.IdLoja);
                }

                //remove as formas de pagamento com id = 0
                pedido.ListaFormaPagamento.RemoveAll(p => p.Id == 0);

                //formata o campo troco para decimal
                if (string.IsNullOrEmpty(pedido.Troco.ToString()))
                    pedido.Troco = 0;

                //adiciona o pedido
                pedido.Id = pedidoDAO.AdicionarPedido(pedido);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, pedido);

                return response;
            }
            catch (Exception)
            {
                string mensagem = string.Format("Não foi possivel cadastrar o pedido. Por favor, tente novamente ou entre em contato com nosso suporte.");
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Busca todos os pedidos de uma determinada loja
        /// </summary>
        /// <param name="idLoja">Id da loja</param>
        /// <param name="ehDoDia">Diz se quer buscar os pedidos do dia</param>
        /// <param name="estadoPedido">Estado do pedido que deseja buscar. Na fila, em andamento ou entregue</param>
        [HttpGet]
        [Route("api/Pedido/BuscarPedidos/{idLoja}/{ehDoDia}")]
        public HttpResponseMessage BuscarPedidos(int idLoja, bool ehDoDia, EstadoPedido estadoPedido = EstadoPedido.Todos)
        {
            List<Pedido> listaPedidosCliente = new List<Pedido>();

            try
            {
                //busca os pedidos
                listaPedidosCliente = pedidoDAO.ConsultarPedidosLoja(idLoja, ehDoDia, estadoPedido);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, listaPedidosCliente);

                return response;
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar os pedidos. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Busca o e-mail do usuário que realizou determinado pedido
        /// </summary>
        /// <param name="idPedido">Id do pedido</param>
        /// <param name="idLoja">Id da loja onde o pedido foi realizado</param>
        /// <returns>e-mail do usuário que realizou o pedido</returns>
        [HttpGet]
        [Route("api/Pedido/BuscarEmailUsuarioPedido/{idPedido}/{idLoja}")]
        public HttpResponseMessage BuscarEmailUsuarioPedido(int idPedido, int idLoja)
        {
            string emailUsuario;

            try
            {
                emailUsuario = pedidoDAO.ConsultarEmailUsuarioPedido(idPedido, idLoja);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, emailUsuario);

                return response;
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar o e-mail do usuário. Por favor, tente novamente ou entre em contato com nosso suporte.";
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
        [Route("api/Pedido/BuscarHistorico/{idUsuarioParceiro}/{idLoja}")]
        public HttpResponseMessage BuscarHistoricoPedidos(int idUsuarioParceiro, int idLoja)
        {
            List<Pedido> listaPedidosCliente = new List<Pedido>();
            
            try
            {
                listaPedidosCliente = pedidoDAO.ConsultarPedidosCliente(idUsuarioParceiro, idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, listaPedidosCliente);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar o histórico de pedidos. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Pedido/AtualizarStatusPedido/{idLoja}")]
        public HttpResponseMessage AtualizarStatusPedido([FromBody] DadosAtualizarStatusPedido dadosPedido, int idLoja)
        {
            Pedido pedido = new Pedido();

            try
            {
                Pedido pedidoAtual = pedidoDAO.BuscarPorId(dadosPedido.IdPedido, idLoja);

                pedidoAtual.PedidoStatus = new PedidoStatus()
                {
                    IdStatus = dadosPedido.IdStatusPedido
                };

                pedidoDAO.AtualizarStatusPedido(pedidoAtual, dadosPedido.MotivoCancelamento);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o pedido. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/Pedido/CancelarPedido/{idPedido}/{idLoja}/{motivoCancelamento}")]
        public HttpResponseMessage CancelarPedido(int idPedido, int idLoja, string motivoCancelamento)
        {
            Pedido pedido = new Pedido();

            try
            {
                Pedido pedidoAtual = pedidoDAO.BuscarPorId(idPedido, idLoja);

                pedidoAtual.PedidoStatus = new PedidoStatus()
                {
                    IdStatus = EstadoPedido.Cancelado.GetHashCode()
                };

                pedidoDAO.AtualizarStatusPedido(pedidoAtual, motivoCancelamento);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível cancelar o pedido. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

    }
}
