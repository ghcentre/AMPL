using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Web.Mvc
{
  /// <summary>
  /// Configuration for Display and Editor Templates.
  /// </summary>
  public class EditorTemplateConfiguration
  {
    /// <summary>
    /// Gets or sets the Default configuration used as template for all newly created
    /// <see cref="EditorTemplateConfiguration"/> instances.
    /// </summary>
    public static EditorTemplateConfiguration DefaultConfiguration { get; set; } = new EditorTemplateConfiguration();

    /// <summary>
    /// <see langword="true" /> to render null properties, or <see langword="false" /> otherwise.
    /// </summary>
    public bool RenderNullProperties { get; set; } = (DefaultConfiguration?.RenderNullProperties ?? true);

    /// <summary>
    /// CSS Class for the label column.
    /// </summary>
    public string LabelClass { get; set; } = (DefaultConfiguration?.LabelClass ?? "col-xs-12 col-sm-4 col-md-3");

    /// <summary>
    /// CSS Class for the editor column.
    /// </summary>
    public string EditorClass { get; set; } = (DefaultConfiguration?.EditorClass ?? "col-xs-12 col-sm-8 col-md-9");

    /// <summary>
    /// CSS Class fot the editor column without label column.
    /// </summary>
    public string EditorOffsetClass
    {
      get
      {
        return $" {LabelClass}"
          .Replace("-xs-", "-xs-offset-")
          .Replace("-sm-", "-sm-offset-")
          .Replace("-md-", "-md-offset-")
          .Replace("-lg-", "-lg-offset-")
          .Replace("col-xs-offset-12", string.Empty)
          .Replace("col-sm-offset-12", string.Empty)
          .Replace("col-md-offset-12", string.Empty)
          .Replace("col-lg-offset-12", string.Empty);
      }
    }

    /// <summary>
    /// Gets or sets the value specifying whether or not to override container properties.
    /// When this value is <see langword="true" />, the model properties override container properties with the
    /// same name.
    /// </summary>
    /// <remarks>
    /// If ViewBag.Title overrides model's Title property, set the
    /// <see cref="EditorTemplateConfiguration.OverrideContainerProperties"/> to true.
    /// </remarks>
    public bool OverrideContainerProperties { get; set; } = (DefaultConfiguration?.OverrideContainerProperties ?? true);

    /// <summary>
    /// Display checkbox label in Editor Templates.
    /// </summary>
    public bool SeparateCheckboxLabel { get; set; } = (DefaultConfiguration?.SeparateCheckboxLabel ?? false);

    /// <summary>
    /// Maximum depth of nested templates Html.DisplayForModel() can render.
    /// </summary>
    /// <remarks>For all properties that exceed MaximumTmeplateDepth, the SimpleText is rendered.</remarks>
    public int MaximumTemplateDepth { get; set; } = (DefaultConfiguration?.MaximumTemplateDepth ?? 1);
  }
}
