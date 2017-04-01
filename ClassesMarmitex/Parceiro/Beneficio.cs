using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Beneficio
    {
        public int Id { get; set; }
        public int IdParceiro { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<Produto> Produtos { get; set; }
        public int Desconto { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_beneficio")]
    public class BeneficioEntidade
    {
        [Column("id_beneficio")]
        public int Id { get; set; }

        [Column("id_parceiro")]
        public int IdParceiro { get; set; }

        [Column("nm_beneficio")]
        public string Nome { get; set; }

        [Column("nm_descricao")]
        public string Descricao { get; set; }

        [Column("perc_desconto")]
        public int Desconto { get; set; }

        [Column("dt_inicial")]
        public DateTime DataInicial { get; set; }

        [Column("dt_final")]
        public DateTime DataFinal { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }
    }

    [Table("tab_beneficio_produto")]
    public class BeneficioProdutoEntidade
    {
        [Column("id_beneficio_produto")]
        public int Id { get; set; }

        [Column("id_beneficio")]
        public int IdBeneficio { get; set; }

        [Column("id_produto")]
        public int IdProduto { get; set; }
    }
}
