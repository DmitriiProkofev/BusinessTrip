namespace Core.BusinessTrip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewLineToPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Persons", "LocationId", c => c.Int());
            AddColumn("dbo.Persons", "HeadId", c => c.Int());
            CreateIndex("dbo.Persons", "LocationId");
            CreateIndex("dbo.Persons", "HeadId");
            AddForeignKey("dbo.Persons", "HeadId", "dbo.Persons", "PersonId");
            AddForeignKey("dbo.Persons", "LocationId", "dbo.Locations", "LocationId");
            DropColumn("dbo.Persons", "Address");
            DropColumn("dbo.Persons", "Head");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Persons", "Head", c => c.String(maxLength: 150));
            AddColumn("dbo.Persons", "Address", c => c.String(maxLength: 150));
            DropForeignKey("dbo.Persons", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Persons", "HeadId", "dbo.Persons");
            DropIndex("dbo.Persons", new[] { "HeadId" });
            DropIndex("dbo.Persons", new[] { "LocationId" });
            DropColumn("dbo.Persons", "HeadId");
            DropColumn("dbo.Persons", "LocationId");
        }
    }
}
