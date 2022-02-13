using System;

namespace Ampl.Core;

/// <summary>
/// Provides <b>Maybe monad</b> implementation via extension methods.
/// </summary>
/// <example>
/// <code><![CDATA[
/// //
/// // Safe property access
/// // (Similar to C# 6.0 null propagating operator; but allows to modify return values.) 
/// //
/// Exception exception = GetException();
/// string innerMessage = exception.With(x => x.InnerException).With(x => x.Message);
/// 
/// //
/// // Decorate string with HTML tags only if the string is not null.
/// //
/// string s = GetString();
/// string heading = s.With(x => $"<h1>{x}</h1>"); // heading is null if s is null
/// 
/// //
/// // Validate input, evaluate if input is valid, return model or not found result.
/// // Note that Nullable<T> can also be used.
/// //
/// int? blogId = 420;
/// return blogId
///     .If(id => id > 42)
///     .With(id => _repository.GetBlog(id))
///     .Return(blog => GenerateModel(blog), () => GenerateNotFound());
/// 
/// //
/// // Increment views for special blogs only.
/// //
/// int? blogId = GetSomeBlogId();
/// blogId
///     .With(id => _repository.GetBlog(id))
///     .If(blog => blog.IsSpecial)
///     .Do(blog => _blogViewService.IncrementViews(blog))
///     .Return(blog => blog, null);
/// ]]></code>
/// </example>
public static class MaybeMonadExtensions
{
    /// <summary>
    /// Evaluates the <paramref name="evaluator"/> delegate only if the input is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the <paramref name="input"/> argument.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="input">The object whose properties need to be safely accessed.</param>
    /// <param name="evaluator">The <see cref="Func{T, TResult}"/> which is evaluated only
    /// if the value specified in <paramref name="input"/> is not <see langword="null"/>.</param>
    /// <returns>
    /// The result of the <paramref name="evaluator"/>, or,
    /// <see langword="null"/> if the <paramref name="input"/> is <see langword="null"/>.
    /// </returns>
    /// <example>
    /// See the <see cref="MaybeMonadExtensions"/> class documentation for the examples.
    /// </example>
    public static TResult? With<TInput, TResult>(this TInput? input,
                                                 Func<TInput, TResult?> evaluator)
    {
        if (input == null)
        {
            return default;
        }

        Guard.Against.Null(evaluator, nameof(evaluator));

        return evaluator(input);
    }

    /// <summary>
    /// Evaluates the <paramref name="evaluator"/> delegate and returns the evaluation result 
    /// if <paramref name="input"/> is not <see langword="null"/>. Otherwise, returns the <paramref name="fallbackValue"/> value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <param name="input">The input</param>
    /// <param name="evaluator">The evaluator delegate.</param>
    /// <param name="fallbackValue">The value to return if the <paramref name="input"/> is <see langword="null"/>.</param>
    /// <returns>If the <paramref name="input"/> value is not <see langword="null"/>, the method
    /// executes the <paramref name="evaluator"/> delegate and returns its result.
    /// Otherwise, the method returns the <paramref name="fallbackValue"/>.</returns>
    /// <example>
    /// See the <see cref="MaybeMonadExtensions"/> class documentation for the examples.
    /// </example>
    public static TResult Return<TInput, TResult>(this TInput input,
                                                  Func<TInput, TResult> evaluator,
                                                  TResult fallbackValue)
    {
        if (input == null)
        {
            return fallbackValue;
        }

        Guard.Against.Null(evaluator, nameof(evaluator));

        return evaluator(input);
    }

