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

        [HttpPost]
        [Route("api/HorarioEntrega/Adicionar")]
        public HttpResponseMessage Adicionar([FromBody] HorarioEntrega horarioEntrega)
        {
            try
            {
                horarioEntregaDAO.Adicionar(horarioEntrega);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                return response;
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível cadastrar o horário de entrega. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/HorarioEntrega/{id}/{idLoja}")]
        public HttpResponseMessage Buscar(int id, int idLoja)
        {
            try
            {
                HorarioEntrega horarioEntrega = new HorarioEntrega();

                horarioEntrega = horarioEntregaDAO.BuscarPorId(id, idLoja);

                return Request.CreateResponse(HttpStatusCode.OK, horarioEntrega);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar o horário de entrega. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/HorarioEntrega/Atualizar")]
        public HttpResponseMessage Atualizar([FromBody] HorarioEntrega horarioEntrega)
        {
            try
            {
                horarioEntregaDAO.Atualizar(horarioEntrega);

                return Request.CreateResponse(HttpStatusCode.OK, horarioEntrega);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o horário de entrega. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/HorarioEntrega/Excluir")]
        public HttpResponseMessage Excluir([FromBody] HorarioEntrega horarioEntrega)
        {
            try
            {
                horarioEntregaDAO.Excluir(horarioEntrega);
                return Request.CreateResponse(HttpStatusCode.OK, horarioEntrega);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o horário de entrega. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/HorarioEntrega/Listar/{idLoja}")]
        public HttpResponseMessage ListarHorarios(int idLoja)
        {
            try
            {
                DadosHorarioEntrega horariosEntrega = horarioEntregaDAO.ListarHorariosEntrega(idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, horariosEntrega);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar os horários de entrega. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #region tempo antecedencia

        [HttpGet]
        [Route("api/HorarioEntrega/TempoAntecedencia/{id}/{idLoja}")]
        public HttpResponseMessage BuscarTempoAntecedencia(int id, int idLoja)
        {
            try
            {
                TempoAntecedenciaEntrega tempoAntecedenciaEntrega = new TempoAntecedenciaEntrega();

                tempoAntecedenciaEntrega = horarioEntregaDAO.BuscarTempoAntecedencia(id, idLoja);

                return Request.CreateResponse(HttpStatusCode.OK, tempoAntecedenciaEntrega);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar o tempo de antecedência. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/HorarioEntrega/TempoAntecedencia/Atualizar")]
        public HttpResponseMessage AtualizarTempoAntecedencia([FromBody] TempoAntecedenciaEntrega tempoAntecedenciaEntrega)
        {
            try
            {
                horarioEntregaDAO.AtualizarTempoAntecedencia(tempoAntecedenciaEntrega);

                return Request.CreateResponse(HttpStatusCode.OK, tempoAntecedenciaEntrega);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o tempo de antecedência. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #endregion
    }
}
