namespace ClassesMarmitex
{
    using System.Collections.Generic;

    public class Carrinho
    {
        public virtual int ID { get; set; }
        public virtual int IdEmpresa { get; set; }
        public virtual List<ProdutoPedido> ProdutosPedido { get; set; }
    }
}
