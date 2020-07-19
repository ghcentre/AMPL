using System;
using System.Web.Mvc;

namespace Ampl.Web.Mvc
{
    /// <summary>
    /// Provides an attribute which, applied to the property of the model,
    /// controls whether this property is visible in read-write views.
    /// </summary>
    /// <example>
    /// <para>In the following example, the <c>FileContents</c> property
    /// is visible in read-only (edit) views only.</para>
    /// <code>
    ///   public class SampleViewModel
    ///   {
    ///     public string ExistingFileContents { get; set; }
    ///     
    ///     [Display(Name = "UploadNewFile")]
    ///     [DataType(DataType.Upload)]
    ///     [ShowForEdit(false)]
    ///     public HttpPostedFileBase FileContents { get; set; }
    ///   }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ShowForEditAttribute : Attribute, IMetadataAware
    {
        private readonly bool _showForEdit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowForDisplayAttribute"/> class.
        /// </summary>
        /// <param name="showForEdit"><see langword="true" /> to display the property
        /// this attrubute is applies to, in read-write views,
        /// or <see langword="false"/> to hide the property in read-write views.</param>
        public ShowForEditAttribute(bool showForEdit)
        {
            _showForEdit = showForEdit;
        }

        /// <summary>
        /// Provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.ShowForEdit = _showForEdit;
        }
    }
}
