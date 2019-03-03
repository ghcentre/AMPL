using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ampl.Web.Mvc
{
  /// <summary>
  /// Helpers for the Display and Editor Templates.
  /// </summary>
  public static class EditorTemplateHelpers
  {
    /// <summary>
    /// Determines whether to show complex object property in read-only views
    /// </summary>
    /// <param name="modelMetadata">Model metadata.</param>
    /// <param name="templateInfo">Template info.</param>
    /// <returns></returns>
    public static bool ShouldDisplayProperty(ModelMetadata modelMetadata, TemplateInfo templateInfo)
    {
      if(modelMetadata == null || templateInfo == null)
      {
        return false;
      }

      return modelMetadata.ShowForDisplay &&
             //metadata.ModelType != typeof(System.Data.Entity.EntityState) &&
             //!metadata.IsComplexType &&
             !templateInfo.Visited(modelMetadata);
    }

    /// <summary>
    /// Determines whether to show complex object property in editor views.
    /// </summary>
    /// <param name="modelMetadata">Model metadata.</param>
    /// <param name="templateInfo">Template info.</param>
    /// <returns></returns>
    public static bool ShouldEditProperty(ModelMetadata modelMetadata, TemplateInfo templateInfo)
    {
      if(modelMetadata == null || templateInfo == null)
      {
        return false;
      }

      return modelMetadata.ShowForEdit &&
             //metadata.ModelType != typeof(System.Data.Entity.EntityState) &&
             //!metadata.IsComplexType &&
             !templateInfo.Visited(modelMetadata);
    }

    public static void OverrideContainerProperties(
      bool readOnlyView,
      ViewDataDictionary containerViewData,
      ModelMetadata modelMetadata,
      TemplateInfo templateInfo,
      EditorTemplateConfiguration configuration = null)
    {
      if(containerViewData == null || modelMetadata == null)
      {
        return;
      }

      configuration = configuration ?? EditorTemplateConfiguration.DefaultConfiguration;
      if(!configuration.OverrideContainerProperties)
      {
        return;
      }

      foreach(var propertyMetadata in modelMetadata.Properties
                                                   .Where(pm => readOnlyView
                                                                  ? ShouldDisplayProperty(pm, templateInfo)
                                                                  : ShouldEditProperty(pm, templateInfo))
             )
      {
        string propertyName = propertyMetadata.PropertyName;
        if(containerViewData.Keys.Contains(propertyName))
        {
          containerViewData.Remove(propertyName);
        }
      }
    }
  }
}
