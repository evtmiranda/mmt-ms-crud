using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Loja
    {
        public int Id { get; set; }
        public Rede Rede { get; set; }
        public string Nome { get; set; }
        public Endereco Endereco { get; set; }
        public List<Parceiro> Parceiros { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_loja")]
    public class LojaEntidade
    {
        [Column("id_loja")]
        public int Id { get; set; }

        [Column("id_rede")]
        public int IdRede { get; set; }

        [Column("nm_loja")]
        public string Nome { get; set; }

        [Column("id_endereco")]
        public int IdEndereco { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public Loja ToLoja() { return new Loja { Id = this.Id, Nome = this.Nome, Ativo = this.Ativo}; }
    }
}
