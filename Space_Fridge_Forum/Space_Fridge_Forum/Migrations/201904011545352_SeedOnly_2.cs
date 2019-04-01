namespace Space_Fridge_Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedOnly_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IngredientTypes", "CreateRecipeViewModel_Id", c => c.Int());
            AddColumn("dbo.IngredientUnits", "CreateRecipeViewModel_Id", c => c.Int());
            AddColumn("dbo.Recipes", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.IngredientTypes", "CreateRecipeViewModel_Id");
            CreateIndex("dbo.IngredientUnits", "CreateRecipeViewModel_Id");
            AddForeignKey("dbo.IngredientTypes", "CreateRecipeViewModel_Id", "dbo.Recipes", "Id");
            AddForeignKey("dbo.IngredientUnits", "CreateRecipeViewModel_Id", "dbo.Recipes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IngredientUnits", "CreateRecipeViewModel_Id", "dbo.Recipes");
            DropForeignKey("dbo.IngredientTypes", "CreateRecipeViewModel_Id", "dbo.Recipes");
            DropIndex("dbo.IngredientUnits", new[] { "CreateRecipeViewModel_Id" });
            DropIndex("dbo.IngredientTypes", new[] { "CreateRecipeViewModel_Id" });
            DropColumn("dbo.Recipes", "Discriminator");
            DropColumn("dbo.IngredientUnits", "CreateRecipeViewModel_Id");
            DropColumn("dbo.IngredientTypes", "CreateRecipeViewModel_Id");
        }
    }
}
