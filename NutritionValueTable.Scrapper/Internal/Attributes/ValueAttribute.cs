namespace NutritionValueTable.Scrapper
{
   public class ValueAttribute
   {
      public string Value { get; }

      public string AttributeShortName { get; }

      public ValueAttribute(string value, string id)
      {
         Value = value;

         AttributeShortName = id.Replace("cphMain_lbl", "");
         AttributeShortName = AttributeShortName.Replace("lbl", "");
      }
   }
}