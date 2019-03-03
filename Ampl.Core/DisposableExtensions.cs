using System;
using System.Runtime.CompilerServices;

namespace Ampl.Core
{
    /// <summary>
    /// Provides a <see langword="static"/> extension methods for the <see cref="IDisposable"/> interface.
    /// </summary>
    public static class DisposableExtensions
    {
        /// <summary>
        /// Invokes the delegate in the context of the disposable object.
        /// </summary>
        /// <typeparam name="T">The type of the disposable object. Must implement <see cref="IDisposable"/> interface.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="obj">The object on which context the delegate will be invoked.</param>
        /// <param name="func">The deledate to invoke.</param>
        /// <returns>
        /// <para>If the <paramref name="obj"/> is <see langword="null"/>,
        /// the method returns the default of <typeparamref name="TReturn"/>.</para>
        /// <para>Otherwise the method invokes the delegate specified in <paramref name="func"/> and returns its result.</para>
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="func"/> is <see langword="null"/>.</exception>
        /// <remarks>The disposable object's <see cref="IDisposable.Dispose"/> method is called immediately after the delegate
        /// returns.
        /// </remarks>
        /// <example>
        /// <code>
        /// //
        /// // Reads entire text file
        /// // After the file contents are read, the StreamReader is disposed.
        /// //
        /// string contents = new StreamReader(@"c:\windows\win.ini").Use(sr => sr.ReadToEnd());
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TReturn Use<T, TReturn>(this T obj, Func<T, TReturn> func) where T : class, IDisposable
        {
            if(obj == null)
            {
                return default(TReturn);
            }

            Check.NotNull(func, nameof(func));
            using(obj)
            {
                return func(obj);
            }
        }
    }
}
