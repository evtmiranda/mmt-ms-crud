using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Brinde
    {
        public int Id { get; set; }
        public int IdParceiro { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_beneficio")]
    public class BrindeEntidade
    {
        [Column("id_brinde")]
        public int Id { get; set; }

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
    }
}
