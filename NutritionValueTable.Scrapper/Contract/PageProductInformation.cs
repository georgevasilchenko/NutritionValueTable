using HtmlAgilityPack;
using System.Collections.Generic;

namespace NutritionValueTable.Scrapper
{
   internal class PageProductInformation
   {
      public List<LabelAttribute> LabelAttributes { get; internal set; }

      public List<UnitAttribute> UnitAttributes { get; internal set; }

      public List<ValueAttribute> ValueAttributes { get; internal set; }

      public HtmlAttribute ImageRelativeUri { get; internal set; }

      public PageProductInformation()
      {
         LabelAttributes = new List<LabelAttribute>();
         UnitAttributes = new List<UnitAttribute>();
         ValueAttributes = new List<ValueAttribute>();
      }
   }
}