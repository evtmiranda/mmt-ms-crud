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
        public int IdLoja { get; set; }
        public int NivelPermissao { get; set; }
        public string UrlLoja { get; set; }
    }

    [Table("tab_usuario_loja")]
    public class UsuarioLojaEntidade
    {
        [Column("id_usuario_loja")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

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

        public UsuarioLoja ToUsuarioLoja()
        {
            return new UsuarioLoja { Id = this.Id, IdLoja = this.IdLoja, Nome = this.Nome, Apelido = this.Apelido, Email = this.Email, Senha = this.Senha, NivelPermissao = this.NivelPermissao, Ativo = this.Ativo };
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
    }

    [Table("tab_usuario_parceiro")]
    public class UsuarioParceiroEntidade
    {
        [Column("id_usuario_parceiro")]
        public int Id { get; set; }

        [Column("id_parceiro")]
        public int IdParceiro { get; set; }

        [Column("nm_usuario")]
        public string Nome { get; set; }

        [Column("nm_apelido")]
        public string Apelido { get; set; }

        [Column("nm_email")]
        public string Email { get; set; }

        [Column("nm_celular")]
        public string NumeroCelular { get; set; }

        [Column("nm_senha")]
        public string Senha { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }

        public UsuarioParceiro ToUsuarioParceiro()
        {
            return new UsuarioParceiro { Id = this.Id, IdParceiro = this.IdParceiro, Nome = this.Nome, Apelido = this.Apelido, Email = this.Email, NumeroCelular = this.NumeroCelular, Senha = this.Senha, Ativo = this.Ativo };
        }
    }
}
