namespace NutritionValueTable.Scrapper
{
   public class LabelAttribute
   {
      public string Title { get; }

      public string AlternativeText { get; }

      public LabelAttribute(string title, string alternativeText)
      {
         Title = title;
         AlternativeText = alternativeText;
      }
   }
}