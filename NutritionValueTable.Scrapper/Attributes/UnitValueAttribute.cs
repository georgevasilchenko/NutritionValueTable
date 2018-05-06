using System;

namespace NutritionValueTable.Scrapper
{
   public class UnitValueAttribute : Attribute
   {
      public string ShortName { get; }

      public UnitValueAttribute(string shortName)
      {
         ShortName = shortName;
      }
   }
}