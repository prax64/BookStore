namespace BookStore.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Books", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Books", "Category", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Category", c => c.String());
            AlterColumn("dbo.Books", "Description", c => c.String());
            AlterColumn("dbo.Books", "Name", c => c.String());
        }
    }
}
