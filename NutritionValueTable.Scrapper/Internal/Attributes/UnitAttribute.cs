namespace NutritionValueTable.Scrapper
{
   public class UnitAttribute
   {
      public string Title { get; }

      public string Unit { get; }

      public UnitAttribute(string title, string unit)
      {
         Title = title;
         Unit = unit;
      }
   }
}