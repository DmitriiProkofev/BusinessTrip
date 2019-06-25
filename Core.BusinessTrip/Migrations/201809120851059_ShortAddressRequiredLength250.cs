namespace Core.BusinessTrip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShortAddressRequiredLength250 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "ShortAddress", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Locations", "ShortAddress", c => c.String(maxLength: 100));
        }
    }
}
