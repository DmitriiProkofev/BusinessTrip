namespace Core.BusinessTrip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProxyPropertyAndChangeMaxLength : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessTrips", "ProxyHeadSD", c => c.String(maxLength: 250));
            AddColumn("dbo.BusinessTrips", "ProxyHeadO", c => c.String(maxLength: 250));
            AddColumn("dbo.Positions", "Category", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Departments", "Nominative", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Departments", "Genitive", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Departments", "Dative", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Positions", "Nominative", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Positions", "Genitive", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Positions", "Dative", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Transports", "Mark", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transports", "Mark", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Positions", "Dative", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Positions", "Genitive", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Positions", "Nominative", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Departments", "Dative", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Departments", "Genitive", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Departments", "Nominative", c => c.String(nullable: false, maxLength: 150));
            DropColumn("dbo.Positions", "Category");
            DropColumn("dbo.BusinessTrips", "ProxyHeadO");
            DropColumn("dbo.BusinessTrips", "ProxyHeadSD");
        }
    }
}
