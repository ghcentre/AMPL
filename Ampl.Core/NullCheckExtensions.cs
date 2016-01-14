using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.System
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
    /// <code>
    ///   string s = SomeMethodReturningStringOrNull();
    ///   string heading = s.With(s => $"&lt;h1&gt;{s}&lt;/h1&gt;"; // null if s is null
    /// </code>
    /// </example>
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

/*
    /// <summary>
    /// Evaluates the next delegate if the input or the result of the evaluation of the previous delegate is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the <paramref name="input"/> argument.</typeparam>
    /// <typeparam name="TA">The type of the return value of the <paramref name="evaluatorA"/> function.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="input"></param>
    /// <param name="evaluatorA"></param>
    /// <param name="evaluatorResult"></param>
    /// <returns></returns>
    public static TResult With<TInput, TA, TResult>(this TInput input,
      Func<TInput, TA> evaluatorA,
      Func<TA, TResult> evaluatorResult)
    {
      if(input == null)
      {
        return default(TResult);
      }

      TA a = evaluatorA(input);
      if(a == null)
      {
        return default(TResult);
      }

      return evaluatorResult(a);
    }

    /// <summary>
    /// Evaluates the next delegate if the input or the result of the evaluation of the previous delegate is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TB"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="input"></param>
    /// <param name="evaluatorA"></param>
    /// <param name="evaluatorB"></param>
    /// <param name="evaluatorResult"></param>
    /// <returns></returns>
    public static TResult With<TInput, TA, TB, TResult>(this TInput input,
      Func<TInput, TA> evaluatorA,
      Func<TA, TB> evaluatorB,
      Func<TB, TResult> evaluatorResult)
    {
      if(input == null)
      {
        return default(TResult);
      }

      TA a = evaluatorA(input);
      if(a == null)
      {
        return default(TResult);
      }

      TB b = evaluatorB(a);
      if(b == null)
      {
        return default(TResult);
      }

      return evaluatorResult(b);
    }

    /// <summary>
    /// Evaluates the next delegate if the input or the result of the evaluation of the previous delegate is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TB"></typeparam>
    /// <typeparam name="TC"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="input"></param>
    /// <param name="evaluatorA"></param>
    /// <param name="evaluatorB"></param>
    /// <param name="evaluatorC"></param>
    /// <param name="evaluatorResult"></param>
    /// <returns></returns>
    public static TResult With<TInput, TA, TB, TC, TResult>(this TInput input,
      Func<TInput, TA> evaluatorA,
      Func<TA, TB> evaluatorB,
      Func<TB, TC> evaluatorC,
      Func<TC, TResult> evaluatorResult)
    {
      if(input == null)
      {
        return default(TResult);
      }

      TA a = evaluatorA(input);
      if(a == null)
      {
        return default(TResult);
      }

      TB b = evaluatorB(a);
      if(b == null)
      {
        return default(TResult);
      }

      TC c = evaluatorC(b);
      if(c == null)
      {
        return default(TResult);
      }

      return evaluatorResult(c);
    }

    /// <summary>
    /// Evaluates the next delegate if the input or the result of the evaluation of the previous delegate is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TB"></typeparam>
    /// <typeparam name="TC"></typeparam>
    /// <typeparam name="TD"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="input"></param>
    /// <param name="evaluatorA"></param>
    /// <param name="evaluatorB"></param>
    /// <param name="evaluatorC"></param>
    /// <param name="evaluatorD"></param>
    /// <param name="evaluatorResult"></param>
    /// <returns></returns>
    public static TResult With<TInput, TA, TB, TC, TD, TResult>(this TInput input,
      Func<TInput, TA> evaluatorA,
      Func<TA, TB> evaluatorB,
      Func<TB, TC> evaluatorC,
      Func<TC, TD> evaluatorD,
      Func<TD, TResult> evaluatorResult)
    {
      if(input == null)
      {
        return default(TResult);
      }

      TA a = evaluatorA(input);
      if(a == null)
      {
        return default(TResult);
      }

      TB b = evaluatorB(a);
      if(b == null)
      {
        return default(TResult);
      }

      TC c = evaluatorC(b);
      if(c == null)
      {
        return default(TResult);
      }

      TD d = evaluatorD(c);
      if(d == null)
      {
        return default(TResult);
      }

      return evaluatorResult(d);
    }
 */

    /// <summary>
    /// Evaluates the <paramref name="evaluatorResult"/> function only if <paramref name="input"/> is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="input"></param>
    /// <param name="evaluatorResult"></param>
    /// <param name="fallbackValue"></param>
    /// <returns></returns>
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

    //public static bool ReturnSuccess<TInput>(this TInput o)
    //  where TInput : class
    //{
    //  return o != null;
    //}

    /// <summary>
    /// Evaluates the predicate if input is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="input"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TInput If<TInput>(this TInput input, Predicate<TInput> predicate)
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
  }
}
