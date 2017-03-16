namespace ms_crud_rest.Mapeamentos
{
    using FluentNHibernate.Mapping;
    using ClassesMarmitex;

    public class EmpresaMapping : ClassMap<Empresa>
    {
        public EmpresaMapping()
        {
            Id(p => p.Id).GeneratedBy.Identity();
            Map(p => p.Nome);
            Map(p => p.Cep);
            Map(p => p.UF);
            Map(p => p.Cidade);
            Map(p => p.Bairro);
            Map(p => p.Logradouro);
            Map(p => p.NumeroEndereco);
            Map(p => p.ComplementoEndereco);
        }
    }
}
