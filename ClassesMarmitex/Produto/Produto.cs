namespace ClassesMarmitex
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Produto
    {
        public int Id { get; set; }
        public int IdMenuCardapio { get; set; }
        public int IdLoja { get; set; }

        [Required(ErrorMessage = "o preenchimento do nome é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o nome é 200 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "o preenchimento da descrição é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para a descrição é 200 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "o preenchimento do valor é obrigatório")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "é necessário incluir uma imagem para o produto")]
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

    /// <summary>
    /// Classe para utilizacao da tela de histórico de pedidos do usuário
    /// </summary>
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

    ///// <summary>
    ///// Classe para armazenar um produto e seus produtos adicionais. 
    ///// Utilizada na tela de produtos do painel admin
    ///// </summary>
    //public class ProdutoDetalhes
    //{
    //    public Produto Produto { get; set; }
    //    public List<DadosProdutoAdicionalProduto> ProdutosAdicionais { get; set; }
    //}
}
