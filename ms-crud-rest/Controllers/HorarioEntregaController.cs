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
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o horário de entrega. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/HorarioEntrega/{id}")]
        public HttpResponseMessage Buscar(int id)
        {
            try
            {
                HorarioEntrega horarioEntrega = new HorarioEntrega();

                horarioEntrega = horarioEntregaDAO.BuscarPorId(id);

                if (horarioEntrega == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, horarioEntrega);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar o horário de entrega. Por favor, tente novamente ou entre em contato com o administrador do sistema";
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
                string mensagem = "Não foi possível atualizar o horário de entrega. Por favor, tente novamente ou entre em contato com o administrador do sistema";
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
                string mensagem = "Não foi possível excluir o horário de entrega. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Retorna os horários de entrega de uma determinada loja
        /// </summary>
        /// <param name="idLoja"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/HorarioEntrega/Listar/{idLoja}")]
        public HttpResponseMessage ListarHorarios(int idLoja)
        {
            try
            {
                DadosHorarioEntrega horariosEntrega = horarioEntregaDAO.ListarHorariosEntrega(idLoja);
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


        #region tempo antecedencia

        [HttpGet]
        [Route("api/HorarioEntrega/TempoAntecedencia/{id}")]
        public HttpResponseMessage BuscarTempoAntecedencia(int id)
        {
            try
            {
                TempoAntecedenciaEntrega tempoAntecedenciaEntrega = new TempoAntecedenciaEntrega();

                tempoAntecedenciaEntrega = horarioEntregaDAO.BuscarTempoAntecedencia(id);

                if (tempoAntecedenciaEntrega == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, tempoAntecedenciaEntrega);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível consultar o tempo de antecedencia. Por favor, tente novamente ou entre em contato com o administrador do sistema";
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
                string mensagem = "Não foi possível atualizar o tempo de antecedência. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #endregion
    }
}
