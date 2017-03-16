namespace ClassesMarmitex
{
    public class BeneficioDescontoProduto
    {
        public virtual int Id { get; set; }
        public virtual int IdEmpresa { get; set; }
        public virtual int IdProduto { get; set;}
        public virtual int Desconto { get; set; }
    }
}
