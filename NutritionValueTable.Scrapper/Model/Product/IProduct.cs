using System.Collections.Generic;

namespace NutritionValueTable.Scrapper
{
   public interface IProduct
   {
      string ImageUri { get; }

      int UniqueId { get; }

      string Name { get; }

      IEnumerable<INutrient> Nutrients { get; }
   }
}