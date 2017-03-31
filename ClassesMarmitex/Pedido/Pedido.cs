namespace ClassesMarmitex
{
    using System;

    public class Pedido
    {
        public virtual int Id { get; set; }
        public virtual int IdEmpresa { get; set; }
        public UsuarioParceiro Cliente { get; set; }
        public virtual Carrinho Carrinho { get; set; }
        public virtual FormaDePagamento FormaDePagamento { get; set; }
        public virtual DateTime DataEntrega { get; set; }
        public virtual string Observacao { get; set; }
    }

    public class DetalhePedido
    {
        public string HorarioEntrega { get; set; }
        public string FormaPagamento { get; set; }
        public string Troco { get; set; }
        public string Observacao { get; set; }
    }
}
