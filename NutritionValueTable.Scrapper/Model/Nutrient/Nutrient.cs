using NutritionValueTable.Contract;
using System.ComponentModel.DataAnnotations;

namespace NutritionValueTable.Scrapper
{
   /// <summary>Nutrient class.</summary>
   /// <seealso cref="NutritionValueTable.Contract.INutrient" />
   public class Nutrient : INutrient
   {
      /// <summary>Gets the identifier.</summary>
      /// <value>The identifier.</value>
      [Key]
      public int Id { get; private set; }

      /// <summary>Gets the name.</summary>
      /// <value>The name.</value>
      public string Name { get; private set; }

      /// <summary>Gets the unit.</summary>
      /// <value>The unit.</value>
      public Unit Unit { get; private set; }

      /// <summary>Gets the value.</summary>
      /// <value>The value.</value>
      public double Value { get; private set; }

      /// <summary>Initializes a new instance of the <see cref="Nutrient" /> class.</summary>
      /// <param name="name">The name.</param>
      /// <param name="unit">The unit.</param>
      /// <param name="value">The value.</param>
      public Nutrient(string name, Unit unit, double value)
      {
         Name = name;
         Unit = unit;
         Value = value;
      }

      /// <summary>
      /// Prevents a default instance of the <see cref="Nutrient" /> class from being created.
      /// </summary>
      private Nutrient()
      {
      }
   }
}