using System;
using System.Text.RegularExpressions;

namespace NutritionValueTable.Scrapper
{
   public class PageProductItem
   {
      public int Id { get; }

      public string Name { get; }

      public string RelativeUri { get; }

      public PageProductItem(string name, string uri)
      {
         Name = name;
         RelativeUri = uri;

         var idText = Regex.Match(RelativeUri, @"(?!(id=))\d{1,5}");
         Id = Convert.ToInt32(idText.Value);
      }
   }
}