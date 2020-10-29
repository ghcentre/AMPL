using System;
using System.Runtime.CompilerServices;

namespace Ampl.Core
{
    /// <summary>
    /// Contains a set of <see langword="static"/> methods to operate with <c>enum</c>s.
    /// </summary>
    public static class Enums
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Parse<T>(string source, bool ignoreCase = false)
            where T : struct, Enum
        {
            return (T)Enum.Parse(typeof(T), source, ignoreCase);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? ParseAsNullable<T>(string source, bool ignoreCase = false)
            where T : struct, Enum
        {
            return Enum.TryParse(source, ignoreCase, out T result)
                        ? (T?)result
                        : null;
        }
    }
}
