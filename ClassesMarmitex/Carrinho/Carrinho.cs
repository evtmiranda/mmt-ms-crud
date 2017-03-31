namespace ClassesMarmitex
{
    using System.Collections.Generic;

    public class Carrinho
    {
        public int ID { get; set; }
        public int IdEmpresa { get; set; }
        public virtual List<ProdutoPedido> ProdutosPedido { get; set; }
    }
}
