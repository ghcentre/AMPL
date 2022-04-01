﻿using System;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace Ampl.Core;

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
    /// <para>See <see cref="CompactGuid.Parse(ReadOnlySpan{char})"/> to convert the compact GUID representation
    /// to the <see cref="Guid"/> value.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var guid = new Guid(Enumerable.Range(0, 16).Select(x => (byte)x).ToArray()); // {03020100-0504-0706-0809-0a0b0c0d0e0f}
    /// string compactString = guid.ToCompactString(); // AAECAwQFBgcICQoLDA0ODw
    /// </code>
    /// <seealso cref="CompactGuid.Parse(ReadOnlySpan{char})"></seealso>
    /// </example>
    public static string ToCompactString(this Guid guid)
    {
        Span<byte> guidBytes = stackalloc byte[CompactGuidConstants.GuidByteArrayLength];
        Span<byte> base64Bytes = stackalloc byte[CompactGuidConstants.Base64GuidStringLength];

        MemoryMarshal.TryWrite(guidBytes, ref guid);
        Base64.EncodeToUtf8(guidBytes, base64Bytes, out int bytesConsumed, out int bytesWritten);

        Span<char> resultChars = stackalloc char[CompactGuidConstants.CompactGuidStringLength];

        for (int i = 0; i < CompactGuidConstants.CompactGuidStringLength; i++)
        {
            resultChars[i] = base64Bytes[i] switch
            {
                CompactGuidConstants.Bytes.Plus => CompactGuidConstants.Chars.Minus,
                CompactGuidConstants.Bytes.Slash => CompactGuidConstants.Chars.Underscore,
                _ => (char)base64Bytes[i]
            };
        }

        string result = new(resultChars);
        return result;
    }
}
