namespace Space_Fridge_Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFridgeRelationshipIngredient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ingredients", "Fridge_UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Ingredients", "Fridge_UserId");
            AddForeignKey("dbo.Ingredients", "Fridge_UserId", "dbo.Fridges", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ingredients", "Fridge_UserId", "dbo.Fridges");
            DropIndex("dbo.Ingredients", new[] { "Fridge_UserId" });
            DropColumn("dbo.Ingredients", "Fridge_UserId");
        }
    }
}
