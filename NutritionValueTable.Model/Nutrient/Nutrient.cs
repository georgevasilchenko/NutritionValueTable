namespace NutritionValueTable.Model
{
   public class Nutrient : INutrient
   {
      public string Name { get; }

      public Unit Unit { get; }

      public double Value { get; }

      public Nutrient(string name, Unit unit, double value)
      {
         Name = name;
         Unit = unit;
         Value = value;
      }
   }
}