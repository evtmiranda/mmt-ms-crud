using ClassesMarmitex;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class GenericDAO <T>
    {
        //recebe uma conexão e atribui à sessão da classe
        //recebe também um logDAO
        protected SqlServer sqlConn;
        protected LogDAO logDAO;

        public GenericDAO(SqlServer sqlConn, LogDAO logDAO)
        {
            this.sqlConn = sqlConn;
            this.logDAO = logDAO;
        }

        public virtual void Adicionar(T t) { }

        //public virtual int AdicionarPedido(T t) { return 0; }

        public virtual List<T> Listar() { return null; }

        public virtual List<T> Listar(int id) { return null; }

        public virtual T BuscarPorId(int id) { return default(T); }

        public virtual void Atualizar(T t) { }

        public virtual void Excluir(T t) { }

        public virtual void ExcluirPorId(int id) { }
    }
}
