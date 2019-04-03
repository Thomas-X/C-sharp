namespace Space_Fridge_Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Clean : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fridges",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        HexColor = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        IngredientType_Id = c.Int(),
                        IngredientUnit_Id = c.Int(),
                        Fridge_UserId = c.String(maxLength: 128),
                        Recipe_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IngredientTypes", t => t.IngredientType_Id)
                .ForeignKey("dbo.IngredientUnits", t => t.IngredientUnit_Id)
                .ForeignKey("dbo.Fridges", t => t.Fridge_UserId)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Id)
                .Index(t => t.IngredientType_Id)
                .Index(t => t.IngredientUnit_Id)
                .Index(t => t.Fridge_UserId)
                .Index(t => t.Recipe_Id);
            
            CreateTable(
                "dbo.IngredientTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        IngredientUnit_Id = c.Int(),
                        CreateRecipeViewModel_Id = c.Int(),
                        EditFridgeViewModel_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IngredientUnits", t => t.IngredientUnit_Id)
                .ForeignKey("dbo.Recipes", t => t.CreateRecipeViewModel_Id)
                .ForeignKey("dbo.Fridges", t => t.EditFridgeViewModel_UserId)
                .Index(t => t.IngredientUnit_Id)
                .Index(t => t.CreateRecipeViewModel_Id)
                .Index(t => t.EditFridgeViewModel_UserId);
            
            CreateTable(
                "dbo.IngredientUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Unit = c.String(),
                        CreateRecipeViewModel_Id = c.Int(),
                        EditFridgeViewModel_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.CreateRecipeViewModel_Id)
                .ForeignKey("dbo.Fridges", t => t.EditFridgeViewModel_UserId)
                .Index(t => t.CreateRecipeViewModel_Id)
                .Index(t => t.EditFridgeViewModel_UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        People = c.Int(nullable: false),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.IngredientUnits", "EditFridgeViewModel_UserId", "dbo.Fridges");
            DropForeignKey("dbo.IngredientTypes", "EditFridgeViewModel_UserId", "dbo.Fridges");
            DropForeignKey("dbo.Fridges", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.IngredientUnits", "CreateRecipeViewModel_Id", "dbo.Recipes");
            DropForeignKey("dbo.IngredientTypes", "CreateRecipeViewModel_Id", "dbo.Recipes");
            DropForeignKey("dbo.Recipes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ingredients", "Recipe_Id", "dbo.Recipes");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ingredients", "Fridge_UserId", "dbo.Fridges");
            DropForeignKey("dbo.Ingredients", "IngredientUnit_Id", "dbo.IngredientUnits");
            DropForeignKey("dbo.Ingredients", "IngredientType_Id", "dbo.IngredientTypes");
            DropForeignKey("dbo.IngredientTypes", "IngredientUnit_Id", "dbo.IngredientUnits");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Recipes", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.IngredientUnits", new[] { "EditFridgeViewModel_UserId" });
            DropIndex("dbo.IngredientUnits", new[] { "CreateRecipeViewModel_Id" });
            DropIndex("dbo.IngredientTypes", new[] { "EditFridgeViewModel_UserId" });
            DropIndex("dbo.IngredientTypes", new[] { "CreateRecipeViewModel_Id" });
            DropIndex("dbo.IngredientTypes", new[] { "IngredientUnit_Id" });
            DropIndex("dbo.Ingredients", new[] { "Recipe_Id" });
            DropIndex("dbo.Ingredients", new[] { "Fridge_UserId" });
            DropIndex("dbo.Ingredients", new[] { "IngredientUnit_Id" });
            DropIndex("dbo.Ingredients", new[] { "IngredientType_Id" });
            DropIndex("dbo.Fridges", new[] { "UserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Recipes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.IngredientUnits");
            DropTable("dbo.IngredientTypes");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Fridges");
        }
    }
}
