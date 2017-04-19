namespace ClassesMarmitex
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Produto
    {
        public int Id { get; set; }
        public int IdMenuCardapio { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public byte[] Imagem { get; set; }
        public List<DadosProdutoAdicional> DadosAdicionaisProdutos { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_produto")]
    public class ProdutoEntidade
    {
        [Column("id_produto")]
        public int Id { get; set; }

        [Column("id_menu_cardapio")]
        public int IdMenuCardapio { get; set; }

        [Column("nm_produto")]
        public string Nome { get; set; }

        [Column("nm_descricao")]
        public string Descricao { get; set; }

        [Column("vlr_produto")]
        public decimal Valor { get; set; }

        [Column("img_imagem")]
        public byte[] Imagem { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public Produto ToProduto()
        {
            return new Produto { Id = this.Id, IdMenuCardapio = this.IdMenuCardapio, Nome = this.Nome, Descricao = this.Descricao, Valor = this.Valor, Imagem = this.Imagem, Ativo = this.Ativo };
        }
    }
}
