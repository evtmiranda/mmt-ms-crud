﻿using ms_crud_rest.HelperClasses;
using NHibernate;
using System.Collections.Generic;

namespace ms_crud_rest.DAO
{
    public class GenericDAO <T>
    {
        public virtual void Adicionar(T t)
        {
            using (ISession session = NHibernateHelper.AbreSession())
            {
                ITransaction tx = session.BeginTransaction();
                session.Save(t);
                tx.Commit();
            }
        }

        public virtual IList<T> Listar()
        {
            using (ISession session = NHibernateHelper.AbreSession())
            {
                string hql = string.Format("select t from {0} t", typeof(T).Name);
                IQuery query = session.CreateQuery(hql);
                return query.List<T>();
            }
        }

        public T BuscarPorId(int id)
        {
            using (ISession session = NHibernateHelper.AbreSession())
            {
                return session.Get<T>(id);
            }
        }

        public virtual void Atualizar(T t)
        {
            using (ISession session = NHibernateHelper.AbreSession())
            {
                ITransaction tx = session.BeginTransaction();
                session.Update(t);
                tx.Commit();
            }
        }

        public virtual void Excluir(T t)
        {
            using (ISession session = NHibernateHelper.AbreSession())
            {
                ITransaction tx = session.BeginTransaction();
                session.Delete(t);
                tx.Commit();
            }
        }

        public virtual void ExcluirPorId(int id)
        {
            using (ISession session = NHibernateHelper.AbreSession())
            {
                T t = BuscarPorId(id);

                ITransaction tx = session.BeginTransaction();
                session.Delete(t);
                tx.Commit();
            }
        }
    }
}
