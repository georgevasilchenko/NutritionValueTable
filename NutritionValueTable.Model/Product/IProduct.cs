using System.Collections.Generic;

namespace NutritionValueTable.Model
{
   public interface IProduct
   {
      string Name { get; }

      IEnumerable<INutrient> Nutrients { get; }
   }
}