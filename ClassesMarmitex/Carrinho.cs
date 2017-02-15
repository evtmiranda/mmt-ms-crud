using System.Collections.Generic;

namespace ClassesMarmitex
{
    public class Carrinho
    {
        public virtual int ID { get; set; }
        public virtual Usuario Cliente { get; set; }
        public virtual List<Produto> Produtos { get; set; }
    }
}
