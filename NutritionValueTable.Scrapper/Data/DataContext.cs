using System.Data.Entity;

namespace NutritionValueTable.Scrapper
{
   public class DataContext : DbContext
   {
      public DataContext(string nameOrConnectionString) : base(nameOrConnectionString)
      {
      }
   }
}