    /// <summary>
    /// Evaluates the <paramref name="evaluator"/> delegate if <paramref name="input"/> is not <see langword="null"/>,
    /// otherwise evaluates the <paramref name="fallbackEvaluator"/> delegate.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <param name="input">The input value.</param>
    /// <param name="evaluator">The <see cref="Func{T, TResult}"/> has to be evaluated
    /// if the <paramref name="input"/> is not <see langword="null"/>.</param>
    /// <param name="fallbackEvaluator">The <see cref="Func{TResult}"/> has to be evaluated
    /// if the <paramref name="input"/> is <see langword="null"/>.</param>
    /// <returns>The result of either <c>evaluatorResult</c> or <c>fallbackResult</c></returns>
    /// <example>
    /// See the <see cref="MaybeMonadExtensions"/> class documentation for the examples.
    /// </example>
    public static TResult Return<TInput, TResult>(this TInput input,
                                                  Func<TInput, TResult> evaluator,
                                                  Func<TResult> fallbackEvaluator)
    {
        if (input == null)
        {
            Guard.Against.Null(fallbackEvaluator, nameof(fallbackEvaluator));

            return fallbackEvaluator();
        }

        Guard.Against.Null(evaluator, nameof(evaluator));

        return evaluator(input);
    }

    /// <summary>
    /// Evaluates the <paramref name="predicate"/> delegate if the input is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the input</typeparam>
    /// <param name="input">The input value</param>
    /// <param name="predicate">The delegate to evaluate if <paramref name="input"/> is not <see langword="null"/>.</param>
    /// <returns>If the input is not <see langword="null"/> and <paramref name="predicate"/> succeeds,
    /// the method returns the value specified in <paramref name="input"/>.
    /// Otherwise the method returns <see langword="null"/>.</returns>
    /// <example>
    /// See the <see cref="MaybeMonadExtensions"/> class documentation for the examples.
    /// </example>
    public static TInput? If<TInput>(this TInput? input,
                                     Func<TInput, bool> predicate)
    {
        if (input == null)
        {
            return default;
        }

        Guard.Against.Null(predicate, nameof(predicate));

        return predicate(input) ? input : default;
    }

    /// <summary>
    /// Evaluates the <paramref name="action"/> delegate if <paramref name="input"/> is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input value</param>
    /// <param name="action">The action to be evaluated if <paramref name="input"/> is not <see langword="null"/>.</param>
    /// <returns>If the value specified in <paramref name="input"/> is not <see langword="null"/>,
    /// the method evaluates the delegate specified in <paramref name="action"/> and returns the input value.
    /// Otherwise, the method returns <see langword="null"/>.</returns>
    /// <example>
    /// See the <see cref="MaybeMonadExtensions"/> class documentation for the examples.
    /// </example>
    public static TInput? Do<TInput>(this TInput? input,
                                     Action<TInput> action)
    {
        if (input == null)
        {
            return default;
        }

        Guard.Against.Null(action, nameof(action));
        action(input);

        return input;
    }

    /// <summary>
    /// Evaluates the <paramref name="notNullAction"/> delegate if <paramref name="input"/> is not <see langword="null"/>,
    /// or,
    /// evaluates the <paramref name="nullAction"/> delegate otherwise.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input value</param>
    /// <param name="notNullAction">The action to be evaluated if <paramref name="input"/> is not <see langword="null"/>.</param>
    /// <param name="nullAction">The action to be evaluated if <paramref name="input"/> is <see langword="null"/>.</param>
    /// <returns>If the value specified in <paramref name="input"/> is not <see langword="null"/>,
    /// the method evaluates the delegate specified in <paramref name="notNullAction"/>,
    /// otherwise, the method evaluates the delegate specified in <paramref name="nullAction"/>.
    /// The method always returns the value specified in <paramref name="input"/>.</returns>
    /// <example>
    /// See the <see cref="MaybeMonadExtensions"/> class documentation for the examples.
    /// </example>
    public static TInput Do<TInput>(this TInput input,
                                    Action<TInput> notNullAction,
                                    Action<TInput> nullAction)
    {
        if (input != null)
        {
            notNullAction?.Invoke(input);
            return input;
        }

        nullAction?.Invoke(input);
        return input;
    }
}
