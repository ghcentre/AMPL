using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ampl.System;

namespace Ampl.Web.Mvc.Html
{
  /// <summary>
  /// Provides a set of <see langword="static" /> methods for the drop down list.
  /// </summary>
  public static class HtmlHelperDropDownListExtensions
  {
    /// <summary>
    /// Gets the sequence of <see cref="SelectListItem"/> for the drop down list.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <param name="htmlHelper">The Html helper.</param>
    /// <returns>
    /// The sequence of the <see cref="SelectListItem"/>.
    /// On error, the method returns an empty sequence.
    /// </returns>
    public static IEnumerable<SelectListItem> GetDropDownListItems<TModel>(this HtmlHelper<TModel> htmlHelper)
    {
      Check.NotNull(htmlHelper, nameof(htmlHelper));

      var containerType = htmlHelper.ViewData.ModelMetadata.ContainerType;
      var container = htmlHelper.ViewData.ModelMetadata.Container;
      var dropDownListAttribute = containerType.GetProperty(htmlHelper.ViewData.ModelMetadata.PropertyName)
                                               .GetCustomAttributes(typeof(DropDownListAttribute), false)
                                               .Cast<DropDownListAttribute>()
                                               .FirstOrDefault();
      if(dropDownListAttribute == null)
      {
        return Enumerable.Empty<SelectListItem>();
      }

      IEnumerable<SelectListItem> items = null;

      Type enumType = dropDownListAttribute.EnumType;
      if(enumType != null && typeof(Enum).IsAssignableFrom(enumType))
      {
        items = GetEnumerableItems(enumType);
      }
      else
      {
        items = GetItemsFromMethodOrPropertyName(dropDownListAttribute, htmlHelper, containerType, container);
      }

      var itemList = (items == null) ? new List<SelectListItem>() : items.ToList();
      bool hasDefault = !htmlHelper.ViewData.ModelMetadata.IsRequired ||
                        container == null;
      if(hasDefault)
      {
        itemList.Insert(0, new SelectListItem() {
          Value = string.Empty,
          Text = htmlHelper.ViewData.ModelMetadata.NullDisplayText ?? Messages.DropDownListDefaultNullDisplayText
        });
      }

      foreach(var item in itemList)
      {
        item.Selected = false;
      }

      string modelValue = htmlHelper.ViewData.Model == null ? string.Empty : htmlHelper.ViewData.Model.ToString();
      foreach(var item in itemList)
      {
        if(item.Value == modelValue)
        {
          item.Selected = true;
          break;
        }
      }

      return itemList;
    }

    private static IEnumerable<SelectListItem> GetItemsFromMethodOrPropertyName<TModel>(
                                                    DropDownListAttribute dropDownListAttribute,
                                                    HtmlHelper<TModel> htmlHelper,
                                                    Type containerType,
                                                    object container
                                               )
    {
      string methodOrPropertyName = dropDownListAttribute.ItemContainer.ToNullIfWhiteSpace();
      if(methodOrPropertyName == null)
      {
        return Enumerable.Empty<SelectListItem>();
      }

      var methodInfo = containerType.GetMethod(methodOrPropertyName);
      if(methodInfo != null)
      {
        if(!methodInfo.IsStatic && container == null)
        {
          return Enumerable.Empty<SelectListItem>();
        }
        var items = (IEnumerable<SelectListItem>)methodInfo.Invoke(container, new object[] { });
        return items;
      }

      var propertyInfo = containerType.GetProperty(methodOrPropertyName);
      if(propertyInfo == null)
      {
        return Enumerable.Empty<SelectListItem>();
      }

      var getMethodInfo = propertyInfo.GetGetMethod();
      if(getMethodInfo != null)
      {
        if(!getMethodInfo.IsStatic && container == null)
        {
          return Enumerable.Empty<SelectListItem>();
        }
        var items = (IEnumerable<SelectListItem>)propertyInfo.GetValue(container);
        return items;
      }

      return Enumerable.Empty<SelectListItem>();
    }

    private static IEnumerable<SelectListItem> GetEnumerableItems(Type enumType)
    {
      var values = Enum.GetValues(enumType);
      foreach(var value in values)
      {
        var enumValue = (Enum)value;
        var item = new SelectListItem() {
          Value = enumValue.ToString(),
          Text = enumValue.GetDisplayName() ?? enumValue.ToString()
        };
        yield return item;
      }
    }
  }
}
