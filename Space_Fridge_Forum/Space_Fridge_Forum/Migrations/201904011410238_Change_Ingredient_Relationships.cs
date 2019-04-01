namespace Space_Fridge_Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Ingredient_Relationships : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IngredientUnits", "IngredientType_Id", "dbo.IngredientTypes");
            DropIndex("dbo.IngredientUnits", new[] { "IngredientType_Id" });
            AddColumn("dbo.IngredientTypes", "IngredientUnit_Id", c => c.Int());
            CreateIndex("dbo.IngredientTypes", "IngredientUnit_Id");
            AddForeignKey("dbo.IngredientTypes", "IngredientUnit_Id", "dbo.IngredientUnits", "Id");
            DropColumn("dbo.IngredientUnits", "IngredientType_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IngredientUnits", "IngredientType_Id", c => c.Int());
            DropForeignKey("dbo.IngredientTypes", "IngredientUnit_Id", "dbo.IngredientUnits");
            DropIndex("dbo.IngredientTypes", new[] { "IngredientUnit_Id" });
            DropColumn("dbo.IngredientTypes", "IngredientUnit_Id");
            CreateIndex("dbo.IngredientUnits", "IngredientType_Id");
            AddForeignKey("dbo.IngredientUnits", "IngredientType_Id", "dbo.IngredientTypes", "Id");
        }
    }
}
