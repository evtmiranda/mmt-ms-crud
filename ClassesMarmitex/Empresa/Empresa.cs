using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClassesMarmitex
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Endereco Endereco { get; set; }
        public List<Parceiro> Parceiros { get; set; }
        public bool Ativo { get; set; }
    }

    [Table("Empresa")]
    public class EmpresaEntidade
    {
        [Column("id_empresa")]
        public int Id { get; set; }

        [Column("nm_nome_empresa")]
        public string Nome { get; set; }

        [Column("id_endereco")]
        public int IdEndereco { get; set; }

        [Column("bol_ativo")]
        public bool Ativo { get; set; }
    }
}
