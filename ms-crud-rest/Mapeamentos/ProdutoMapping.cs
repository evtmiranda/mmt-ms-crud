using ClassesMarmitex;
using FluentNHibernate.Mapping;

namespace ms_crud_rest.Mapeamentos
{
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
            Map(p => p.DadosAdicionaisProdutos);
            Map(p => p.Ativo);

            //cria uma foreign key com a tab MenuCardapio
            //References(p => p.IdMenuCardapio, "Id").Not.LazyLoad();
        }
    }
}
