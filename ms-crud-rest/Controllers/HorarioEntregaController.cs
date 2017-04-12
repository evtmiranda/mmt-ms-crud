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
    public class HorarioEntregaController : ApiController
    {
        private HorarioEntregaDAO horarioEntregaDAO;
        private LogDAO logDAO;

        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public HorarioEntregaController(HorarioEntregaDAO horarioEntregaDAO, LogDAO logDAO)
        {
            this.horarioEntregaDAO = horarioEntregaDAO;
            this.logDAO = logDAO;
        }

        /// <summary>
        /// Retorna os horários de entrega de uma determinada loja
        /// </summary>
        /// <param name="idParceiro"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/HorarioEntrega/Listar/{idLoja}")]
        public HttpResponseMessage ListarHorarios(int idLoja)
        {
            try
            {
                IList<HorarioEntrega> horariosEntrega = horarioEntregaDAO.Listar(idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, horariosEntrega);
            }
            catch (HorarioNaoEncontradoException hreEx)
            {
                string mensagem = hreEx.Message;
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            catch (Exception)
            {
                string mensagem = string.Format("ocorreu um problema ao buscar os horários de entrega. por favor, tente atualizar a página ou acessar dentro de alguns minutos...");
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
