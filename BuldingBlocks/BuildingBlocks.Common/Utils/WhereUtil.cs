using System;
using System.Linq;
using System.Linq.Expressions;

namespace BuildingBlocks.Common.Utils
{
    public static class WhereUtil
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool isSatisfy,
            Expression<Func<T, bool>> expression) =>
            isSatisfy ? query.Where(expression) : query;
    }
}