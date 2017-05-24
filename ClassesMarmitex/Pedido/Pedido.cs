namespace ClassesMarmitex
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Pedido
    {
        public int Id { get; set; }
        public UsuarioParceiro Cliente { get; set; }
        public Parceiro Parceiro { get; set; }
        public DateTime DataPedido { get; set; }
        public List<ProdutoPedido> ListaProdutos { get; set; }
        public DateTime DataEntrega { get; set; }
        public string HorarioEntrega { get; set; }
        public List<FormaDePagamento> ListaFormaPagamento { get; set; }
        public string Troco { get; set; }
        public string Observacao { get; set; }
        public PedidoStatus PedidoStatus { get; set; }
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

        [Column("dt_entrega")]
        public DateTime DataEntrega { get; set; }

        [Column("vlr_troco")]
        public decimal Troco { get; set; }

        [Column("nm_observacao")]
        public string Observacao { get; set; }

        public Pedido ToPedido()
        {
            Pedido pedido = new Pedido()
            {
                Id = Id,
                Cliente = new UsuarioParceiro { Id = IdCliente },
                DataPedido = DataPedido,
                DataEntrega = DataEntrega,
                Troco = Troco.ToString(),
                Observacao = Observacao
            };

            return pedido;
        }
    }

    public class DetalhePedido
    {
        public string HorarioEntrega { get; set; }
        public List<string> FormaPagamento { get; set; }
        public string Troco { get; set; }
        public string Observacao { get; set; }
    }

    /// <summary>
    /// classe utilizada para trafegar dados na tela de pedido
    /// </summary>
    public class DadosAtualizarStatusPedido
    {
        public int IdPedido { get; set; }
        /// <summary>
        /// 0 = na fila, 1 = entregue, 2 = cancelado
        /// </summary>
        public int IdStatusPedido { get; set; }
    }

    public class PedidoStatus
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public int IdStatus { get; set; }
        public DateTime DataStatus { get; set; }
        public int Ativo { get; set; }
    }

    [Table("tab_pedido_status")]
    public class PedidoStatusEntidade
    {
        [Column("id_status_pedido")]
        public int Id { get; set; }

        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Column("id_status")]
        public int IdStatus { get; set; }

        [Column("dt_status")]
        public DateTime DataStatus { get; set; }

        [Column("bol_ativo")]
        public int Ativo { get; set; }

        public PedidoStatus ToPedidoStatus()
        {
            PedidoStatus pedidoStatus = new PedidoStatus
            {
                Id = this.Id,
                IdPedido = this.IdPedido,
                IdStatus = this.IdStatus,
                DataStatus = this.DataStatus,
                Ativo = this.Ativo
            };

            return pedidoStatus;
        }
    }

    public enum EstadoPedido
    {
        Fila,
        EmAndamento,
        Entregue,
        Todos
    }
}
