using System;
using System.Web.Mvc;

namespace Ampl.Web.Mvc
{
    /// <summary>
    /// Renders the model property as drop down list. The <see cref="ItemContainer"/> property of the attribute
    /// contains the name of the property or method which returns the sequence of list items.
    /// </summary>
    /// <example>
    /// <code>
    ///   public class SampleViewModel
    ///   {
    ///     //
    ///     // This example uses method to get items for the drop down list.
    ///     // You may use a property such as:
    ///     //   public IEnumerable&lt;SelectListItem&gt; SamplePropertyItems { get { return .... } }
    ///     //
    ///     [DropDownList("GetSamplePropertyItems")]
    ///     public string SampleProperty { get; set; }
    ///     
    ///     public IEnumerable&lt;SelectListItem&gt; GetSamplePropertyItems()
    ///     {
    ///       return new[] {
    ///         new SelectListItem() { Value = 0, Text = "Zero" },
    ///         new SelectListItem() { Value = 1, Text = "One" }
    ///       };
    ///     }
    ///   }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DropDownListAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownListAttribute"/> class.
        /// </summary>
        /// <param name="itemContainer">The name of the model's property or method name
        /// which is used to retrieve the sequence of <see cref="SelectListItem"/>
        /// to fill the drop down list.</param>
        public DropDownListAttribute(string itemContainer)
        {
            //
            // itemContainer may be null to conform AutoMapper's 
            //  cfg.CreateMap<TSrc, TDest>().ForMember(d => d.SomeProperty, opts => opts.Ignore())
            //
            //Check.NotNullOrEmptyString(ItemContainer, nameof(ItemContainer));
            //
            ItemContainer = itemContainer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownListAttribute"/> class.
        /// </summary>
        /// <param name="enumType">The type of the Enum which values are used to construct a sequence of the
        /// <see cref="SelectListItem"/> to fill the drop down list.</param>
        public DropDownListAttribute(Type enumType)
        {
            EnumType = enumType;
        }

        /// <summary>
        /// Gets the model's property or method name which is used to retrieve the sequence of <see cref="SelectListItem"/>
        /// to fill the drop down list.
        /// </summary>
        /// <remarks>The method or property name must be public and may be <see langword="static"/>.</remarks>
        /// <value>The name of the property or method.</value>
        public string ItemContainer { get; private set; }

        /// <summary>
        /// Gets the Enum type to be used as the source for the sequence of <see cref="SelectListItem"/>
        /// to fill the drop down list.
        /// </summary>
        public Type EnumType { get; private set; }

        /// <summary>
        /// Provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (metadata.TemplateHint == null)
            {
                metadata.TemplateHint = "DropDownList";
            }
        }
    }
}
