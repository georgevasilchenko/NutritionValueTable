using System.Collections.Generic;

namespace NutritionValueTable.Scrapper
{
   /// <summary>PageProductInformation class.</summary>
   internal class PageProductInformation
   {
      /// <summary>Gets or sets the label attributes.</summary>
      /// <value>The label attributes.</value>
      public List<LabelAttribute> LabelAttributes { get; internal set; }

      /// <summary>Gets or sets the unit attributes.</summary>
      /// <value>The unit attributes.</value>
      public List<UnitAttribute> UnitAttributes { get; internal set; }

      /// <summary>Gets or sets the value attributes.</summary>
      /// <value>The value attributes.</value>
      public List<ValueAttribute> ValueAttributes { get; internal set; }

      /// <summary>Gets or sets the image relative URI.</summary>
      /// <value>The image relative URI.</value>
      public string ImageRelativeUri { get; internal set; }

      /// <summary>Initializes a new instance of the <see cref="PageProductInformation" /> class.</summary>
      public PageProductInformation()
      {
         LabelAttributes = new List<LabelAttribute>();
         UnitAttributes = new List<UnitAttribute>();
         ValueAttributes = new List<ValueAttribute>();
      }
   }
}