namespace ClassesMarmitex
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Pedido
    {
        public int Id { get; set; }
        public UsuarioParceiro Cliente { get; set; }
        public List<ProdutoPedido> ListaProdutos { get; set; }
        public string HorarioEntrega { get; set; }
        public List<FormaDePagamento> ListaFormaPagamento { get; set; }
        public decimal Troco { get; set; }
        public string Observacao { get; set; }
    }

    [Table("tab_pedido")]
    public class PedidoEntidade
    {
        [Column("id_pedido")]
        public int Id { get; set; }

        [Column("id_usuario_parceiro")]
        public int IdCliente { get; set; }

        [Column("hr_entrega")]
        public string HorarioEntrega { get; set; }

        [Column("vlr_troco")]
        public decimal Troco { get; set; }

        [Column("nm_observacao")]
        public string Observacao { get; set; }
    }
}
