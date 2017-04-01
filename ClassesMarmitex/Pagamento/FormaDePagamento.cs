namespace ClassesMarmitex
{
    public class FormaDePagamento
    {
        public int Id { get; set; }
        public string  Nome { get; set; }
        public virtual bool Ativo { get; set; }
    }

    public class FormaDePagamentoEntidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual bool Ativo { get; set; }
    }
}
