using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Endereco
    {
        public int Id { get; set; }
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
    }

    [Table("tab_endereco")]
    public class EnderecoEntidade
    {
        [Column("id_endereco")]
        public int Id { get; set; }

        [Column("nm_cep")]
        public string Cep { get; set; }

        [Column("nm_uf")]
        public string UF { get; set; }

        [Column("nm_cidade")]
        public string Cidade { get; set; }

        [Column("nm_bairro")]
        public string Bairro { get; set; }

        [Column("nm_logradouro")]
        public string Logradouro { get; set; }

        [Column("nm_numero_endereco")]
        public string NumeroEndereco { get; set; }

        [Column("nm_complemento_endereco")]
        public string ComplementoEndereco { get; set; }

        public Endereco ToEndereco()
        {
            Endereco endereco = new Endereco
            {
                Id = Id,
                Cep = Cep,
                UF = UF,
                Cidade = Cidade,
                Bairro = Bairro,
                Logradouro = Logradouro,
                NumeroEndereco = NumeroEndereco,
                ComplementoEndereco = ComplementoEndereco
            };

            return endereco;
        }
    }
}
