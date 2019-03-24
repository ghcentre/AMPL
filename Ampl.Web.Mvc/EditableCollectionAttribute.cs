using System;
using System.Web.Mvc;

namespace Ampl.Web.Mvc
{
    /// <summary>
    /// Renders the model property as editable collection with add/remove item support.
    /// </summary>
    /// <example>
    /// <code>
    ///   public class SampleCollectionItemViewModel
    ///   {
    ///     public int IntegerProperty { get; set; }
    ///     public string StringProperty { get; set; }
    ///   }
    ///   
    ///   public class SampleViewModel
    ///   {
    ///     [EditableCollection(ItemFactory = "CreateItem")]
    ///     public IEnumerable&lt;SampleCollectionItemViewModel&gt; SampleCollection { get; set; }
    ///     
    ///     //
    ///     // This example uses method to initialize a new collection item.
    ///     //
    ///     public static SampleCollectionItemViewModel CreateItem()
    ///     {
    ///       return new SampleCollectionItemViewModel() {
    ///         StringProperty = "Some Value"
    ///       };
    ///     }
    ///   }
    /// </code>
    /// <remarks>
    /// <para>The collection must not be <see langword="null"/> to render properly.</para>
    /// </remarks>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EditableCollectionAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableCollectionAttribute"/> class.
        /// </summary>
        public EditableCollectionAttribute()
        { }

        /// <summary>
        /// Gets the model's property or method name
        /// which is used to initialize a new instance of the collection item.
        /// </summary>
        /// <remarks>The method must not have parameters
        /// and must return the instance of the type of the items in collection.</remarks>
        /// <remarks>If the value is <see langword="null"/> the
        /// <c>Activator.CreateInstance(typeof(TypeInCollection))</c> is used
        /// to inialize a new instance of the collection item.</remarks>
        /// <value>The name of the property or method.</value>
        public string ItemFactory { get; set; }

        /// <summary>
        /// Provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if(metadata.TemplateHint == null)
            {
                metadata.TemplateHint = "EditableCollection";
            }
        }
    }
}
