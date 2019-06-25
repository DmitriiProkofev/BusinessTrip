namespace Core.BusinessTrip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhoneNumberToPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Persons", "PhoneNumber", c => c.String(maxLength: 150));
            DropColumn("dbo.RequestTransports", "ContactInformation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestTransports", "ContactInformation", c => c.String(maxLength: 250));
            DropColumn("dbo.Persons", "PhoneNumber");
        }
    }
}
