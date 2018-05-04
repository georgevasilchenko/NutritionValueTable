using System.Collections.Generic;

namespace NutritionValueTable.Scrapper
{
   internal class PageProductInformation
   {
      public List<LabelAttribute> LabelAttributes { get; set; }

      public List<UnitAttribute> UnitAttributes { get; set; }

      public List<ValueAttribute> ValueAttributes { get; set; }

      public PageProductInformation()
      {
         LabelAttributes = new List<LabelAttribute>();
         UnitAttributes = new List<UnitAttribute>();
         ValueAttributes = new List<ValueAttribute>();
      }
   }
}