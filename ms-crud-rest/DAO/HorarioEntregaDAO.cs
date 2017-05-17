using System;
using ClassesMarmitex;
using ms_crud_rest.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ms_crud_rest.DAO
{
    public class HorarioEntregaDAO : GenericDAO<HorarioEntrega>
    {
        public HorarioEntregaDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override HorarioEntrega BuscarPorId(int id)
        {
            HorarioEntrega horarioEntrega;
            List<HorarioEntregaEntidade> listahorarioEntregaEntidade;

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
                                                            WHERE id_horario_entrega = @id_horario_entrega
                                                            AND bol_ativo = 1;");

                sqlConn.Command.Parameters.AddWithValue("@id_horario_entrega", id);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listahorarioEntregaEntidade = new ModuloClasse().PreencheClassePorDataReader<HorarioEntregaEntidade>(sqlConn.Reader);

                if (listahorarioEntregaEntidade.Count == 0)
                    throw new KeyNotFoundException();

                horarioEntrega = listahorarioEntregaEntidade[0].ToHorarioEntrega();

                //fecha o reader
                sqlConn.Reader.Close();

                return horarioEntrega;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar o horário de entrega", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }

        public DadosHorarioEntrega ListarHorariosEntrega(int idLoja)
        {
            List<HorarioEntregaEntidade> listaHorariosEntidade = new List<HorarioEntregaEntidade>();
            List<HorarioEntrega> listaHorarios = new List<HorarioEntrega>();

            TempoAntecedenciaEntrega tempoAntecedencia = new TempoAntecedenciaEntrega();
            List<TempoAntecedenciaEntregaEntidade> listaTempoAntecedenciaEntidade = new List<TempoAntecedenciaEntregaEntidade>();

            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region horário de entrega

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

                #endregion

                #region tempo de antecedencia

                //habilita ou não um horário de entrega de acordo com o tempo mínimo de atecedência definido
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_tempo_antecedencia,
	                                                            id_loja,
	                                                            nr_minutos_antecedencia,
	                                                            bol_ativo
                                                            FROM tab_horario_entrega_tempo_anteced_pedido
                                                            WHERE id_loja = @id_loja
                                                            AND bol_ativo = 1;");

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaTempoAntecedenciaEntidade = new ModuloClasse().PreencheClassePorDataReader<TempoAntecedenciaEntregaEntidade>(sqlConn.Reader);

                if (listaTempoAntecedenciaEntidade.Count == 0)
                    throw new Exception("Não foi possível consultar o tempo de mínimo de antecedência de pedidos");

                tempoAntecedencia = listaTempoAntecedenciaEntidade.FirstOrDefault().ToTempoAntecedenciaEntrega();

                #endregion


                #region monta o objeto com a lista de horários e o tempo de antecedência de entrega

                DadosHorarioEntrega dadosHorarioEntrega = new DadosHorarioEntrega()
                {
                    HorariosEntrega = listaHorarios,
                    TempoAntecedenciaEntrega = tempoAntecedencia
                };

                #endregion

                return dadosHorarioEntrega;
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

        public override void Adicionar(HorarioEntrega horarioEntrega)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_horario_entrega(id_loja, nm_horario)
                                                                VALUES(@id_loja, @nm_horario);");

                sqlConn.Command.Parameters.AddWithValue("@id_loja", horarioEntrega.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_horario", horarioEntrega.Horario);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao cadastrar o horário de entrega", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Atualizar(HorarioEntrega horarioEntrega)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.AddWithValue("@nm_horario", horarioEntrega.Horario);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", horarioEntrega.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_horario_entrega", horarioEntrega.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_horario_entrega
	                                                            SET nm_horario = @nm_horario,
		                                                            bol_ativo = @bol_ativo
                                                            WHERE id_horario_entrega = @id_horario_entrega;");

                sqlConn.Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar o horário de entrega", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Excluir(HorarioEntrega horarioEntrega)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", horarioEntrega.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_horario_entrega", horarioEntrega.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_horario_entrega
	                                                            SET bol_ativo = @bol_ativo
                                                            WHERE id_horario_entrega = @id_horario_entrega;");

                sqlConn.Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar o horário de entrega", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        #region tempo antecedencia

        public TempoAntecedenciaEntrega BuscarTempoAntecedencia(int id)
        {
            TempoAntecedenciaEntrega tempoAntecedenciaEntrega;
            List<TempoAntecedenciaEntregaEntidade> listaTempoAntecedenciaEntregaEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = string.Format(@"SELECT
	                                                            id_tempo_antecedencia,
	                                                            id_loja,
	                                                            nr_minutos_antecedencia,
	                                                            bol_ativo
                                                            FROM tab_horario_entrega_tempo_anteced_pedido
                                                            WHERE id_tempo_antecedencia = @id_tempo_antecedencia
                                                            AND bol_ativo = 1;");

                sqlConn.Command.Parameters.AddWithValue("@id_tempo_antecedencia", id);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaTempoAntecedenciaEntregaEntidade = new ModuloClasse().PreencheClassePorDataReader<TempoAntecedenciaEntregaEntidade>(sqlConn.Reader);

                if (listaTempoAntecedenciaEntregaEntidade.Count == 0)
                    throw new KeyNotFoundException();

                tempoAntecedenciaEntrega = listaTempoAntecedenciaEntregaEntidade[0].ToTempoAntecedenciaEntrega();

                //fecha o reader
                sqlConn.Reader.Close();

                return tempoAntecedenciaEntrega;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao buscar o tempo de antecedência", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
                sqlConn.Reader.Close();
            }
        }

        public void AtualizarTempoAntecedencia(TempoAntecedenciaEntrega tempoAntecedenciaEntrega)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.AddWithValue("@nr_minutos_antecedencia", tempoAntecedenciaEntrega.MinutosAntecedencia);
                sqlConn.Command.Parameters.AddWithValue("@id_loja", tempoAntecedenciaEntrega.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@id_tempo_antecedencia", tempoAntecedenciaEntrega.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_horario_entrega_tempo_anteced_pedido
	                                                            SET nr_minutos_antecedencia = @nr_minutos_antecedencia
                                                            WHERE id_tempo_antecedencia = @id_tempo_antecedencia
                                                            AND id_loja = @id_loja;");

                sqlConn.Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { Mensagem = "Erro ao atualizar o tempo de antecedência", Descricao = ex.Message, StackTrace = ex.StackTrace == null ? "" : ex.StackTrace });

                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        #endregion
    }
}
