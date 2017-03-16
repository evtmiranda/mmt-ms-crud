namespace ms_crud_rest.Mapeamentos
{
    using ClassesMarmitex;
    using FluentNHibernate.Mapping;

    public class ProdutoMapping : ClassMap<Produto>
    {
        public ProdutoMapping()
        {
            Id(p => p.Id).GeneratedBy.Identity();
            Map(p => p.Nome);
            Map(p => p.Descricao);
            Map(p => p.Valor);
            Map(p => p.Imagem);
            Map(p => p.IdMenuCardapio);
            Map(p => p.Ativo);
        }
    }
}
