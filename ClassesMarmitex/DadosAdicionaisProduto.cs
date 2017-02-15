namespace ClassesMarmitex
{
    public class DadosAdicionaisProduto
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual decimal? Valor { get; set; }
        public virtual int OrdemExibicao { get; set; }
        public virtual bool Ativo { get; set; }
    }
}
