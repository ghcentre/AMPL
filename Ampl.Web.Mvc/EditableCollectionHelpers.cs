using Ampl.Core;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Ampl.Web.Mvc
{
    /// <summary>
    /// Provides a set of <see langword="static"/> helper methods for EditableCollection support.
    /// </summary>
    public static class EditableCollectionHelpers
  {
    /// <summary>
    /// Initializes a new instance of the collection item by calling the method or property getter specified in the
    /// model's collection <see cref="EditableCollectionAttribute"/>.
    /// </summary>
    /// <param name="modelMetadata">The metadata for the model which is parent for the collection.</param>
    /// <param name="itemType">The type of items in the collection.</param>
    /// <returns>The newly created instance of the collection item.</returns>
    public static object CreateCollectionItem(ModelMetadata modelMetadata, Type itemType)
    {
      Check.NotNull(modelMetadata, nameof(modelMetadata));

      var containerType = modelMetadata.ContainerType;
      var editableCollectionAttribute = (EditableCollectionAttribute)containerType
                                            .GetProperty(modelMetadata.PropertyName)
                                            .GetCustomAttributes(typeof(EditableCollectionAttribute), false)
                                            .FirstOrDefault();
      if(editableCollectionAttribute == null)
      {
        return Activator.CreateInstance(itemType);
      }

      string methodOrPropertyName = editableCollectionAttribute.ItemFactory.ToNullIfWhiteSpace();
      if(methodOrPropertyName == null)
      {
        return Activator.CreateInstance(itemType);
      }

      var container = modelMetadata.Container;
      object result = null;

      var methodInfo = containerType.GetMethod(methodOrPropertyName);
      if(methodInfo != null)
      {
        if(!methodInfo.IsStatic && container == null)
        {
          return Activator.CreateInstance(itemType);
        }
        result = methodInfo.Invoke(container, new object[] { });
      }
      else
      {
        var propertyInfo = containerType.GetProperty(methodOrPropertyName);
        if(propertyInfo != null)
        {
          var getMethodInfo = propertyInfo.GetGetMethod();
          if(getMethodInfo != null)
          {
            if(!getMethodInfo.IsStatic && container == null)
            {
              return Activator.CreateInstance(itemType);
            }
            result = propertyInfo.GetValue(container);
          }
        }
      }

      return result;
    }
  }
}
