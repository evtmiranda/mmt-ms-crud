using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class MenuCardapio
    {
        public int Id { get; set; }
        public int IdRede { get; set; }
        public string Nome { get; set; }
        public int OrdemExibicao { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_menu_cardapio")]
    public class MenuCardapioEntidade
    {
        [Column("id_menu_cardapio")]
        public int Id { get; set; }

        [Column("id_rede")]
        public int IdRede { get; set; }

        [Column("nm_cardapio")]
        public string Nome { get; set; }

        [Column("nr_ordem_exibicao")]
        public int OrdemExibicao { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }
    }
}
