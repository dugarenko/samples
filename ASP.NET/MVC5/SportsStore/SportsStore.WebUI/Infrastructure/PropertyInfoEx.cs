using System;
using System.Linq.Expressions;

namespace SportsStore.WebUI.Infrastructure
{
    public static class PropertyInfoEx
    {
        /// <summary>
		/// Zwraca nazwę właściwości.
		/// </summary>
		/// <typeparam name="T">Typ danych.</typeparam>
		/// <param name="property">Właściwość.</param>
		public static string GetPropertyName<T>(Expression<Func<T>> property)
        {
            LambdaExpression lambdaExpression = (LambdaExpression)property;
            MemberExpression memberExpression = (!(lambdaExpression.Body is UnaryExpression) ?
                (MemberExpression)lambdaExpression.Body : (MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
            return memberExpression.Member.Name;
        }
    }
}