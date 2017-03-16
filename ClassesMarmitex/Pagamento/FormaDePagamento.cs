namespace ClassesMarmitex
{
    public class FormaDePagamento
    {
        public virtual int ID { get; set; }
        public virtual int IdEmpresa { get; set; }
        public virtual string  Nome { get; set; }
        public virtual decimal Troco { get; set; }
    }
}
