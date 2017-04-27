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
        public string Imagem { get; set; }
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

        [Column("url_imagem")]
        public string Imagem { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public Produto ToProduto()
        {
            return new Produto { Id = this.Id, IdMenuCardapio = this.IdMenuCardapio, Nome = this.Nome, Descricao = this.Descricao, Valor = this.Valor, Imagem = this.Imagem, Ativo = this.Ativo };
        }
    }

    //classe apenas para armazenar dados para exibição
    public class ProdutoCliente
    {
        public int IdPedido { get; set; }
        public string NomeProduto { get; set; }
        public int QtdProduto { get; set; }
    }

    public class ProdutoClienteEntidade
    {
        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Column("nm_produto")]
        public string NomeProduto { get; set; }

        [Column("nr_qtd_produto")]
        public int QtdProduto { get; set; }

        public ProdutoCliente ToProdutoCliente()
        {
            return new ProdutoCliente { NomeProduto = this.NomeProduto, QtdProduto = this.QtdProduto, IdPedido = this.IdPedido };
        }
    }
}
