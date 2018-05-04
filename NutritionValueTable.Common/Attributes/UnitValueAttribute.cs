using System;

namespace NutritionValueTable.Common
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