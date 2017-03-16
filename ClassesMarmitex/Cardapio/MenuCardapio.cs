namespace ClassesMarmitex
{
    public class MenuCardapio
    {
        public virtual int Id { get; set; }
        public virtual int IdEmpresa { get; set; }
        public virtual string Nome { get; set; }
        public virtual int OrdemExibicao { get; set; }
        public virtual bool Ativo { get; set; }
    }
}
