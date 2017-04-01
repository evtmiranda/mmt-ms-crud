using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Endereco
    {
        public int Id { get; set; }
        public string Cep { get; set; }
        public string UF { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
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
    }
}
