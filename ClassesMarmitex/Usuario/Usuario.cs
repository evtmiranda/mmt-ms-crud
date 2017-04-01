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
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
    }
    public class UsuarioLoja : Usuario
    {
        public int IdLoja { get; set; }
        public int NivelPermissao { get; set; }
    }

    [Table("tab_usuario_loja")]
    public class UsuarioLojaEntidade
    {
        [Column("id_usuario_loja")]
        public int Id { get; set; }

        [Column("id_loja")]
        public int IdLoja { get; set; }

        [Column("nm_nome")]
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
            return new UsuarioLoja { Id = this.Id, IdLoja = this.IdLoja, Nome = this.Nome, Apelido = this.Apelido, Email = this.Email, Senha = this.Senha, NivelPermissao = this.NivelPermissao, Ativo = this.Ativo};
        }
    }

    public class UsuarioParceiro : Usuario
    {
        public string Id_parceiro { get; set; }
    }

    [Table("tab_usuario_parceiro")]
    public class UsuarioParceiroEntidade
    {
        [Column("id_usuario_parceiro")]
        public int Id { get; set; }

        [Column("id_parceiro")]
        public string Id_parceiro { get; set; }

        [Column("nm_nome")]
        public string Nome { get; set; }

        [Column("nm_apelido")]
        public string Apelido { get; set; }

        [Column("nm_email")]
        public string Email { get; set; }

        [Column("nm_senha")]
        public string Senha { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }
    }
}
