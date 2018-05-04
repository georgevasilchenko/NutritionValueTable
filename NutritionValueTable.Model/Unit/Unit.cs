using NutritionValueTable.Common;

namespace NutritionValueTable.Model
{
   public enum Unit
   {
      [UnitValue("kcal")]
      KiloCalorie = 0,

      [UnitValue("kJ")]
      KiloJoule = 1,

      [UnitValue("g")]
      Gram = 2,

      [UnitValue("mg")]
      MilliGram = 3,

      [UnitValue("µg")]
      MicroGram = 4
   }
}