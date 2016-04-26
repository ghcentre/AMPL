namespace CodeDucky
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Throw
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given value is null
        /// </summary>
        public static void IfNull<T>(T value, string parameterName)
        {
            Throw<ArgumentNullException>.If(value == null, parameterName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the given condition is true
        /// </summary>
        public static void If(bool condition, string parameterName)
        {
            Throw<ArgumentException>.If(condition, parameterName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the given value is outside of the specified range
        /// </summary>
        public static void IfOutOfRange<T>(T value, string paramName, T? min = null, T? max = null)
            where T : struct, IComparable<T>
        {
            if (min.HasValue && value.CompareTo(min.Value) < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, string.Format("Expected: >= {0}, but was {1}", min, value));
            }
            if (max.HasValue && value.CompareTo(max.Value) > 0)
            {
                throw new ArgumentOutOfRangeException(paramName, string.Format("Expected: <= {0}, but was {1}", max, value));
            }
        }
    }

    public static class Throw<TException>
        where TException : Exception
    {
        /// <summary>
        /// Throws an exception of type <see cref="TException"/> if the condition is true
        /// </summary>
        public static void If(bool condition, string message)
        {
            if (condition)
            {
                throw Create(message);
            }
        }

        /// <summary>
        /// As <see cref="Throw.If(bool, string)"/>, but allows the message to be specified lazily. The message function will only be evaluated
        /// if the condition is true
        /// </summary>
        public static void If(bool condition, Func<string> message)
        {
            if (condition)
            {
                throw Create(message());
            }
        }

        private static TException Create(string message)
        {
            return (TException)Activator.CreateInstance(typeof(TException), message);
        }
    }
}

namespace CodeDucky
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Helpers
    {
        /// <summary>
        /// Bounds a value within a range
        /// </summary>
        public static T Capped<T>(this T @this, T? min = null, T? max = null)
            where T : struct, IComparable<T>
        {
            return min.HasValue && @this.CompareTo(min.Value) < 0 ? min.Value
                : max.HasValue && @this.CompareTo(max.Value) > 0 ? max.Value
                : @this;
        }

        /// <summary>
        /// Type-safely casts the given value to the specified type
        /// </summary>
        public static T As<T>(this T @this) 
        { 
            return @this; 
        }

        /// <summary>
        /// Invokes the given function on the given object if and only if the given object is not null. Otherwise,
        /// the value specified by "ifNullReturn" is returned
        /// </summary>
        public static TResult NullSafe<TObj, TResult>(
            this TObj obj,
            Func<TObj, TResult> func,
            TResult ifNullReturn = default(TResult))
        {
            Throw.IfNull(func, "func");
            return obj != null ? func(obj) : ifNullReturn;
        }
    }
}

namespace CodeDucky
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Traverse
    {
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> based on traversing the "linked list" represented by the head node
        /// and the next function. The list terminates when a null node is reached
        /// </summary>
        /// <typeparam name="T">the type of node in the list</typeparam>
        /// <param name="node">the head node of the list</param>
        /// <param name="next">a function that, given a node in the list, generates the next node in the list</param>
        public static IEnumerable<T> Along<T>(T node, Func<T, T> next)
            where T : class
        {
            Throw.IfNull(next, "next");

            for (var current = node; current != null; current = next(current))
            {
                yield return current;
            }
        }
    }
}
