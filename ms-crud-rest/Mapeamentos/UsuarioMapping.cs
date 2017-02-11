using FluentNHibernate.Mapping;
using ClassesMarmitex;

namespace ms_crud_rest.Mapeamentos
{
    public class UsuarioMapping : ClassMap<Usuario>
    {
        public UsuarioMapping()
        {
            Id(p => p.Id).GeneratedBy.Identity();
            Map(p => p.Nome);
            Map(p => p.Email);
            Map(p => p.Senha);
        }
    }
}
