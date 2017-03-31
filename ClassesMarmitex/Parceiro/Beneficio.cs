using System;

namespace ClassesMarmitex
{
    public class Beneficio
    {
        public int Id { get; set; }
        public int IdParceiro { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Produto { get; set; }
        public int Desconto { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public bool Ativo { get; set; }
    }

    public class BeneficioEntidade
    {
        public int Id { get; set; }
        public int IdParceiro { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public bool Ativo { get; set; }
    }
}
