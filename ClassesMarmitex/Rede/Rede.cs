using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Rede
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Dominio { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_rede")]
    public class RedeEntidade
    {
        [Column("id_rede")]
        public int Id { get; set; }

        [Column("nm_nome_rede")]
        public string Nome { get; set; }

        [Column("nm_descricao_rede")]
        public string Descricao { get; set; }

        [Column("nm_dominio_rede")]
        public string Dominio { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }
    }
}
