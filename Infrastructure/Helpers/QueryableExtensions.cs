using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy2<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(toLambda<T>(propertyName));
        }
        public static IOrderedQueryable<T> OrderByDescending2<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(toLambda<T>(propertyName));
        }
        private static Expression<Func<T, object>> toLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propertyAsObject = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(propertyAsObject, parameter);
        }
    }
}
