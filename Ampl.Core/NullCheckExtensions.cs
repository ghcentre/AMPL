using System;
using System.Runtime.CompilerServices;

namespace Ampl.Core
{
    /// <summary>
    /// Provides a set of <see langword="static"/> methods for <see langword="null"/> checking.
    /// </summary>
    public static class NullCheckExtensions
    {
        /// <summary>
        /// Evaluates the delegate only if the input is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TInput">The type of the <paramref name="input"/> argument.</typeparam>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="input">The object whose properties need to be safely accessed.</param>
        /// <param name="evaluatorResult">The <see cref="Func{T, TResult}"/> which is evaluated only
        /// if the value specified in <paramref name="input"/> is not <see langword="null"/>.</param>
        /// <returns>
        /// The result of the <paramref name="evaluatorResult"/>, or, <see langword="null"/> if the <paramref name="input"/> is <see langword="null"/>.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        ///   string s = SomeMethodReturningStringOrNull();
        ///   string heading = s.With(s => $"<h1>{s}</h1>"); // null if s is null
        /// ]]>/// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult With<TInput, TResult>(this TInput input,
                                                    Func<TInput, TResult> evaluatorResult)
        {
            if(input == null)
            {
                return default(TResult);
            }

            Check.NotNull(evaluatorResult, nameof(evaluatorResult));
            return evaluatorResult(input);
        }

        /// <summary>
        /// Evaluates the <paramref name="evaluatorResult"/> function
        /// only if <paramref name="input"/> is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="input"></param>
        /// <param name="evaluatorResult"></param>
        /// <param name="fallbackValue"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Return<TInput, TResult>(this TInput input,
                                                      Func<TInput, TResult> evaluatorResult,
                                                      TResult fallbackValue)
        {
            if(input == null)
            {
                return fallbackValue;
            }

            Check.NotNull(evaluatorResult, nameof(evaluatorResult));
            return evaluatorResult(input);
        }

        /// <summary>
        /// Evaluates the <paramref name="evaluatorResult"/> only if <paramref name="input"/> is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="input">The input value.</param>
        /// <param name="evaluatorResult">The <see cref="Func{T, TResult}"/> has to be evaluated
        /// if the <paramref name="input"/> is not <see langword="null"/>.</param>
        /// <param name="fallbackResult">The <see cref="Func{TResult}"/> has to be evaluated
        /// if the <paramref name="input"/> is <see langword="null"/>.</param>
        /// <returns>The result of either <c>evaluatorResult</c> or <c>fallbackResult</c></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Return<TInput, TResult>(this TInput input,
                                                      Func<TInput, TResult> evaluatorResult,
                                                      Func<TResult> fallbackResult)
        {
            if(input == null)
            {
                Check.NotNull(fallbackResult, nameof(fallbackResult));
                return fallbackResult();
            }

            Check.NotNull(evaluatorResult, nameof(evaluatorResult));
            return evaluatorResult(input);
        }

        /// <summary>
        /// Evaluates the predicate if input is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="input"></param>
        /// <param name="predicate"></param>
        /// <returns>If the input is not null and predicate succeeds the method returns input. Otherwise the
        /// method returns null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TInput If<TInput>(this TInput input, Func<TInput, bool> predicate)
        {
            if(input == null)
            {
                return default(TInput);
            }

            Check.NotNull(predicate, nameof(predicate));
            return predicate(input) ? input : default(TInput);
        }

        /// <summary>
        /// Evaluates the action if input is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="input"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TInput Do<TInput>(this TInput input, Action<TInput> action)
        {
            if(input == null)
            {
                return default(TInput);
            }

            Check.NotNull(action, nameof(action));
            action(input);
            return input;
        }

        /// <summary>
        /// Evaluates one of the actions if input is or is not <see langword="null" />.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="input"></param>
        /// <param name="notNullAction">The action (<see cref="Action{T}"/>)
        /// to be evaluated if the <paramref name="input"/> is <b>not</b> null.
        /// The action can be <see langword="null" />; if so, nothing is evaluated.</param>
        /// <param name="nullAction">The action (<see cref="Action{T}"/>)
        /// to be evaluated if the <paramref name="input"/> is null.
        /// The action can be <see langword="null" />; if so, nothing is evaluated.</param>
        /// <returns>The method returns <paramref name="input"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TInput Do<TInput>(this TInput input, Action<TInput> notNullAction, Action<TInput> nullAction)
        {
            if(input != null)
            {
                notNullAction?.Invoke(input);
            }
            else
            {
                nullAction?.Invoke(input);
            }

            return input;
        }
    }
}
