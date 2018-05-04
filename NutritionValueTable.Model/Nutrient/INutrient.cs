namespace NutritionValueTable.Model
{
   public interface INutrient
   {
      string Name { get; }

      Unit Unit { get; }

      double Value { get; }
   }
}