using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Twitter.Core.Entity;
using Twitter.Core.Entity.Enum;
using Twitter.Core.Service;
using Twitter.Model.Context;

namespace Twitter.Service
{
    public class BaseService<T> : ICoreService<T> where T : CoreEntity
    {
        private readonly TwitterContext context;

        public BaseService(TwitterContext context)
        {
            this.context = context;
        }

        public bool Activate(Guid id)
        {
            T activated = GetById(id);
            activated.Status = Status.Active;
            return Update(activated);
        }

        public bool Add(T item)
        {
            try
            {
                context.Set<T>().Add(item);
                return Save() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                //Hata alırsa rollback yapacak
                using (TransactionScope ts = new TransactionScope())
                {
                    context.Set<T>().AddRange(items);
                    ts.Complete();
                    return Save() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Any(Expression<Func<T, bool>> exp) => context.Set<T>().Any(exp);

        public List<T> GetActive() => context.Set<T>().Where(x => x.Status != Status.Deleted).ToList();

        public List<T> GetAll() => context.Set<T>().ToList();

        public T GetByDefault(Expression<Func<T, bool>> exp) => context.Set<T>().FirstOrDefault(exp);

        public T GetById(Guid id) => context.Set<T>().Find(id);

        public List<T> GetDefault(Expression<Func<T, bool>> exp) => context.Set<T>().Where(exp).ToList();

        public bool Remove(T item)
        {
            item.Status = Status.Deleted;
            return Update(item);
        }

        public bool Remove(Guid id)
        {
            try
            {

                T item = GetById(id);
                item.Status = Status.Deleted;
                return Update(item);

            }
            catch
            {
                return false;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var collection = GetDefault(exp);
                    int count = 0;
                    foreach (var item in collection)
                    {
                        item.Status = Status.Deleted;
                        bool operationResult = Update(item);
                        if (operationResult) count++;
                    }

                    if (collection.Count == count) ts.Complete();
                    else return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int Save() => context.SaveChanges();

        public bool Update(T item)
        {
            try
            {
                context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
