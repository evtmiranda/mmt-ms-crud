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

        public override void Adicionar(HorarioEntrega horarioEntrega)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = @"INSERT INTO tab_horario_entrega(id_loja, nm_horario)
                                                VALUES(@id_loja, @nm_horario)";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", horarioEntrega.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_horario", horarioEntrega.Horario);

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = horarioEntrega.IdLoja, Mensagem = "Erro ao cadastrar o horário de entrega", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override HorarioEntrega BuscarPorId(int id, int idLoja)
        {
            HorarioEntrega horarioEntrega;
            List<HorarioEntregaEntidade> listahorarioEntregaEntidade;

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = @"SELECT
	                                                id_horario_entrega,
	                                                id_loja,
	                                                nm_horario,
	                                                bol_ativo
                                                FROM tab_horario_entrega
                                                WHERE id_horario_entrega = @id_horario_entrega
                                                AND bol_excluido = 0";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_horario_entrega", id);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listahorarioEntregaEntidade = new ModuloClasse().PreencheClassePorDataReader<HorarioEntregaEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException();

                horarioEntrega = listahorarioEntregaEntidade[0].ToHorarioEntrega();

                return horarioEntrega;
            }
            //sempre que for realizado uma busca por id, é necessário que o recurso exista. se o recurso não existir, é um erro interno, por este motivo
            //o log é gravado
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Horario de entrega nao encontrado com id " + id, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar o horario de entrega com id " + id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if(sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        public override void Atualizar(HorarioEntrega horarioEntrega)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@nm_horario", horarioEntrega.Horario);
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", horarioEntrega.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_horario_entrega", horarioEntrega.Id);

                sqlConn.Command.CommandText = @"UPDATE tab_horario_entrega
	                                                SET nm_horario = @nm_horario,
		                                                bol_ativo = @bol_ativo
                                                WHERE id_horario_entrega = @id_horario_entrega";

                sqlConn.Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = horarioEntrega.IdLoja, Mensagem = "Erro ao atualizar o horário de entrega", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
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

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_horario_entrega", horarioEntrega.Id);

                sqlConn.Command.CommandText = @"UPDATE tab_horario_entrega
                                                    SET bol_excluido = 1, bol_ativo = 0
                                                WHERE id_horario_entrega = @id_horario_entrega";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = horarioEntrega.IdLoja, Mensagem = "Erro ao excluir o horário de entrega", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public override void Desativar(HorarioEntrega horarioEntrega)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", horarioEntrega.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_horario_entrega", horarioEntrega.Id);

                sqlConn.Command.CommandText = string.Format(@"UPDATE tab_horario_entrega
	                                                            SET bol_ativo = @bol_ativo
                                                            WHERE id_horario_entrega = @id_horario_entrega;");

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = horarioEntrega.IdLoja, Mensagem = "Erro ao desativar o horário de entrega", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        public DadosHorarioEntrega ListarHorariosEntrega(int idLoja)
        {
            List<HorarioEntregaEntidade> listaHorariosEntidade = new List<HorarioEntregaEntidade>();
            List<HorarioEntrega> listaHorarios = new List<HorarioEntrega>();

            TempoAntecedenciaEntrega tempoAntecedencia = new TempoAntecedenciaEntrega();
            List<TempoAntecedenciaEntregaEntidade> listaTempoAntecedenciaEntidade = new List<TempoAntecedenciaEntregaEntidade>();

            List<DiasDeFuncionamento> listaDiasFuncionamento = new List<DiasDeFuncionamento>();
            List<DiasDeFuncionamentoEntidade> listaDiasFuncionamentoEntidade = new List<DiasDeFuncionamentoEntidade>();

            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                #region horário de entrega

                sqlConn.Command.CommandText = @"SELECT
	                                                id_horario_entrega,
	                                                id_loja,
	                                                nm_horario,
	                                                bol_ativo
                                                FROM tab_horario_entrega
                                                WHERE id_loja = @id_loja
                                                AND bol_excluido = 0
                                                ORDER BY nm_horario";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listaHorariosEntidade = new ModuloClasse().PreencheClassePorDataReader<HorarioEntregaEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException("Não foram encontrados horários de entrega");

                //transforma a entidade em objeto
                foreach (var horario in listaHorariosEntidade)
                    listaHorarios.Add(horario.ToHorarioEntrega());

                //fecha o reader
                sqlConn.Reader.Close();

                #endregion

                #region tempo de antecedencia

                //habilita ou não um horário de entrega de acordo com o tempo mínimo de atecedência definido
                sqlConn.Command.CommandText = @"SELECT
	                                                id_tempo_antecedencia,
	                                                id_loja,
	                                                nr_minutos_antecedencia
                                                FROM tab_horario_entrega_tempo_anteced_pedido
                                                WHERE id_loja = @id_loja";

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listaTempoAntecedenciaEntidade = new ModuloClasse().PreencheClassePorDataReader<TempoAntecedenciaEntregaEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException("Não foi encontrado o tempo mínimo de antecedência ao pedido");

                tempoAntecedencia = listaTempoAntecedenciaEntidade.FirstOrDefault().ToTempoAntecedenciaEntrega();

                sqlConn.Reader.Close();

                #endregion

                #region dias de funcionamento

                sqlConn.Command.CommandText = @"SELECT
                                                    id_dia_funcionamento,
	                                                id_loja,
	                                                cod_dia_semana,
	                                                nm_dia_semana,
	                                                bol_ativo
                                                FROM tab_dias_funcionamento
                                                WHERE id_loja = @id_loja
                                                ORDER BY cod_dia_semana";

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    listaDiasFuncionamentoEntidade = new ModuloClasse().PreencheClassePorDataReader<DiasDeFuncionamentoEntidade>(sqlConn.Reader);
                else
                    throw new KeyNotFoundException("Não foi possível listar os dias de funcionamento");

                //transforma a entidade em objeto
                foreach (var diaSemana in listaDiasFuncionamentoEntidade)
                    listaDiasFuncionamento.Add(diaSemana.ToDiasDeFuncionamento());

                //fecha o reader
                sqlConn.Reader.Close();

                #endregion

                #region monta o objeto com a lista de horários, tempo de antecedência de entrega e dias de funcionamento

                DadosHorarioEntrega dadosHorarioEntrega = new DadosHorarioEntrega()
                {
                    HorariosEntrega = listaHorarios,
                    TempoAntecedenciaEntrega = tempoAntecedencia,
                    DiasDeFuncionamento = listaDiasFuncionamento
                };

                #endregion

                //ativa ou não um horário de entrega, de acordo com o tempo mínimo de antecedência ao pedido
                DateTime dtAntecedencia = new DateTime();
                dtAntecedencia = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                foreach (var horarioEntrega in dadosHorarioEntrega.HorariosEntrega)
                {
                    DateTime dtHorarioEntrega = new DateTime();
                    dtHorarioEntrega = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd " + horarioEntrega.Horario));

                    if ((dtHorarioEntrega - dtAntecedencia).TotalMinutes > dadosHorarioEntrega.TempoAntecedenciaEntrega.MinutosAntecedencia)
                    {
                        horarioEntrega.HorarioDisponivel = true;
                    }
                }

                //ativa o dia atual
                foreach (var diaFuncionamento in dadosHorarioEntrega.DiasDeFuncionamento)
                {
                    //inativa o dia
                    diaFuncionamento.DiaDisponivel = false;

                    //verifica se o dia é o dia atual e se está ativo. Se sim, ativa o dia de funcionamento
                    if (diaFuncionamento.CodDiaDaSemana == System.DateTime.Now.DayOfWeek.GetHashCode() && diaFuncionamento.Ativo)
                        diaFuncionamento.DiaDisponivel = true;
                }

                return dadosHorarioEntrega;
            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Não foram encontrados horários de entrega, tempo mínimo de antecedência ao pedido ou os dias de funcionamento.", Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar os horários de entrega.", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if(sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }

        
        #region tempo antecedencia

        public TempoAntecedenciaEntrega BuscarTempoAntecedencia(int id, int idLoja)
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
	                                                            nr_minutos_antecedencia
                                                            FROM tab_horario_entrega_tempo_anteced_pedido
                                                            WHERE id_tempo_antecedencia = @id_tempo_antecedencia");

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_tempo_antecedencia", id);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                listaTempoAntecedenciaEntregaEntidade = new ModuloClasse().PreencheClassePorDataReader<TempoAntecedenciaEntregaEntidade>(sqlConn.Reader);

                if (listaTempoAntecedenciaEntregaEntidade.Count == 0)
                    throw new KeyNotFoundException();

                tempoAntecedenciaEntrega = listaTempoAntecedenciaEntregaEntidade[0].ToTempoAntecedenciaEntrega();

                return tempoAntecedenciaEntrega;
            }
            catch (KeyNotFoundException keyEx)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Tempo de antecedência nao encontrado com id " + id, Descricao = keyEx.Message ?? "", StackTrace = keyEx.StackTrace ?? "" });
                throw keyEx;
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar o tempo de antecedência com id " + id, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
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

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@nr_minutos_antecedencia", tempoAntecedenciaEntrega.MinutosAntecedencia);
                sqlConn.Command.Parameters.AddWithValue("@id_loja", tempoAntecedenciaEntrega.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@id_tempo_antecedencia", tempoAntecedenciaEntrega.Id);

                sqlConn.Command.CommandText = @"UPDATE tab_horario_entrega_tempo_anteced_pedido
	                                                SET nr_minutos_antecedencia = @nr_minutos_antecedencia
                                                WHERE id_tempo_antecedencia = @id_tempo_antecedencia
                                                AND id_loja = @id_loja";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = tempoAntecedenciaEntrega.IdLoja, Mensagem = "Erro ao atualizar o tempo de antecedência", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }

        #endregion

        #region dias de funcionamento

        public void AtualizarDiaFuncionamento(DiasDeFuncionamento diaFuncionamento)
        {
            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@bol_ativo", diaFuncionamento.Ativo);
                sqlConn.Command.Parameters.AddWithValue("@id_dia_funcionamento", diaFuncionamento.Id);

                sqlConn.Command.CommandText = @"UPDATE tab_dias_funcionamento
	                                                SET bol_ativo = @bol_ativo
                                                WHERE id_dia_funcionamento = @id_dia_funcionamento";

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = diaFuncionamento.IdLoja, Mensagem = "Erro ao atualizar o dia de funcionamento", Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
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
