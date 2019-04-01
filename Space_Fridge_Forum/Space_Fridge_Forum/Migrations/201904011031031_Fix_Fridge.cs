namespace Space_Fridge_Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_Fridge : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recipes", "User_UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Fridges", "Id", "dbo.AspNetUsers");
            RenameColumn(table: "dbo.Fridges", name: "Id", newName: "UserId");
            RenameColumn(table: "dbo.Recipes", name: "User_UserId", newName: "User_Id");
            RenameIndex(table: "dbo.Fridges", name: "IX_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Recipes", name: "IX_User_UserId", newName: "IX_User_Id");
            DropPrimaryKey("dbo.AspNetUsers");
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Recipes", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Fridges", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Fridges", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recipes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropPrimaryKey("dbo.AspNetUsers");
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String());
            AddPrimaryKey("dbo.AspNetUsers", "UserId");
            RenameIndex(table: "dbo.Recipes", name: "IX_User_Id", newName: "IX_User_UserId");
            RenameIndex(table: "dbo.Fridges", name: "IX_UserId", newName: "IX_Id");
            RenameColumn(table: "dbo.Recipes", name: "User_Id", newName: "User_UserId");
            RenameColumn(table: "dbo.Fridges", name: "UserId", newName: "Id");
            AddForeignKey("dbo.Fridges", "Id", "dbo.AspNetUsers", "UserId");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Recipes", "User_UserId", "dbo.AspNetUsers", "UserId");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "UserId", cascadeDelete: true);
        }
    }
}
