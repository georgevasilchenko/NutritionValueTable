using System;
using System.Text.RegularExpressions;

namespace NutritionValueTable.Scrapper
{
   /// <summary>PageProductItem class.</summary>
   public class PageProductItem
   {
      /// <summary>Gets the identifier.</summary>
      /// <value>The identifier.</value>
      public int Id { get; }

      /// <summary>Gets the name.</summary>
      /// <value>The name.</value>
      public string Name { get; }

      /// <summary>Gets the relative URI.</summary>
      /// <value>The relative URI.</value>
      public string RelativeUri { get; }

      /// <summary>Initializes a new instance of the <see cref="PageProductItem" /> class.</summary>
      /// <param name="name">The name.</param>
      /// <param name="uri">The URI.</param>
      public PageProductItem(string name, string uri)
      {
         Name = name;
         RelativeUri = uri;

         var idText = Regex.Match(RelativeUri, @"(?!(id=))\d{1,5}");
         Id = Convert.ToInt32(idText.Value);
      }
   }
}