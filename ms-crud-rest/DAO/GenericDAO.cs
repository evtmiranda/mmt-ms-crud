using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public abstract class GenericDAO <T>
    {
        public virtual void Adicionar(T t) { }

        public virtual IList<T> Listar() { return null; }

        public virtual T BuscarPorId(int id) { return default(T); }

        public virtual T BuscarPorEmail(string email) { return default(T); }

        public virtual void Atualizar(T t) { }

        public virtual void Excluir(T t) { }

        public virtual void ExcluirPorId(int id) { }
    }
}
