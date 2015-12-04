namespace ShoppingCartSample.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCartDataSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        LastUpdatedUtc = c.DateTime(nullable: false),
                        IsCheckedOut = c.Boolean(nullable: false),
                        IsCleared = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        ProductID = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Cart_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Carts", t => t.Cart_ID)
                .Index(t => t.Cart_ID);
            
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                        Description = c.String(),
                        ImageUri = c.String(),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserActionLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        UserActionId = c.Int(nullable: false),
                        Message = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Cart_ID", "dbo.Carts");
            DropIndex("dbo.Orders", new[] { "Cart_ID" });
            DropTable("dbo.UserActionLogs");
            DropTable("dbo.Products");
            DropTable("dbo.Currencies");
            DropTable("dbo.Orders");
            DropTable("dbo.Carts");
        }
    }
}
