using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ampl.Web.Mvc
{
  /// <summary>
  /// Provides an attribute which, applied to the property of the model,
  /// controls whether this property is visible in read-only views.
  /// </summary>
  /// <example>
  /// <para>In the following example, the <c>FileContents</c> property
  /// is visible in read-write (edit) views only.</para>
  /// <code>
  ///   public class SampleViewModel
  ///   {
  ///     public string ExistingFileContents { get; set; }
  ///     
  ///     [Display(Name = "UploadNewFile")]
  ///     [DataType(DataType.Upload)]
  ///     [ShowForDisplay(false)]
  ///     public HttpPostedFileBase FileContents { get; set; }
  ///   }
  /// </code>
  /// </example>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public sealed class ShowForDisplayAttribute : Attribute, IMetadataAware
  {
    private bool _showForDisplay;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShowForDisplayAttribute"/> class.
    /// </summary>
    /// <param name="showForDisplay"><see langword="true" /> to display the property
    /// this attrubute is applies to, in read-only views,
    /// or <see langword="false"/> to hide the property in read-only views.</param>
    public ShowForDisplayAttribute(bool showForDisplay)
    {
      _showForDisplay = showForDisplay;
    }

    /// <summary>
    ///Provides metadata to the model metadata creation process.
    /// </summary>
    /// <param name="metadata">The model metadata.</param>
    public void OnMetadataCreated(ModelMetadata metadata)
    {
      metadata.ShowForDisplay = _showForDisplay;
    }
  }
}
