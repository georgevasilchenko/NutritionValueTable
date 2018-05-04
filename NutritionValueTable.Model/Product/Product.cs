using System.Collections.Generic;

namespace NutritionValueTable.Model
{
   public class Product : IProduct
   {
      public string Name { get; }

      public List<Nutrient> Nutrients { get; }

      string IProduct.Name => Name;

      IEnumerable<INutrient> IProduct.Nutrients => Nutrients;

      public Product(string name, List<Nutrient> nutrients)
      {
         Name = name;
         Nutrients = nutrients;
      }
   }
}