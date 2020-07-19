using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Ampl.Annotations
{
    /// <summary>
    /// Exposes a set of <see langword="static"/> methods to work with data annotations for <see cref="Enum"/>'s.
    /// </summary>
    public static class AnnotationEnumExtensions
    {
        private static DisplayAttribute GetFirstDisplayAttribute(Enum source)
        {
            //return source?.GetType()
            //              .GetTypeInfo()
            //              .GetDeclaredField(source.ToString())
            //              .GetCustomAttributes(typeof(DisplayAttribute), false)
            //              .Select(x => x as DisplayAttribute)
            //              .FirstOrDefault();

            return source.GetType()
                .GetTypeInfo()
                .DeclaredMembers
                .First(x => x.Name == source.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .Select(x => x as DisplayAttribute)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the <see cref="DisplayAttribute.Name"/> property of the <see cref="DisplayAttribute"/> attached
        /// to the enum value.
        /// </summary>
        /// <param name="source">The enum value.</param>
        /// <returns>
        /// <para>The contents of the <c>Name</c> property of the first <see cref="DisplayAttribute"/>
        /// attached to the enum value.</para>
        /// <para>If there is no attribute, the method returns <see langword="null"/>.</para>
        /// </returns>
        public static string GetDisplayName(this Enum source)
        {
            return GetFirstDisplayAttribute(source)?.GetName();
        }

        /// <summary>
        /// Gets the <see cref="DisplayAttribute.Description"/> property of the <see cref="DisplayAttribute"/> attached
        /// to the enum value.
        /// </summary>
        /// <param name="source">The enum value.</param>
        /// <returns>
        /// <para>The contents of the <c>Description</c> property of the first <see cref="DisplayAttribute"/>
        /// attached to the enum value.</para>
        /// <para>If there is no attribute, the method returns <see langword="null"/>.</para>
        /// </returns>
        public static string GetDisplayDescription(this Enum source)
        {
            return GetFirstDisplayAttribute(source)?.GetDescription();
        }
    }
}
