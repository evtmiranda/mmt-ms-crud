using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class Dashboard
    {
        [Column("Pedidos")]
        public int Pedidos { get; set; }

        [Column("Vendas")]
        public decimal Vendas { get; set; }

        [Column("Visitas")]
        public int Visitas { get; set; }

        [Column("Parceiros")]
        public int Parceiros { get; set; }

        [Column("PedidosAtrasados")]
        public int PedidosAtrasados { get; set; }

        [Column("PedidosEntregues")]
        public int PedidosEntregues { get; set; }
    }
}
