namespace BookStore.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTradePrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "TradePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "TradePrice");
        }
    }
}
