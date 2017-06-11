using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{

    public class DadosHorarioEntrega
    {
        public List<HorarioEntrega> HorariosEntrega { get; set; }
        public TempoAntecedenciaEntrega TempoAntecedenciaEntrega { get; set; }
        public TempoAntecedenciaCancelamentoEntrega TempoAntecedenciaCancelamentoEntrega { get; set; }
        public List<DiasDeFuncionamento> DiasDeFuncionamento { get; set; }
    }

    public class HorarioEntrega
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }

        [Required(ErrorMessage = "o preenchimento do tempo é obrigatório")]
        public string Horario { get; set; }
        public bool HorarioDisponivel { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_horario_entrega")]
    public class HorarioEntregaEntidade
    {
        [Column("id_horario_entrega")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nm_horario")]
        public string Horario { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public HorarioEntrega ToHorarioEntrega()
        {
            return new HorarioEntrega { Id = this.Id, IdLoja = this.IdLoja, Horario = this.Horario, Ativo = this.Ativo };
        }
    }

    public class TempoAntecedenciaEntrega
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }

        [Required(ErrorMessage = "o preenchimento do tempo é obrigatório")]
        public int MinutosAntecedencia { get; set; }
        //public bool Ativo { get; set; }
    }

    [Table("tab_horario_entrega_tempo_anteced_pedido")]
    public class TempoAntecedenciaEntregaEntidade
    {
        [Column("id_tempo_antecedencia")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nr_minutos_antecedencia")]
        public int MinutosAntecedencia { get; set; }

        //[Column("bol_ativo")]
        //public bool Ativo { get; set; }

        public TempoAntecedenciaEntrega ToTempoAntecedenciaEntrega()
        {
            return new TempoAntecedenciaEntrega { Id = this.Id, IdLoja = this.IdLoja, MinutosAntecedencia = this.MinutosAntecedencia/*, Ativo = this.Ativo */};
        }
    }

    public class TempoAntecedenciaCancelamentoEntrega
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }

        [Required(ErrorMessage = "o preenchimento do tempo é obrigatório")]
        public int MinutosAntecedencia { get; set; }
    }

    [Table("tab_horario_entrega_tempo_anteced_cancel_pedido")]
    public class TempoAntecedenciaCancelamentoEntregaEntidade
    {
        [Column("id_tempo_antecedencia")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nr_minutos_antecedencia")]
        public int MinutosAntecedencia { get; set; }

        public TempoAntecedenciaCancelamentoEntrega ToTempoAntecedenciaCancelamentoEntrega()
        {
            return new TempoAntecedenciaCancelamentoEntrega { Id = this.Id, IdLoja = this.IdLoja, MinutosAntecedencia = this.MinutosAntecedencia};
        }
    }

    public class DiasDeFuncionamento
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int CodDiaDaSemana { get; set; }
        public string DiaDaSemana { get; set; }
        public bool DiaDisponivel { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_dias_funcionamento")]
    public class DiasDeFuncionamentoEntidade
    {
        [Column("id_dia_funcionamento")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        /// <summary>
        /// Domingo: 0
        /// Segunda-feira: 1
        /// ...
        /// Sábado: 6
        /// </summary>
        [Column("cod_dia_semana")]
        public int CodDiaDaSemana { get; set; }

        [Column("nm_dia_semana")]
        public string DiaDaSemana { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public DiasDeFuncionamento ToDiasDeFuncionamento()
        {
            return new DiasDeFuncionamento { Id = this.Id, IdLoja = this.IdLoja, CodDiaDaSemana = this.CodDiaDaSemana, DiaDaSemana = this.DiaDaSemana, Ativo = this.Ativo };
        }
    }
}
