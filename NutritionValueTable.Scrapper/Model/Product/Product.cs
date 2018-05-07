using NutritionValueTable.Contract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NutritionValueTable.Scrapper
{
   /// <summary>Product class.</summary>
   /// <seealso cref="NutritionValueTable.Contract.IProduct" />
   public class Product : IProduct
   {
      /// <summary>Gets the identifier.</summary>
      /// <value>The identifier.</value>
      [Key]
      public int Id { get; private set; }

      /// <summary>Gets the name.</summary>
      /// <value>The name.</value>
      public string Name { get; private set; }

      /// <summary>Gets the image URI.</summary>
      /// <value>The image URI.</value>
      public string ImageUri { get; private set; }

      /// <summary>Gets the unique identifier.</summary>
      /// <value>The unique identifier.</value>
      public int UniqueId { get; private set; }

      /// <summary>Gets the nutrients.</summary>
      /// <value>The nutrients.</value>
      public virtual List<Nutrient> Nutrients { get; private set; }

      /// <summary>Initializes a new instance of the <see cref="Product" /> class.</summary>
      /// <param name="name">The name.</param>
      /// <param name="imageUri">The image URI.</param>
      /// <param name="uniqueId">The unique identifier.</param>
      /// <param name="nutrients">The nutrients.</param>
      public Product(string name, string imageUri, int uniqueId, List<Nutrient> nutrients)
      {
         Name = name;
         ImageUri = imageUri;
         UniqueId = uniqueId;
         Nutrients = nutrients;
      }

      /// <summary>
      /// Prevents a default instance of the <see cref="Product" /> class from being created.
      /// </summary>
      private Product()
      {
      }

      #region IProduct

      /// <summary>Gets the name.</summary>
      /// <value>The name.</value>
      string IProduct.Name => Name;

      /// <summary>Gets the image URI.</summary>
      /// <value>The image URI.</value>
      string IProduct.ImageUri => ImageUri;

      /// <summary>Gets the unique identifier.</summary>
      /// <value>The unique identifier.</value>
      int IProduct.UniqueId => UniqueId;

      /// <summary>Gets the nutrients.</summary>
      /// <value>The nutrients.</value>
      IEnumerable<INutrient> IProduct.Nutrients => Nutrients;

      #endregion IProduct
   }
}