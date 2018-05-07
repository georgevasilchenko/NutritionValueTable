namespace NutritionValueTable.Scrapper
{
   /// <summary>ValueAttribute class.</summary>
   public class ValueAttribute
   {
      /// <summary>Gets the value.</summary>
      /// <value>The value.</value>
      public string Value { get; }

      /// <summary>Gets the short name of the attribute.</summary>
      /// <value>The short name of the attribute.</value>
      public string AttributeShortName { get; }

      /// <summary>Initializes a new instance of the <see cref="ValueAttribute" /> class.</summary>
      /// <param name="value">The value.</param>
      /// <param name="id">The identifier.</param>
      public ValueAttribute(string value, string id)
      {
         Value = value;

         AttributeShortName = id.Replace("cphMain_lbl", "");
         AttributeShortName = AttributeShortName.Replace("lbl", "");
      }
   }
}