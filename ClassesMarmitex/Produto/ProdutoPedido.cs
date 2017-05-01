using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesMarmitex
{
    public class ProdutoPedido
    {
        public int Id { get; set; }
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
    }

    [Table("tab_produto_pedido")]
    public class ProdutoPedidoEntidade
    {
        [Column("id_produto_pedido")]
        public int Id { get; set; }

        [Column("id_produto")]
        public int IdProduto { get; set; }

        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Column("nr_qtd_produto")]
        public int Quantidade { get; set; }

        [Column("vlr_total_produto")]
        public decimal ValorTotal { get; set; }

        public ProdutoPedido ToProdutoPedido()
        {
            ProdutoPedido produtoPedido = new ProdutoPedido
            {
                Id = Id,
                Produto = new Produto { Id = IdProduto },
                Quantidade = Quantidade,
                ValorTotal = ValorTotal
            };

            return produtoPedido;
        }

    }
}
