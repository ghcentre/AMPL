using System;
using System.Collections.Generic;

namespace Ampl.Core;

/// <summary>
/// Implements the <see cref="IEqualityComparer{T}"/> allowing to specify equality comparer and hash code generator
/// as constructor arguments.
/// </summary>
/// <typeparam name="T">The type of objects to compare</typeparam>
public class InlineEqualityComparer<T> : EqualityComparer<T>
{
    private readonly Func<T, T, bool> _equals;
    private readonly Func<T, int> _getHashCode;


    /// <summary>
    /// Initializes a new instance of the <see cref="InlineEqualityComparer{T}"/> class.
    /// </summary>
    /// <param name="equals">The delegate implementing equality comparer.</param>
    /// <param name="getHashCode">The delegate implementing hash code generator.</param>
    public InlineEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
    {
        _equals = Guard.Against.Null(equals, nameof(equals));
        _getHashCode = Guard.Against.Null(getHashCode, nameof(getHashCode));
    }


    /// <inheritdoc />
    public override bool Equals(T x, T y) => _equals(x, y);

    /// <inheritdoc />
    public override int GetHashCode(T obj) => _getHashCode(obj);
}
