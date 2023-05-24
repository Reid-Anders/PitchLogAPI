using System.Text;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PitchLogAPI.Helpers
{
    public static class IQueryableExtensionMethods
    {
        public static IQueryable<T> ApplyComparisonFilter<T>(this IQueryable<T> query, string propertyName, string[] comparisonClauses)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var type = typeof(T);
            if (type.GetProperty(propertyName) == null)
            {
                throw new ArgumentException($"{propertyName} invalid for type {type.Name}");
            }

            Expression? root = null;
            var parameter = Expression.Parameter(type, "item");
            var memberExpression = Expression.Property(parameter, type.GetProperty(propertyName));

            foreach (var comparisonClause in comparisonClauses)
            {
                var splitComparison = comparisonClause.Split(':');
                var op = splitComparison[0];
                int.TryParse(splitComparison[1], out int comparisonValue);

                var constantExpression = Expression.Constant(comparisonValue, typeof(int?));

                Expression? body = null;
                if (op == "gte")
                {
                    body = Expression.GreaterThanOrEqual(memberExpression, constantExpression);
                }

                if (op == "gt")
                {
                    body = Expression.GreaterThan(memberExpression, constantExpression);
                }

                if (op == "lt")
                {
                    body = Expression.LessThan(memberExpression, constantExpression);
                }

                if (op == "lte")
                {
                    body = Expression.LessThanOrEqual(memberExpression, constantExpression);
                }

                root = root == null ? body : Expression.And(root, body);
            }

            if (root != null)
            {
                var lambdaExpression = Expression.Lambda(root, parameter);
                return query.Where(lambdaExpression);
            }
            else
            {
                return query;
            }
        }

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string orderByClauses)
        {
            var splitClauses = orderByClauses.Split(',');
            var orderByStringBuilder = new StringBuilder();

            foreach (var clause in splitClauses)
            {
                string trimmedClause = clause.Trim().ToLower();

                string propertyName = trimmedClause;
                bool isAscending = true;

                if (trimmedClause.Contains(":"))
                {
                    var splitClause = trimmedClause.Split(':');

                    if (splitClause.Length != 2)
                    {
                        throw new ArgumentException("Invalid format of order by clause.");
                    }

                    if (!(new string[] { "asc", "desc" }.Contains(splitClause[1])))
                    {
                        throw new ArgumentException($"Order by {splitClause[1]} invalid. Must order by either 'asc' or 'desc'.");
                    }

                    propertyName = splitClause[0];

                    if (splitClause[1] == "desc")
                    {
                        isAscending = !isAscending;
                    }
                }

                bool typeHasProperty = typeof(T).GetProperties().Any(propInfo => propInfo.Name.ToLower() == propertyName);

                if (!typeHasProperty)
                {
                    throw new ArgumentException($"{propertyName} invalid for type {typeof(T).Name}.");
                }

                string order = isAscending ? "ascending" : "descending";
                orderByStringBuilder.Append($", {propertyName} {order}");
            }

            return query.OrderBy(orderByStringBuilder.ToString().Trim(',').Trim());
        }
    }
}
