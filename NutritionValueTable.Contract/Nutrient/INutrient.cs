namespace NutritionValueTable.Contract
{
   /// <summary>INutrient interface.</summary>
   public interface INutrient
   {
      /// <summary>Gets the identifier.</summary>
      /// <value>The identifier.</value>
      int Id { get; }

      /// <summary>Gets the name.</summary>
      /// <value>The name.</value>
      string Name { get; }

      /// <summary>Gets the unit.</summary>
      /// <value>The unit.</value>
      Unit Unit { get; }

      /// <summary>Gets the value.</summary>
      /// <value>The value.</value>
      double Value { get; }
   }
}