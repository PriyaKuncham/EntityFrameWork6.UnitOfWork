using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Async;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public class IncludableQueryable<TEntity, TProperty> : IIncludableQueryable<TEntity, TProperty>, IAsyncEnumerable<TEntity>
    {
        private readonly IQueryable<TEntity> _queryable;

        public IncludableQueryable(IQueryable<TEntity> queryable)
        {
            _queryable = queryable;
        }

        public Expression Expression => _queryable.Expression;
        public Type ElementType => _queryable.ElementType;
        public IQueryProvider Provider => _queryable.Provider;

        public IEnumerator<TEntity> GetEnumerator() => _queryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        Task<IAsyncEnumerator<TEntity>> IAsyncEnumerable<TEntity>.GetEnumerator(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
