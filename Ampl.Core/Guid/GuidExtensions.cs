using System;
using System.Runtime.CompilerServices;

namespace Ampl.Core
{
    /// <summary>
    /// Contains a set of <see langword="static"/> method to work with <see cref="Guid"/>s.
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Returns a compact string representation of the <see cref="Guid"/> instance.
        /// </summary>
        /// <param name="guid">The GUID instance.</param>
        /// <returns>The method returns a <c>compact</c> representation of the GUID instance.</returns>
        /// <remarks>
        /// <para>The <c>compact</c> representation of a GUID is a base-64 representation of the GUID bytes
        /// with <c>+</c> converted to <c>-</c>, <c>/</c> converted to <c>_</c> (base64 URI)
        /// and trailing <c>==</c> removed.</para>
        /// <para>Base64 URI representation is defined in the following RFCs:</para>
        /// <list type="bullet">
        ///     <item>(RFC 3548, par. 4).</item>
        ///     <item>(RFC 1575, appendix C)</item>
        /// </list>
        /// <para>See <see cref="CompactGuid.Parse(string)"/> to convert the compact GUID representation
        /// to the <see cref="Guid"/> value.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var guid = new Guid(Enumerable.Range(0, 16).Select(x => (byte)x).ToArray()); // {03020100-0504-0706-0809-0a0b0c0d0e0f}
        /// string compactString = guid.ToCompactString(); // AAECAwQFBgcICQoLDA0ODw
        /// </code>
        /// <seealso cref="CompactGuid.Parse(string)"></seealso>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToCompactString(this Guid guid)
        {
            byte[] bytes = guid.ToByteArray();

            //
            // "+" => "-"
            // "/" => "_"
            // (RFC 3548, par. 4).
            // (RFC 1575, appendix C)
            //
            string stringValue = Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_");

            //
            // remove trailing "==" as base-64 encoded GUID always ends with "=="
            //
            string compactValue = stringValue[..^2];

            return compactValue;
        }
    }
}
