using System.Data.Entity;

namespace NutritionValueTable.Scrapper
{
   /// <summary>DataContext class.</summary>
   /// <seealso cref="System.Data.Entity.DbContext" />
   public class DataContext : DbContext
   {
      /// <summary>Gets or sets the products.</summary>
      /// <value>The products.</value>
      public DbSet<Product> Products { get; set; }

      /// <summary>Gets or sets the nutrients.</summary>
      /// <value>The nutrients.</value>
      public DbSet<Nutrient> Nutrients { get; set; }

      /// <summary>Initializes a new instance of the <see cref="DataContext" /> class.</summary>
      /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
      public DataContext(string nameOrConnectionString) : base(nameOrConnectionString)
      {
      }
   }
}