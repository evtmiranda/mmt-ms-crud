using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{

    public enum TipoUsuario
    {
        Loja,
        Parceiro
    }

    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "o preenchimento do nome é obrigatório")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o nome é 200 caracteres")]
        public string Nome { get; set; }

        public int IdLoja { get; set; }

        public string NomeLoja { get; set; }

        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o apelido é 200 caracteres")]
        public string Apelido { get; set; }

        [Required(ErrorMessage = "o preenchimento do e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "por favor, verifique se o e-mail está correto")]
        [StringLength(200, ErrorMessage = "o tamanho máximo aceito para o e-mail é 200 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "o preenchimento da senha é obrigatório")]
        [StringLength(100, ErrorMessage = "o tamanho máximo aceito para a senha é 200 caracteres")]
        [MinLength(6, ErrorMessage = "a senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; }

        public bool Ativo { get; set; }
    }
    public class UsuarioLoja : Usuario
    {
        [Required(ErrorMessage = "o preenchimento da permissão é obrigatório")]
        public int NivelPermissao { get; set; }
        public string UrlLoja { get; set; }
        public string Imagem { get; set; }
    }

    [Table("tab_usuario_loja")]
    public class UsuarioLojaEntidade
    {
        [Column("id_usuario_loja")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nm_loja")]
        public string NomeLoja { get; set; }

        [Column("nm_usuario")]
        public string Nome { get; set; }

        [Column("nm_apelido")]
        public string Apelido { get; set; }

        [Column("nm_email")]
        public string Email { get; set; }

        [Column("nm_senha")]
        public string Senha { get; set; }

        [Column("nr_nivel_permissao")]
        public int NivelPermissao { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        /// <summary>
        /// a imagem vem da tab_loja
        /// </summary>
        [Column("url_imagem")]
        public string Imagem { get; set; }

        public UsuarioLoja ToUsuarioLoja()
        {
            return new UsuarioLoja { Id = this.Id, IdLoja = this.IdLoja, NomeLoja = this.NomeLoja, Nome = this.Nome, Apelido = this.Apelido, Email = this.Email, Senha = this.Senha, NivelPermissao = this.NivelPermissao, Ativo = this.Ativo, Imagem = this.Imagem };
        }
    }

    public class UsuarioParceiro : Usuario
    {
        public int IdParceiro { get; set; }

        [Required(ErrorMessage = "o preenchimento do código da empresa é obrigatório")]
        [StringLength(5, ErrorMessage = "por favor, verifique o código de empresa digitado")]
        public string CodigoParceiro { get; set; }

        [Required(ErrorMessage = "o preenchimento do telefone celular é obrigatório")]
        [StringLength(15, ErrorMessage = "por favor, verifique o número de celular digitado")]
        public string NumeroCelular { get; set; }

        [Required(ErrorMessage = "o preenchimento da dica de localização é obrigatório")]
        public string DicaDeLocalizacao { get; set; }

        public string Endereco { get; set; }

        public decimal TaxaEntrega { get; set; }
    }

    [Table("tab_usuario_parceiro")]
    public class UsuarioParceiroEntidade
    {
        [Column("id_usuario_parceiro")]
        public int Id { get; set; }

        [Column("id_parceiro")]
        public int IdParceiro { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nm_loja")]
        public string NomeLoja { get; set; }

        [Column("nm_usuario")]
        public string Nome { get; set; }

        [Column("nm_apelido")]
        public string Apelido { get; set; }

        [Column("nm_email")]
        public string Email { get; set; }

        [Column("nm_celular")]
        public string NumeroCelular { get; set; }

        [Column("nm_dica_localizacao")]
        public string DicaDeLocalizacao { get; set; }

        [Column("nm_senha")]
        public string Senha { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        /// <summary>
        /// o endereco vem da tab_parceiro e tab_endereco
        /// </summary>
        [Column("endereco")]
        public string Endereco { get; set; }

        /// <summary>
        /// O valor da taxa vem da tab_parceiro
        /// </summary>
        [Column("vlr_taxa_entrega")]
        public decimal TaxaEntrega { get; set; }

        public UsuarioParceiro ToUsuarioParceiro()
        {
            return new UsuarioParceiro { Id = this.Id, IdLoja = this.IdLoja, NomeLoja = this.NomeLoja, IdParceiro = this.IdParceiro,
                Nome = this.Nome, Apelido = this.Apelido, Email = this.Email, NumeroCelular = this.NumeroCelular,
                DicaDeLocalizacao = this.DicaDeLocalizacao, Senha = this.Senha, Ativo = this.Ativo, Endereco = this.Endereco,
                TaxaEntrega = this.TaxaEntrega };
        }
    }
}
