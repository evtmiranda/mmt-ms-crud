namespace ClassesMarmitex
{
    public class ProdutoPedido
    {
        public virtual int ID { get; set; }
        public virtual int IdEmpresa { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual decimal ValorTotal { get; set; }
    }
}
