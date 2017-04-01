namespace ClassesMarmitex
{
    public class Log
    {
        public virtual int Id { get; set; }
        public virtual int IdEmpresa { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string Mensagem { get; set; }
        public virtual string StackTrace { get; set; }
    }
}
