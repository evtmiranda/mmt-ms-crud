namespace ClassesMarmitex
{
    using System.Collections.Generic;

    public class Empresa
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Cep { get; set; }
        public virtual string UF { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual string NumeroEndereco { get; set; }
        public virtual string ComplementoEndereco { get; set; }
    }
}
