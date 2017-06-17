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
                return Request.CreateResponse(HttpStatusCode.Created);
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

        [HttpPost]
        [Route("api/HorarioEntrega/Desativar")]
        public HttpResponseMessage Desativar([FromBody] HorarioEntrega horarioEntrega)
        {
            try
            {
                horarioEntregaDAO.Desativar(horarioEntrega);
                return Request.CreateResponse(HttpStatusCode.OK, horarioEntrega);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar o horário de entrega. Por favor, tente novamente ou entre em contato com nosso suporte.";
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

        #region tempo antecedencia cancelamento

        [HttpGet]
        [Route("api/HorarioEntrega/TempoAntecedenciaCancelamento/{id}/{idLoja}")]
        public HttpResponseMessage BuscarTempoAntecedenciaCancelamento(int id, int idLoja)
        {
            try
            {
                TempoAntecedenciaCancelamentoEntrega tempoAntecedenciaCancelamentoEntrega = new TempoAntecedenciaCancelamentoEntrega();

                tempoAntecedenciaCancelamentoEntrega = horarioEntregaDAO.BuscarTempoAntecedenciaCancelamento(id, idLoja);

                return Request.CreateResponse(HttpStatusCode.OK, tempoAntecedenciaCancelamentoEntrega);
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

        [HttpGet]
        [Route("api/HorarioEntrega/TempoAntecedenciaCancelamento/{idLoja}")]
        public HttpResponseMessage BuscarTempoAntecedenciaCancelamento(int idLoja)
        {
            try
            {
                TempoAntecedenciaCancelamentoEntrega tempoAntecedenciaCancelamentoEntrega = new TempoAntecedenciaCancelamentoEntrega();

                tempoAntecedenciaCancelamentoEntrega = horarioEntregaDAO.BuscarTempoAntecedenciaCancelamento(idLoja);

                return Request.CreateResponse(HttpStatusCode.OK, tempoAntecedenciaCancelamentoEntrega);
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
        [Route("api/HorarioEntrega/TempoAntecedenciaCancelamento/Atualizar")]
        public HttpResponseMessage AtualizarTempoAntecedenciaCancelamento([FromBody] TempoAntecedenciaCancelamentoEntrega tempoAntecedenciaCancelamentoEntrega)
        {
            try
            {
                horarioEntregaDAO.AtualizarTempoAntecedenciaCancelamento(tempoAntecedenciaCancelamentoEntrega);

                return Request.CreateResponse(HttpStatusCode.OK, tempoAntecedenciaCancelamentoEntrega);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o tempo de antecedência. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/HorarioEntrega/TempoAntecedenciaCancelamento/PermitirCancelamento/{id}/{idLoja}")]
        public HttpResponseMessage PermitirCancelamento(int id, int idLoja)
        {
            try
            {
                bool permitirCancelamento = false;

                permitirCancelamento = horarioEntregaDAO.PermitirCancelamento(id, idLoja);

                return Request.CreateResponse(HttpStatusCode.OK, permitirCancelamento);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível verificar se é permitido cancelar. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #endregion

        #region dias de funcionamento

        [HttpPost]
        [Route("api/HorarioEntrega/DiaFuncionamento/Desativar")]
        public HttpResponseMessage DesativarDiaFuncionamento([FromBody] DiasDeFuncionamento diaFuncionamento)
        {
            try
            {
                horarioEntregaDAO.AtualizarDiaFuncionamento(diaFuncionamento);
                return Request.CreateResponse(HttpStatusCode.OK, diaFuncionamento);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o dia de funcionamento. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #endregion
    }
}
