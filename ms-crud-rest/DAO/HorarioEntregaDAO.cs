using System;
using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class HorarioEntregaDAO : GenericDAO<HorarioEntrega>
    {
        public HorarioEntregaDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override List<HorarioEntrega> Listar(int idParceiro)
        {
            List<HorarioEntregaEntidade> listaHorariosEntidade = new List<HorarioEntregaEntidade>();
            List<HorarioEntrega> listaHorarios = new List<HorarioEntrega>();

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            the.id_horario_entrega,
	                                                            the.id_loja,
	                                                            the.nm_horario,
	                                                            the.bol_ativo
                                                            FROM tab_horario_entrega AS the
                                                            INNER JOIN tab_parceiro AS tp
                                                            ON tp.id_loja = the.id_loja
                                                            WHERE tp.id_parceiro = @id_parceiro
                                                            AND the.bol_ativo = 1;");

                sqlConn.Command.Parameters.AddWithValue("@id_parceiro", idParceiro);

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
                                                            FROM tab_horario_entrega_tempo_anteced_pedido AS ttap
                                                            INNER JOIN tab_parceiro AS tp
                                                            ON tp.id_loja = ttap.id_loja
                                                            WHERE tp.id_parceiro = @id_parceiro
                                                            AND ttap.bol_ativo = 1;");

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
