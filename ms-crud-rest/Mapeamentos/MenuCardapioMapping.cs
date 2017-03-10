using ClassesMarmitex;
using FluentNHibernate.Mapping;

namespace ms_crud_rest.Mapeamentos
{
    public class MenuCardapioMapping : ClassMap<MenuCardapio>
    {
        public MenuCardapioMapping()
        {
            Id(p => p.Id).GeneratedBy.Identity();
            Map(p => p.Nome);
            Map(p => p.OrdemExibicao);
            Map(p => p.Ativo);
        }
    }
}
