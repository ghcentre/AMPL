namespace Ampl.Core;

internal static class CompactGuidConstants
{
    public const int GuidByteArrayLength = 16;

    public const int Base64GuidStringLength = 24;

    public const int CompactGuidStringLength = 22;

    public static class Chars
    {
        public const char Equal = '=';
        public const char Minus = '-';
        public const char Plus = '+';
        public const char Underscore = '_';
        public const char Slash = '/';
    }

    public static class Bytes
    {
        public const byte Plus = (byte)'+';
        public const byte Slash = (byte)'/';
    }
}
