namespace ShoppingCartSample.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCartDataSchema3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ProductName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ProductName");
        }
    }
}
