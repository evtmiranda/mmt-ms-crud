using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Parceiro
    {
        public int Id { get; set; }
        public int IdLoja{ get; set; }

        [Required(ErrorMessage = "o preenchimento do nome é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o nome é 200 caracteres")]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Required]
        public Endereco Endereco { get; set; }

        public List<Brinde> Brindes { get; set; }
        public string Codigo { get; set; }
        public decimal TaxaEntrega { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("tab_parceiro")]
    public class ParceiroEntidade
    {
        [Column("id_parceiro")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nm_parceiro")]
        public string Nome { get; set; }

        [Column("nm_descricao")]
        public string Descricao { get; set; }

        [Column("id_endereco")]
        public int IdEndereco { get; set; }

        [Column("nm_codigo")]
        public string Codigo { get; set; }

        [Column("vlr_taxa_entrega")]
        public decimal TaxaEntrega { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public Parceiro ToParceiro()
        {
            Parceiro parceiro = new Parceiro
            {
                Id = Id,
                IdLoja = IdLoja,
                Nome = Nome,
                Descricao = Descricao,
                Endereco = new Endereco { Id = IdEndereco },
                Codigo = Codigo,
                TaxaEntrega = TaxaEntrega,
                Ativo = Ativo
            };

            return parceiro;
        }
    }

    /// <summary>
    /// classe utilizada para armazenar os dados para cadastro de um parceiro
    /// </summary>
    public class ParceiroCadastro
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }

        [Required(ErrorMessage = "o preenchimento do nome é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o nome é 200 caracteres")]
        public string Nome { get; set; }

        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para a descrição é 200 caracteres")]
        public string Descricao { get; set; }

        public int IdEndereco { get; set; }

        [Required(ErrorMessage = "o preenchimento do cep é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o cep é 10 caracteres")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "o preenchimento do UF é obrigatório")]
        public string UF { get; set; }

        [Required(ErrorMessage = "o preenchimento do nome da cidade é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o nome da cidade é 200 caracteres")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "o preenchimento do nome do bairro é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o nome do bairro é 200 caracteres")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "o preenchimento do nome da rua/avenida é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o nome da rua/avenida é 200 caracteres")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "o preenchimento do número do endereço é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o número do endereço é 10 caracteres")]
        public string NumeroEndereco { get; set; }

        public string ComplementoEndereco { get; set; }

        public List<Brinde> Brindes { get; set; }
        public string Codigo { get; set; }
        public decimal TaxaEntrega { get; set; }
        public bool Ativo { get; set; }
    }
}
