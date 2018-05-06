using System.Collections.Generic;

namespace NutritionValueTable.Scrapper
{
   public class Product : IProduct
   {
      public string Name { get; }

      public string ImageUri { get; }

      public int UniqueId { get; }

      public List<Nutrient> Nutrients { get; }

      public Product(string name, List<Nutrient> nutrients)
      {
         Name = name;
         Nutrients = nutrients;
      }

      #region IProduct

      string IProduct.Name => Name;

      string IProduct.ImageUri => ImageUri;

      int IProduct.UniqueId => UniqueId;

      IEnumerable<INutrient> IProduct.Nutrients => Nutrients;

      #endregion IProduct
   }
}