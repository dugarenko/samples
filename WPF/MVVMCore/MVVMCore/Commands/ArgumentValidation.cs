using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVMCore.Commands
{
	public static class ArgumentValidation
    {
        public static void NotNull([ValidatedNotNull] object variable, string variableName)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(variableName);
            }
        }

        public static void NotNullOrEmpty<T>([ValidatedNotNull] IEnumerable<T> enumerable, string variableName)
        {
            NotNull(enumerable, variableName);
            if (!enumerable.Any())
            {
                throw new ArgumentException(variableName);
            }
        }

        public static TValue NotNullPassThrough<TTarget, TValue>([ValidatedNotNull] TTarget variable, string variableName, Func<TTarget, TValue> valueGetter)
            where TTarget : class
        {
            if (variable == null)
            {
                throw new ArgumentNullException(variableName);
            }
            NotNull(valueGetter, "valueGetter");
            return valueGetter(variable);
        }
    }
}
