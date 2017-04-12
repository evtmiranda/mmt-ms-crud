using System;
using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class HorarioEntregaDAO : GenericDAO<HorarioEntrega>
    {
        public HorarioEntregaDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override List<HorarioEntrega> Listar(int idLoja)
        {
            List<HorarioEntregaEntidade> listaHorariosEntidade = new List<HorarioEntregaEntidade>();
            List<HorarioEntrega> listaHorarios = new List<HorarioEntrega>();

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_horario_entrega,
	                                                            id_loja,
	                                                            nm_horario,
	                                                            bol_ativo
                                                            FROM tab_horario_entrega
                                                            WHERE id_loja = @id_loja
                                                            AND bol_ativo = 1;");

                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaHorariosEntidade = new ModuloClasse().PreencheClassePorDataReader<HorarioEntregaEntidade>(sqlConn.Reader);

                //transforma a entidade em objeto
                foreach (var horario in listaHorariosEntidade)
                    listaHorarios.Add(horario.ToHorarioEntrega());

                //verifica se o retorno foi positivo
                if (listaHorarios.Count == 0)
                    throw new HorarioNaoEncontradoException();

                //fecha o reader
                sqlConn.Reader.Close();


                //habilita ou não um horário de entrega de acordo com o tempo mínimo de atecedência definido
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            nr_minutos_antecedencia
                                                            FROM tab_horario_entrega_tempo_anteced_pedido
                                                            WHERE id_loja = @id_loja
                                                            AND bol_ativo = 1;");

                string tempoAntecedencia = sqlConn.Command.ExecuteScalar().ToString();

                DateTime DataMinimoAntecedencia = DateTime.Now.AddMinutes(Convert.ToDouble(tempoAntecedencia));

                foreach (var horario in listaHorarios)
                {
                    if (!(DataMinimoAntecedencia > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + horario.Horario)))
                        horario.HorarioDisponivel = true;
                }

                return listaHorarios;
            }
            catch (HorarioNaoEncontradoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "erro ao buscar os horários de entrega", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }
    }
}
