using HtmlAgilityPack;
using NutritionValueTable.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NutritionValueTable.Scrapper
{
   /// <summary>Program class.</summary>
   internal class Program
   {
      /// <summary>The base URI.</summary>
      public static string BaseUri = @"https://www.voedingswaardetabel.nl/";

      /// <summary>The products list URI.</summary>
      public static string ProductsListUri = @"https://www.voedingswaardetabel.nl/voedingswaarde/";

      /// <summary>Defines the entry point of the application.</summary>
      /// <param name="args">The arguments.</param>
      private static void Main(string[] args)
      {
         using (var httpClient = new HttpClient())
         {
            using (var dbContext = new DataContext(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
            {
               for (byte i = 65; i < 66; i++) //91
               {
                  var character = Encoding.ASCII.GetString(new byte[] { i });

                  var pageText = GetHtmlPageText(httpClient, ProductsListUri + character).GetAwaiter().GetResult();
                  var pageProductItems = GetPageProductItemsFromHtml("cphMain_ltvNutrition_hplProdname", pageText);

                  foreach (var item in pageProductItems)
                  {
                     var productPageText = GetHtmlPageText(httpClient, BaseUri + item.RelativeUri)
                        .GetAwaiter()
                        .GetResult();
                     var product = GetProductFromHtml(item.Name, productPageText, item.Id);

                     if (product == null)
                     {
                        continue;
                     }

                     dbContext.Products.Add(product);

                     Console.WriteLine("Added item with name: " + product.Name);
                  }

                  Console.ForegroundColor = ConsoleColor.Green;
                  Console.WriteLine("Added all items for character: " + character);
                  Console.ResetColor();
               }

               dbContext.SaveChanges();
            }
         }
      }

      /// <summary>Gets the HTML page text.</summary>
      /// <param name="httpClient">The HTTP client.</param>
      /// <param name="uri">The URI.</param>
      /// <returns>A <see cref="Task{string}" /> reference.</returns>
      private static async Task<string> GetHtmlPageText(HttpClient httpClient, string uri) => await httpClient.GetStringAsync(uri);

      /// <summary>Gets the page product items from HTML.</summary>
      /// <param name="productItemId">The product item identifier.</param>
      /// <param name="htmlText">The HTML text.</param>
      /// <returns>A <see cref="IEnumerable{PageProductItem}" /> reference.</returns>
      private static IEnumerable<PageProductItem> GetPageProductItemsFromHtml(string productItemId, string htmlText)
      {
         var document = new HtmlDocument();

         document.LoadHtml(htmlText);

         return document.DocumentNode.Descendants("a")
               .Where(o => o.Attributes.Contains("id") && o.Attributes["id"].Value.Contains(productItemId))
               .Select(o => new PageProductItem(o.InnerHtml, o.Attributes["href"].Value));
      }

      /// <summary>Gets the product from HTML.</summary>
      /// <param name="productName">Name of the product.</param>
      /// <param name="htmlText">The HTML text.</param>
      /// <param name="productItemId">The product item identifier.</param>
      /// <returns>A <see cref="Product" /> reference.</returns>
      /// <exception cref="System.Exception">Length of attribute collections is not equal!</exception>
      private static Product GetProductFromHtml(string productName, string htmlText, int productItemId)
      {
         var document = new HtmlDocument();
         document.LoadHtml(htmlText);

         var divs = document.DocumentNode.Descendants("div");
         var imgs = document.DocumentNode.Descendants("img");

         var productInformation = new PageProductInformation();

         // Product image
         var imageElement = imgs.SingleOrDefault(o =>
         o.Attributes.Contains("id") &&
         o.Attributes.Contains("src") &&
         o.Attributes["id"].Value == "cphMain_imgProd");
         if (imageElement != null)
         {
            productInformation.ImageRelativeUri = imageElement.Attributes["src"].Value;
         }
         else
         {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Product: " + productName + " is invalid!");
            Console.ResetColor();
            return null;
         }

         // Values elements
         var valueElements = divs.Where(o => o.Attributes.Contains("id") && o.Attributes["id"].Value.Contains("cphMain_pnl"));
         var generalValues = valueElements
            .Where(o => o.Attributes.Contains("class") && o.Attributes["class"].Value == "vwtItemvw")
            .Select(o => o.ChildNodes.First(n => n.Name == "span"))
            .Select(o => new ValueAttribute(o.InnerHtml, o.Id));
         var vitaminesValues = valueElements
            .Where(o => o.Attributes.Contains("class") && o.Attributes["class"].Value == "vwtItemvm")
            .Select(o => o.ChildNodes.First(n => n.Name == "span"))
            .Select(o => new ValueAttribute(o.InnerHtml, o.Id));
         var mineralsValues = valueElements
            .Where(o => o.Attributes.Contains("class") && o.Attributes["class"].Value == "vwtItemmn")
            .Select(o => o.ChildNodes.First(n => n.Name == "span"))
            .Select(o => new ValueAttribute(o.InnerHtml, o.Id));

         productInformation.ValueAttributes.AddRange(generalValues);
         productInformation.ValueAttributes.AddRange(vitaminesValues);
         productInformation.ValueAttributes.AddRange(mineralsValues);

         // Title elements
         var generalTitleElements = imgs
            .Where(o => o.Attributes.Contains("id") && o.Attributes["id"].Value.Contains("cphMain_imgunitWvw"))
            .Select(o => new LabelAttribute(o.Attributes["title"].Value, o.Attributes["alt"].Value));

         var vitaminesTitleElements = imgs
            .Where(o => o.Attributes.Contains("id") && o.Attributes["id"].Value.Contains("cphMain_imgunitWvm"))
            .Select(o => new LabelAttribute(o.Attributes["title"].Value, o.Attributes["alt"].Value));

         var mineralsTitleElements = imgs
            .Where(o => o.Attributes.Contains("id") && o.Attributes["id"].Value.Contains("cphMain_imgunitWmn"))
            .Select(o => new LabelAttribute(o.Attributes["title"].Value, o.Attributes["alt"].Value));

         productInformation.LabelAttributes.AddRange(generalTitleElements);
         productInformation.LabelAttributes.AddRange(vitaminesTitleElements);
         productInformation.LabelAttributes.AddRange(mineralsTitleElements);

         // Units elements
         var generalUnitsElements = divs
            .Where(o => o.Attributes.Contains("id") && o.Attributes["id"].Value.Contains("cphMain_unitvw"))
            .Select(o => o.ChildNodes.First(n => n.Name == "span"))
            .Select(o => new UnitAttribute(o.Attributes["title"]?.Value, o.InnerHtml))
            .Where(o => o.Title != null);

         var vitaminesUnitsElements = divs
            .Where(o => o.Attributes.Contains("id") && o.Attributes["id"].Value.Contains("cphMain_unitvm"))
            .Select(o => o.ChildNodes.First(n => n.Name == "span"))
            .Select(o => new UnitAttribute(o.Attributes["title"]?.Value, o.InnerHtml))
            .Where(o => o.Title != null);

         var mineralsUnitsElements = divs
            .Where(o => o.Attributes.Contains("id") && o.Attributes["id"].Value.Contains("cphMain_unitmn"))
            .Select(o => o.ChildNodes.First(n => n.Name == "span"))
            .Select(o => new UnitAttribute(o.Attributes["title"]?.Value, o.InnerHtml))
            .Where(o => o.Title != null);

         productInformation.UnitAttributes.AddRange(generalUnitsElements);
         productInformation.UnitAttributes.AddRange(vitaminesUnitsElements);
         productInformation.UnitAttributes.AddRange(mineralsUnitsElements);

         if (productInformation.LabelAttributes.Count != productInformation.UnitAttributes.Count ||
            productInformation.LabelAttributes.Count != productInformation.ValueAttributes.Count)
         {
            throw new Exception("Length of attribute collections is not equal!");
         }

         var nutrients = new List<Nutrient>();

         for (var i = 0; i < productInformation.LabelAttributes.Count; i++)
         {
            var name = productInformation.LabelAttributes[i].Title;
            var unit = GetUnit(productInformation.UnitAttributes[i].Unit);
            var value = 0.0;

            if (double.TryParse(productInformation.ValueAttributes[i].Value.Replace(',', '.'), out var tempValue))
            {
               value = tempValue;
            }

            nutrients.Add(new Nutrient(name, unit, value));
         }

         return new Product(productName, BaseUri.TrimEnd('/') + productInformation.ImageRelativeUri, productItemId, nutrients);
      }

      /// <summary>Gets the unit.</summary>
      /// <param name="title">The title.</param>
      /// <returns>A <see cref="Unit" /> value.</returns>
      /// <exception cref="System.ArgumentException">Unknown nutrient abbreviation was parsed.</exception>
      private static Unit GetUnit(string title)
      {
         switch (title)
         {
            case "kcal":
               return Unit.KiloCalorie;

            case "kJ":
               return Unit.KiloJoule;

            case "g":
               return Unit.Gram;

            case "mg":
               return Unit.MilliGram;

            case "µg":
               return Unit.MicroGram;

            default:
               throw new ArgumentException("Unknown nutrient abbreviation was parsed.");
         }
      }
   }
}