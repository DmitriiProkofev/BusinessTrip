namespace Core.BusinessTrip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditRequestTransport : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestTransports", "TransportCustomerID", "dbo.Persons");
            DropIndex("dbo.RequestTransports", new[] { "TransportCustomerID" });
            DropColumn("dbo.RequestTransports", "TransportCustomerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestTransports", "TransportCustomerID", c => c.Int());
            CreateIndex("dbo.RequestTransports", "TransportCustomerID");
            AddForeignKey("dbo.RequestTransports", "TransportCustomerID", "dbo.Persons", "PersonId");
        }
    }
}
