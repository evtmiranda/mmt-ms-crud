using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class DadosAdicionaisProduto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<DadosAdicionaisProdutoItem> ItensAdicionais { get; set; }
        public int QtdMin { get; set; }
        public int QtdMax { get; set; }
        public int OrdemExibicao { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_produto_adicional")]
    public class DadosAdicionaisProdutoEntidade
    {
        [Column("id_produto_adicional")]
        public int Id { get; set; }

        [Column("nm_adicional")]
        public string Nome { get; set; }

        [Column("nm_descricao")]
        public string Descricao { get; set; }

        [Column("nr_qtd_min")]
        public int QtdMin { get; set; }

        [Column("nr_qtd_max")]
        public int QtdMax { get; set; }

        [Column("nr_ordem_exibicao")]
        public int OrdemExibicao { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }
    }

    public class DadosAdicionaisProdutoItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_produto_adicional_item")]
    public class DadosAdicionaisProdutoItemEntidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public bool Ativo { get; set; }
    }

}
