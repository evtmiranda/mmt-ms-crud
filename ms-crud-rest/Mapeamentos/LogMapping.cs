namespace ms_crud_rest.Mapeamentos
{
    using ClassesMarmitex;
    using FluentNHibernate.Mapping;

    public class LogMapping : ClassMap<Log>
    {
        public LogMapping()
        {
            Id(p => p.Id).GeneratedBy.Identity();
            Map(p => p.Descricao);
            Map(p => p.Mensagem);
            Map(p => p.StackTrace).Length(5000);
        }
    }
}
