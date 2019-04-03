using System.Collections.Generic;

namespace Space_Fridge_Forum.Models.ViewModels
{
    public class EditFridgeViewModel : Fridge
    {
        public List<IngredientType> IngredientTypes { get; set; }
        public List<IngredientUnit> IngredientUnits { get; set; }
    }

   
}