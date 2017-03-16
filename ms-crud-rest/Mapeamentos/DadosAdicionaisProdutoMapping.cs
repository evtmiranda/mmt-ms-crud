namespace ms_crud_rest.Mapeamentos
{
    using ClassesMarmitex;
    using FluentNHibernate.Mapping;

    public class DadosAdicionaisProdutoMapping : ClassMap<DadosAdicionaisProduto>
    {
        public DadosAdicionaisProdutoMapping()
        {
            Id(p => p.ID).GeneratedBy.Identity();
            Map(p => p.IDProduto);
            Map(p => p.Nome);
            Map(p => p.Valor);
            Map(p => p.OrdemExibicao);
            Map(p => p.Ativo);
        }
    }
}
