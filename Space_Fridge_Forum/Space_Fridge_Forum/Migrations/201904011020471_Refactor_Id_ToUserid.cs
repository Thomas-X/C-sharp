namespace Space_Fridge_Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refactor_Id_ToUserid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recipes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Fridges", "Id", "dbo.AspNetUsers");
            RenameColumn(table: "dbo.Recipes", name: "User_Id", newName: "User_UserId");
            RenameIndex(table: "dbo.Recipes", name: "IX_User_Id", newName: "IX_User_UserId");
            DropPrimaryKey("dbo.AspNetUsers");
            AddColumn("dbo.AspNetUsers", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String());
            AddPrimaryKey("dbo.AspNetUsers", "UserId");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Recipes", "User_UserId", "dbo.AspNetUsers", "UserId");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Fridges", "Id", "dbo.AspNetUsers", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fridges", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recipes", "User_UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropPrimaryKey("dbo.AspNetUsers");
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.AspNetUsers", "UserId");
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            RenameIndex(table: "dbo.Recipes", name: "IX_User_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Recipes", name: "User_UserId", newName: "User_Id");
            AddForeignKey("dbo.Fridges", "Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Recipes", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
