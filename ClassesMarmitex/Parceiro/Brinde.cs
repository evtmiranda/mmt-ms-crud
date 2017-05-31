using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Brinde
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int IdParceiro { get; set; }
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

        /// <summary>
        /// id loja vem da tab_parceiro
        /// </summary>
        [Column("id_brinde")]
        public int IdLoja { get; set; }

        [Column("id_parceiro")]
        public int IdParceiro { get; set; }

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
                IdParceiro = this.IdParceiro,
                Nome = this.Nome,
                Descricao = this.Descricao,
                Imagem = this.Imagem,
                Ativo = this.Ativo
            };
        }
    }
}
