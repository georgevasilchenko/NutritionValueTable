using HtmlAgilityPack;
using NutritionValueTable.Common;
using NutritionValueTable.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NutritionValueTable.Scrapper
{
   internal class Program
   {
      public static string BaseUri = @"https://www.voedingswaardetabel.nl/";
      public static string ProductsListUri = @"https://www.voedingswaardetabel.nl/voedingswaarde/";

      public static HttpClient HttpClient;
      public static List<Product> Products = new List<Product>();

      public static string ProductsTable;

      private static void Main(string[] args)
      {
         HttpClient = new HttpClient();

         for (byte i = 65; i < 66; i++) //91
         {
            var character = Encoding.ASCII.GetString(new byte[] { i });

            var pageText = GetHtmlPageText(HttpClient, ProductsListUri + character).GetAwaiter().GetResult();
            var pageProductItems = GetPageProductItemsFromHtml("cphMain_ltvNutrition_hplProdname", pageText);

            foreach (var item in pageProductItems)
            {
               var productPageText = GetHtmlPageText(HttpClient, BaseUri + item.RelativeUri)
                  .GetAwaiter()
                  .GetResult();
               var product = GetProductFromHtml(item.Name, productPageText);
               Products.Add(product);

               Console.WriteLine("Added: " + product.Name);
            }

            Console.WriteLine("Ready for: " + character);
         }

         ProductsTable = string.Join("\r\n", Products.Select(o =>
         {
            var values = o.Name + ";";
            values += string.Join(";", o.Nutrients.Select(n => $"{n.Name},{n.Unit},{n.Value}"));
            return values;
         }));

         var exportPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\")) + "sample.txt";

         using (var stream = File.CreateText(exportPath))
         {
            stream.Write(ProductsTable);
         }
      }

      private static async Task<string> GetHtmlPageText(HttpClient httpClient, string uri) => await httpClient.GetStringAsync(uri);

      private static IEnumerable<PageProductItem> GetPageProductItemsFromHtml(string productItemId, string htmlText)
      {
         var document = new HtmlDocument();

         document.LoadHtml(htmlText);

         return document.DocumentNode.Descendants("a")
               .Where(o => o.Attributes.Contains("id") && o.Attributes["id"].Value.Contains(productItemId))
               .Select(o => new PageProductItem(o.InnerHtml, o.Attributes["href"].Value));
      }

      private static Product GetProductFromHtml(string productName, string htmlText)
      {
         var document = new HtmlDocument();
         document.LoadHtml(htmlText);

         var divs = document.DocumentNode.Descendants("div");
         var imgs = document.DocumentNode.Descendants("img");

         var productInformation = new PageProductInformation();

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

         return new Product(productName, nutrients);
      }

      private static Unit GetUnit(string title)
      {
         var type = typeof(Unit);
         var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

         var targetField = fields.Single(o => ((UnitValueAttribute)o.GetCustomAttribute(typeof(UnitValueAttribute))).ShortName == title);
         var value = (Unit)Enum.Parse(type, targetField.Name);

         return value;
      }
   }
}