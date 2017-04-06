﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class MenuCardapio
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public string Nome { get; set; }
        public List<Produto> Produtos { get; set; }
        public int OrdemExibicao { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_menu_cardapio")]
    public class MenuCardapioEntidade
    {
        [Column("id_menu_cardapio")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nm_cardapio")]
        public string Nome { get; set; }

        [Column("nr_ordem_exibicao")]
        public int OrdemExibicao { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public MenuCardapio ToMenuCardapio()
        {
            return new MenuCardapio { Id = this.Id, IdLoja = this.IdLoja, Nome = this.Nome, OrdemExibicao = this.OrdemExibicao, Ativo = this.Ativo};
        }
    }
}
