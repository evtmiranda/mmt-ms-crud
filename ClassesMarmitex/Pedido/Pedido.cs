namespace ClassesMarmitex
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Pedido
    {
        public int Id { get; set; }
        public UsuarioParceiro Cliente { get; set; }
        public DateTime DataPedido { get; set; }
        public List<ProdutoPedido> ListaProdutos { get; set; }
        public string HorarioEntrega { get; set; }
        public List<FormaDePagamento> ListaFormaPagamento { get; set; }
        public string Troco { get; set; }
        public string Observacao { get; set; }
        public int IdStatusPedido { get; set; }
        public DateTime DataPedidoEntregue { get; set; }
        public bool PedidoEntregue { get; set; }
    }

    [Table("tab_pedido")]
    public class PedidoEntidade
    {
        [Column("id_pedido")]
        public int Id { get; set; }

        [Column("id_usuario_parceiro")]
        public int IdCliente { get; set; }

        [Column("dt_pedido")]
        public DateTime DataPedido { get; set; }

        [Column("hr_entrega")]
        public string HorarioEntrega { get; set; }

        [Column("vlr_troco")]
        public decimal Troco { get; set; }

        [Column("nm_observacao")]
        public string Observacao { get; set; }

        [Column("id_status_pedido")]
        public int IdStatusPedido { get; set; }

        [Column("dt_pedido_entregue")]
        public DateTime DataPedidoEntregue { get; set; }

        [Column("bol_pedido_entregue")]
        public bool PedidoEntregue { get; set; }
    }

    public class DetalhePedido
    {
        public string HorarioEntrega { get; set; }
        public List<string> FormaPagamento { get; set; }
        public string Troco { get; set; }
        public string Observacao { get; set; }
    }

    /// <summary>
    /// Classe para utilizacao da tela de histórico de pedidos do usuário
    /// </summary>
    public class PedidoCliente
    {
        public int IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public List<ProdutoCliente> Produtos { get; set; }
    }

    public class PedidoClienteEntidade
    {
        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Column("dt_pedido")]
        public DateTime DataPedido { get; set; }

        public PedidoCliente ToPedidoCliente()
        {
            return new PedidoCliente { IdPedido = this.IdPedido, DataPedido = this.DataPedido };
        }
    }
}
