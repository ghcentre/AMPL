using System;

namespace Ampl.Core;

/// <summary>
/// Contains a set of <see langword="static"/> methots that converts the <see cref="Guid"/> compact string representation
/// to the Guid instance.
/// </summary>
public static class CompactGuid
{
    private const string _base64GuidStringTerminator = "==";


    /// <summary>
    /// Converts the compact string representation of a GUID to the equivalent <see cref="Guid"/> structure.
    /// </summary>
    /// <param name="stringValue">The string to convert.</param>
    /// <returns>A structure that contains the value that was parsed.</returns>
    /// <exception cref="ArgumentNullException">Input is <see langword="null"/>.</exception>
    /// <exception cref="FormatException">Input is not in a recognized format.</exception>
    public static Guid Parse(ReadOnlySpan<char> stringValue)
    {
        if (stringValue.Length != CompactGuidConstants.CompactGuidStringLength)
        {
            throw new FormatException(Messages.InputStringWasNotInACorrectFormat);
        }

        Span<char> base64Chars = stackalloc char[CompactGuidConstants.Base64GuidStringLength];

        for (int i = 0; i < CompactGuidConstants.CompactGuidStringLength; i++)
        {
            base64Chars[i] = stringValue[i] switch
            {
                CompactGuidConstants.Chars.Minus => CompactGuidConstants.Chars.Plus,
                CompactGuidConstants.Chars.Underscore => CompactGuidConstants.Chars.Slash,
                _ => stringValue[i]
            };
        }

        base64Chars[CompactGuidConstants.CompactGuidStringLength] = CompactGuidConstants.Chars.Equal;
        base64Chars[CompactGuidConstants.CompactGuidStringLength + 1] = CompactGuidConstants.Chars.Equal;

        Span<byte> guidBytes = stackalloc byte[CompactGuidConstants.GuidByteArrayLength];
        Convert.TryFromBase64Chars(base64Chars, guidBytes, out int bytesWritten);

        if (bytesWritten != CompactGuidConstants.GuidByteArrayLength)
        {
            throw new FormatException(Messages.InputStringWasNotInACorrectFormat);
        }

        var guid = new Guid(guidBytes);
        return guid;
    }
}
