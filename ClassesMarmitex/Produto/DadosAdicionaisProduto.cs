using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class DadosProdutoAdicional
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }

        [Required(ErrorMessage = "o preenchimento do nome é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o nome é 200 caracteres")]
        public string Nome { get; set; }

        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para a descrição é 200 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "defina a quantidade mínima do produto adicional")]
        public int QtdMin { get; set; }

        [Required(ErrorMessage = "defina a quantidade máxima do produto adicional")]
        public int QtdMax { get; set; }

        [Required(ErrorMessage = "o preenchimento da ordem de exibição é obrigatório")]
        public int OrdemExibicao { get; set; }

        public List<DadosProdutoAdicionalItem> ItensAdicionais { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_produto_adicional")]
    public class DadosProdutoAdicionalEntidade
    {
        [Column("id_produto_adicional")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nm_adicional")]
        public string Nome { get; set; }

        [Column("nm_descricao")]
        public string Descricao { get; set; }

        // os campos nr_qtd_min, nr_qtd_max e nr_ordem_exibicao são lidos
        // da tabela tab_produto_adicional_produto
        [Column("nr_qtd_min")]
        public int QtdMin { get; set; }

        [Column("nr_qtd_max")]
        public int QtdMax { get; set; }

        [Column("nr_ordem_exibicao")]
        public int OrdemExibicao { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public DadosProdutoAdicional ToProdutoAdicional()
        {
            return new DadosProdutoAdicional { Id = this.Id, IdLoja = this.IdLoja, Nome = this.Nome, Descricao = this.Descricao,
                QtdMin = this.QtdMin, QtdMax = this.QtdMax, OrdemExibicao = this.OrdemExibicao, Ativo = this.Ativo };
        }
    }

    public class DadosProdutoAdicionalItem
    {
        public int Id { get; set; }
        public int IdProdutoAdicional { get; set; }
        public int IdLoja { get; set; }

        [Required(ErrorMessage = "o preenchimento do nome é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o nome é 200 caracteres")]
        public string Nome { get; set; }

        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para a descrição é 200 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "o preenchimento do valor é obrigatório")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "o preenchimento da quantidade é obrigatório")]
        public int Qtd { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_produto_adicional_item")]
    public class DadosProdutoAdicionalItemEntidade
    {
        [Column("id_produto_adicional_item")]
        public int Id { get; set; }

        [Column("id_produto_adicional")]
        public int IdProdutoAdicional { get; set; }

        [Column("nm_adicional_item")]
        public string Nome { get; set; }

        [Column("nm_descricao_item")]
        public string Descricao { get; set; }

        [Column("vlr_adicional_item")]
        public decimal Valor { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public DadosProdutoAdicionalItem ToProdutoAdicionalItem()
        {
            return new DadosProdutoAdicionalItem { Id = this.Id, Nome = this.Nome, Descricao = this.Descricao, Valor = this.Valor, IdProdutoAdicional = this.IdProdutoAdicional, Ativo = this.Ativo };
        }
    }

    public class DadosProdutoAdicionalProduto
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public int IdLoja { get; set; }
        public string NomeProduto { get; set; }
        public int IdProdutoAdicional { get; set; }
        public string NomeProdutoAdicional { get; set; }
        public string DescricaoProdutoAdicional { get; set; }

        [Required(ErrorMessage = "o preenchimento da quantidade mínima é obrigatório")]
        public int QtdMin { get; set; }

        [Required(ErrorMessage = "o preenchimento da quantidade máxima é obrigatório")]
        public int QtdMax { get; set; }

        [Required(ErrorMessage = "o preenchimento da ordem de exibição é obrigatório")]
        public int OrdemExibicao { get; set; }

        public bool Ativo { get; set; }
    }

    [Table("tab_produto_adicional_produto")]
    public class DadosProdutoAdicionalProdutoEntidade
    {
        [Column("id_produto_adicional_produto")]
        public int Id { get; set; }

        [Column("id_produto")]
        public int IdProduto { get; set; }

        //o campo NomeProduto é lido da tabela tab_produto
        [Column("nm_produto")]
        public string NomeProduto { get; set; }

        [Column("id_produto_adicional")]
        public int IdProdutoAdicional { get; set; }

        //os campos NomeProdutoAdicional e DescricaoProdutoAdicional
        //são lidos da tabela tab_produto_adicional

        [Column("nm_adicional")]
        public string NomeProdutoAdicional { get; set; }

        [Column("nm_descricao")]
        public string DescricaoProdutoAdicional { get; set; }

        [Column("nr_qtd_min")]
        public int QtdMin { get; set; }

        [Column("nr_qtd_max")]
        public int QtdMax { get; set; }

        [Column("nr_ordem_exibicao")]
        public int OrdemExibicao { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public DadosProdutoAdicionalProduto ToProdutoAdicionalProduto()
        {
            return new DadosProdutoAdicionalProduto { Id = this.Id, IdProduto = this.IdProduto, NomeProduto = this.NomeProduto,
                IdProdutoAdicional = this.IdProdutoAdicional, NomeProdutoAdicional = this.NomeProdutoAdicional,
                DescricaoProdutoAdicional = this.DescricaoProdutoAdicional, QtdMin = this.QtdMin, QtdMax = this.QtdMax,
                OrdemExibicao = this.OrdemExibicao, Ativo = this.Ativo};
        }
    }

    public class DadosProdutoAdicionalPedido
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public int IdProdutoPedido { get; set; }
        public int IdProdutoAdicionalItem{ get; set; }
        public int QtdItemAdicional { get; set; }
    }

    [Table("tab_produto_adicional_pedido")]
    public class DadosProdutoAdicionalPedidoEntidade
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Column("id_produto_pedido")]
        public int IdProdutoPedido { get; set; }

        [Column("id_produto_adicional_item")]
        public int IdProdutoAdicionalItem { get; set; }

        [Column("qtd_item_adicional")]
        public int QtdItemAdicional { get; set; }

        public DadosProdutoAdicionalPedido ToProdutoAdicionalPedido()
        {
            DadosProdutoAdicionalPedido dadosProdutoAdicionalPedido = new DadosProdutoAdicionalPedido()
            {
                Id = Id,
                IdPedido = IdPedido,
                IdProdutoPedido = IdProdutoPedido,
                IdProdutoAdicionalItem = IdProdutoAdicionalItem,
                QtdItemAdicional = QtdItemAdicional
            };

            return dadosProdutoAdicionalPedido;
        }
    }

    public class DadosAtualizarProdutoAdicional
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public List<DadosAtualizarProdutoAdicionalItem> ListaProdutosAdicionais { get; set; }
    }

    public class DadosAtualizarProdutoAdicionalItem
    {
        public int Id { get; set; }
        public int Qtd { get; set; }
    }
}
