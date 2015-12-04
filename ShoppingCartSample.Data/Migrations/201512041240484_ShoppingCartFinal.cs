namespace ShoppingCartSample.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCartFinal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Cart_ID", "dbo.Carts");
            DropIndex("dbo.Orders", new[] { "Cart_ID" });
            RenameColumn(table: "dbo.Orders", name: "Cart_ID", newName: "CartID");
            AlterColumn("dbo.Orders", "CartID", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "CartID");
            AddForeignKey("dbo.Orders", "CartID", "dbo.Carts", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CartID", "dbo.Carts");
            DropIndex("dbo.Orders", new[] { "CartID" });
            AlterColumn("dbo.Orders", "CartID", c => c.Int());
            RenameColumn(table: "dbo.Orders", name: "CartID", newName: "Cart_ID");
            CreateIndex("dbo.Orders", "Cart_ID");
            AddForeignKey("dbo.Orders", "Cart_ID", "dbo.Carts", "ID");
        }
    }
}
