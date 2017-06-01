using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Brinde
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_brinde")]
    public class BrindeEntidade
    {
        [Column("id_brinde")]
        public int Id { get; set; }

        [Column("id_brinde")]
        public int IdLoja { get; set; }

        [Column("nm_brinde")]
        public string Nome { get; set; }

        [Column("nm_descricao")]
        public string Descricao { get; set; }

        [Column("url_imagem")]
        public string Imagem { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public Brinde ToBrinde()
        {
            return new Brinde
            {
                Id = this.Id,
                IdLoja = this.IdLoja,
                Nome = this.Nome,
                Descricao = this.Descricao,
                Imagem = this.Imagem,
                Ativo = this.Ativo
            };
        }
    }

    public class BrindeParceiro
    {
        public int Id { get; set; }
        public int IdParceiro { get; set; }
        public int IdLoja { get; set; }
        public string IdBrinde { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_brinde_parceiro")]
    public class BrindeParceiroEntidade
    {
        [Column("id_brinde_parceiro")]
        public int Id { get; set; }

        [Column("id_parceiro")]
        public int IdParceiro { get; set; }

        /// <summary>
        /// id_loja vem da tabela tab_brinde
        /// </summary>
        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("id_brinde")]
        public string IdBrinde { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public BrindeParceiro ToBrindeParceiro()
        {
            return new BrindeParceiro
            {
                Id = this.Id,
                IdParceiro = this.IdParceiro,
                IdLoja = this.IdLoja,
                IdBrinde = this.IdBrinde,
                Ativo = this.Ativo
            };
        }
    }
}
