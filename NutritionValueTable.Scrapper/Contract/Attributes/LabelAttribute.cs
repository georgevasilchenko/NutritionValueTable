namespace NutritionValueTable.Scrapper
{
   /// <summary>LabelAttribute class.</summary>
   public class LabelAttribute
   {
      /// <summary>Gets the title.</summary>
      /// <value>The title.</value>
      public string Title { get; }

      /// <summary>Gets the alternative text.</summary>
      /// <value>The alternative text.</value>
      public string AlternativeText { get; }

      /// <summary>Initializes a new instance of the <see cref="LabelAttribute" /> class.</summary>
      /// <param name="title">The title.</param>
      /// <param name="alternativeText">The alternative text.</param>
      public LabelAttribute(string title, string alternativeText)
      {
         Title = title;
         AlternativeText = alternativeText;
      }
   }
}