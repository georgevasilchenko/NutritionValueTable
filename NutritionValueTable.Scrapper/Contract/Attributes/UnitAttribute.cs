namespace NutritionValueTable.Scrapper
{
   /// <summary>UnitAttribute class.</summary>
   public class UnitAttribute
   {
      /// <summary>Gets the title.</summary>
      /// <value>The title.</value>
      public string Title { get; }

      /// <summary>Gets the unit.</summary>
      /// <value>The unit.</value>
      public string Unit { get; }

      /// <summary>Initializes a new instance of the <see cref="UnitAttribute" /> class.</summary>
      /// <param name="title">The title.</param>
      /// <param name="unit">The unit.</param>
      public UnitAttribute(string title, string unit)
      {
         Title = title;
         Unit = unit;
      }
   }
}