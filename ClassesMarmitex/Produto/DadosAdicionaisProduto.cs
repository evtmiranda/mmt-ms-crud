using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class DadosProdutoAdicional
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<DadosProdutoAdicionalItem> ItensAdicionais { get; set; }
        public int QtdMin { get; set; }
        public int QtdMax { get; set; }
        public int OrdemExibicao { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_produto_adicional")]
    public class DadosProdutoAdicionalEntidade
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

        public DadosProdutoAdicional ToProdutoAdicional()
        {
            throw new NotImplementedException();
        }
    }

    public class DadosProdutoAdicionalItem
    {
        public int Id { get; set; }
        public int IdProdutoAdicional { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
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
            throw new NotImplementedException();
        }
    }

    public class DadosProdutoAdicionalProduto
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public int IdProdutoAdicional { get; set; }
    }

    [Table("tab_produto_adicional_produto")]
    public class DadosProdutoAdicionalProdutoEntidade
    {
        [Column("id_produto_adicional_produto")]
        public int Id { get; set; }

        [Column("id_produto")]
        public int IdProduto { get; set; }

        [Column("id_produto_adicional")]
        public int IdProdutoAdicional { get; set; }

        public DadosProdutoAdicionalProduto ToProdutoAdicionalProduto()
        {
            throw new NotImplementedException();
        }
    }

}
