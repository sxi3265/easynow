using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EasyNow.Dto;

namespace EasyNow.Utility.Extensions
{
    /// <summary>
    /// The enumerable extensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// the Foreach
        /// </summary>
        /// <param name="input">the input</param>
        /// <param name="action">the action</param>
        /// <typeparam name="T">the T</typeparam>
        public static void Foreach<T>(this IEnumerable<T> input, Action<T> action)
        {
            foreach (var item in input)
            {
                action(item);
            }
        }

        /// <summary>
        /// LINQ IEnumerable AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Reference to Id/Key of entity</param>
        /// <param name="parentIdProperty">Reference to parent Id/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<TreeIntKeyEntity<TEntity>> AsHierarchy<TEntity, TProperty>
            (this IEnumerable<TEntity> allItems, Func<TEntity, TProperty> idProperty, Func<TEntity, TProperty> parentIdProperty)
            where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, 0);
        }

        /// <summary>
        /// Creates the hierarchy.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="allItems">All items.</param>
        /// <param name="parentItem">The parent item.</param>
        /// <param name="idProperty">The identifier property.</param>
        /// <param name="parentIdProperty">The parent identifier property.</param>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        private static IEnumerable<TreeIntKeyEntity<TEntity>> CreateHierarchy<TEntity, TProperty>
        (IEnumerable<TEntity> allItems, TEntity parentItem,
         Func<TEntity, TProperty> idProperty, Func<TEntity, TProperty> parentIdProperty, int depth) where TEntity : class
        {
            IEnumerable<TEntity> childs;

            if (parentItem == null)
                childs = allItems.Where(i => parentIdProperty(i).Equals(default(TProperty)));
            else
                childs = allItems.Where(i => parentIdProperty(i).Equals(idProperty(parentItem)));

            if (childs.Any())
            {
                depth++;

                foreach (var item in childs)
                    yield return new TreeIntKeyEntity<TEntity>
                                     {
                                         Entity = item,
                                         Children = CreateHierarchy
                                             (allItems, item, idProperty, parentIdProperty, depth),
                                         Depth = depth
                                     };
            }
        }

        public static string Join<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string JoinStrings<TItem>(this IEnumerable<TItem> sequence, string separator)
        {
            return sequence.JoinStrings(separator, item => item.ToString());
        }

        public static string JoinStrings<TItem>(this IEnumerable<TItem> sequence, string separator, Func<TItem, string> converter)
        {
            StringBuilder seed = new StringBuilder();
            sequence.Aggregate(seed, (builder, item) =>
                {
                    if (builder.Length > 0)
                        builder.Append(separator);
                    builder.Append(converter(item));
                    return builder;
                });
            return seed.ToString();
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool cond,
                                                Expression<Func<T, bool>> predicate)
        {
            return cond ? enumerable.Where(predicate.Compile()) : enumerable;
        }
    }
}