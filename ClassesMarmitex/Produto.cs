using System.Collections.Generic;

namespace ClassesMarmitex
{
    public class Produto
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual byte[] Imagem { get; set; }
        public virtual List<DadosAdicionaisProduto> DadosAdicionaisProdutos { get; set; }
        public virtual int IdMenuCardapio { get; set; }
        public virtual bool Ativo { get; set; }
    }
}
