using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVVMCore.Threading.Tasks
{
    [EditorBrowsable(EditorBrowsableState.Never)]
	public static class TaskCancellationHelper
    {
        public static async void FireAndForget(Task @this)
        {
            if (@this != null)
            {
                try
                {
                    await @this;
                }
                catch (OperationCanceledException)
                { }
            }
        }

        public static void FireAndForget(Func<Task> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            FireAndForget(func());
        }

        public static void FireAndForget<T>(Func<T, Task> func, T param)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            FireAndForget(func(param));
        }

        public static AggregateException RemoveCancellationExceptions(this AggregateException aggregateException)
        {
            if (aggregateException == null)
            {
                throw new ArgumentNullException("aggregateException");
            }

            List<Exception> exceptions = new List<Exception>();
            if (aggregateException.InnerExceptions != null)
            {
                foreach (Exception innerException in aggregateException.InnerExceptions)
                {
                    if (innerException is OperationCanceledException)
                    {
                        continue;
                    }

                    AggregateException aggregateException1 = innerException as AggregateException;
                    if (aggregateException1 == null)
                    {
                        exceptions.Add(innerException);
                    }
                    else
                    {
                        aggregateException1 = aggregateException1.RemoveCancellationExceptions();
                        if (aggregateException1 == null)
                        {
                            continue;
                        }
                        exceptions.Add(aggregateException1);
                    }
                }
            }

            if (exceptions.Any())
            {
                return new AggregateException(aggregateException.Message, exceptions);
            }

            return null;
        }
    }
}
