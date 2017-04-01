using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class DadosAdicionaisProduto
    {
        public virtual int Id { get; set; }
        public virtual int IdProduto { get; set; }
        public virtual string Nome { get; set; }
        public virtual decimal? Valor { get; set; }
        public virtual int OrdemExibicao { get; set; }
        public virtual bool Ativo { get; set; }
    }

    [Table("tab_produto_adicional")]
    public class DadosAdicionaisProdutoEntidade
    {
        [Column("id_produto_adicional")]
        public virtual int Id { get; set; }

        [Column("id_produto")]
        public virtual int IdProduto { get; set; }

        [Column("nm_adicional")]
        public virtual string Nome { get; set; }

        [Column("vlr_produto_adicional")]
        public virtual decimal? Valor { get; set; }

        [Column("nr_ordem_exibicao")]
        public virtual int OrdemExibicao { get; set; }

        [Column("bol_ativo")]
        public virtual bool Ativo { get; set; }
    }
}
