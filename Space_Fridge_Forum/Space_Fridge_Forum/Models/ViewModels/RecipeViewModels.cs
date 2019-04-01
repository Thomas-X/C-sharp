using System.Collections.Generic;

namespace Space_Fridge_Forum.Models.ViewModels
{
    public class CreateRecipeViewModel : Recipe
    {
        public List<IngredientType> IngredientTypes { get; set; }
        public List<IngredientUnit> IngredientUnits { get; set; }
    }
}