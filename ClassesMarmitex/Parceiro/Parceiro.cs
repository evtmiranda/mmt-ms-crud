using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Parceiro
    {
        public int Id { get; set; }
        public int IdLoja{ get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Endereco Endereco { get; set; }
        public List<Beneficio> Beneficios { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_parceiro")]
    public class ParceiroEntidade
    {
        [Column("id_parceiro")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nm_nome")]
        public string Nome { get; set; }

        [Column("nm_descricao")]
        public string Descricao { get; set; }

        [Column("id_endereco")]
        public int IdEndereco { get; set; }

        [Column("nm_codigo")]
        public string Codigo { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }
    }
}
