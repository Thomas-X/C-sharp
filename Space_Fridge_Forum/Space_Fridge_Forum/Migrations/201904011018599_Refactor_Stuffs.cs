namespace Space_Fridge_Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refactor_Stuffs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fridges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        HexColor = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        People = c.Int(nullable: false),
                        Description = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        IngredientType_Id = c.Int(),
                        IngredientUnit_Id = c.Int(),
                        Recipe_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IngredientTypes", t => t.IngredientType_Id)
                .ForeignKey("dbo.IngredientUnits", t => t.IngredientUnit_Id)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Id)
                .Index(t => t.IngredientType_Id)
                .Index(t => t.IngredientUnit_Id)
                .Index(t => t.Recipe_Id);
            
            CreateTable(
                "dbo.IngredientTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IngredientUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Unit = c.String(),
                        IngredientType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IngredientTypes", t => t.IngredientType_Id)
                .Index(t => t.IngredientType_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fridges", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recipes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ingredients", "Recipe_Id", "dbo.Recipes");
            DropForeignKey("dbo.Ingredients", "IngredientUnit_Id", "dbo.IngredientUnits");
            DropForeignKey("dbo.IngredientUnits", "IngredientType_Id", "dbo.IngredientTypes");
            DropForeignKey("dbo.Ingredients", "IngredientType_Id", "dbo.IngredientTypes");
            DropIndex("dbo.IngredientUnits", new[] { "IngredientType_Id" });
            DropIndex("dbo.Ingredients", new[] { "Recipe_Id" });
            DropIndex("dbo.Ingredients", new[] { "IngredientUnit_Id" });
            DropIndex("dbo.Ingredients", new[] { "IngredientType_Id" });
            DropIndex("dbo.Recipes", new[] { "User_Id" });
            DropIndex("dbo.Fridges", new[] { "Id" });
            DropTable("dbo.IngredientUnits");
            DropTable("dbo.IngredientTypes");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Recipes");
            DropTable("dbo.Fridges");
        }
    }
}
