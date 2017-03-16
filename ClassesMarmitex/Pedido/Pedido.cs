namespace ClassesMarmitex
{
    using System;

    public class Pedido
    {
        public virtual int Id { get; set; }
        public virtual int IdEmpresa { get; set; }
        public virtual Usuario Cliente { get; set; }
        public virtual Carrinho Carrinho { get; set; }
        public virtual FormaDePagamento FormaDePagamento { get; set; }
        public virtual DateTime DataEntrega { get; set; }
        public virtual string Observacao { get; set; }
    }
}
