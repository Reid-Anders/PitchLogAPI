using System.Text;
using System.Linq.Dynamic.Core;

namespace PitchLogAPI.Helpers
{
    public static class IQueryableExtensionMethods
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string orderByClauses)
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

                    if(!(new string[] { "asc", "desc" }.Contains(splitClause[1])))
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
