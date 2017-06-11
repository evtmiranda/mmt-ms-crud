using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class FormaDePagamento
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }

        [Required(ErrorMessage = "o preenchimento do nome é obrigatório")]
        public string Nome { get; set; }

        public virtual bool Ativo { get; set; }
    }

    [Table("tab_forma_pagamento")]
    public class FormaDePagamentoEntidade
    {
        [Column("id_forma_pagamento")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nm_forma_pagamento")]
        public string Nome { get; set; }

        [Column("bol_ativo")]
        public virtual bool Ativo { get; set; }

        public FormaDePagamento ToFormaPagamento()
        {
            return new FormaDePagamento { Id = this.Id, IdLoja = this.IdLoja, Nome = this.Nome, Ativo = this.Ativo };
        }
    }
}
