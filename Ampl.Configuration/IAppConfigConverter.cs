using System;

namespace Ampl.Configuration
{
    /// <summary>
    /// Represents a converter for a type.
    /// </summary>
    /// <remarks>
    /// The <see cref="IAppConfigConverter"/> is used to convert various types to a format recognizable by a
    /// <see cref="IAppConfigStore"/>. The default list of converters is available in a default implementation of the
    /// <see cref="IAppConfigConfiguration"/>.
    /// </remarks>
    public interface IAppConfigConverter
    {
        /// <summary>
        /// Determines whether this converter can handle the particular Type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to check.</param>
        /// <returns><see langword="true"/> if this converter can handle the type specified in <paramref name="type"/>,
        /// or <see langword="false"/> otherwise.</returns>
        bool CanConvert(Type type);

        /// <summary>
        /// Converts the string representation to the value.
        /// </summary>
        /// <param name="entityValue">The string representation of the value</param>
        /// <returns>The value.</returns>
        object ReadEntity(string entityValue);

        /// <summary>
        /// Converts the value to its string representation.
        /// </summary>
        /// <param name="objectValue">The value.</param>
        /// <returns>The <see cref="string"/> representation or the value.</returns>
        string WriteEntity(object objectValue);
    }
}