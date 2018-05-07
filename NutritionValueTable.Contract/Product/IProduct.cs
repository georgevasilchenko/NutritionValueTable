using System.Collections.Generic;

namespace NutritionValueTable.Contract
{
   /// <summary>IProduct interface.</summary>
   public interface IProduct
   {
      /// <summary>Gets the identifier.</summary>
      /// <value>The identifier.</value>
      int Id { get; }

      /// <summary>Gets the image URI.</summary>
      /// <value>The image URI.</value>
      string ImageUri { get; }

      /// <summary>Gets the unique identifier.</summary>
      /// <value>The unique identifier.</value>
      int UniqueId { get; }

      /// <summary>Gets the name.</summary>
      /// <value>The name.</value>
      string Name { get; }

      /// <summary>Gets the nutrients.</summary>
      /// <value>The nutrients.</value>
      IEnumerable<INutrient> Nutrients { get; }
   }
}