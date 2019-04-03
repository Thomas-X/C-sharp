using System.Collections.Generic;

namespace Space_Fridge_Forum.Models.ViewModels
{
    public class CreateRecipeViewModel : Recipe
    {
        public List<IngredientType> IngredientTypes { get; set; }
        public List<IngredientUnit> IngredientUnits { get; set; }
    }

    public class IndexRecipeViewModel
    {
        public List<IngredientType> IngredientTypes { get; set; }

        public List<IngredientUnit> IngredientUnits { get; set; }

        // For filtering recipes based on the fridge's ingredients
        public List<Ingredient> FridgeIngredients { get; set; }
        // For filtering recipes based on the current ingredients
        public List<Recipe> Recipes { get; set; }

        public ApplicationUser User { get; set; }
    }
}