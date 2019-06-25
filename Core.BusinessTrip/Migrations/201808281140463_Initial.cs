namespace Core.BusinessTrip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessTrips",
                c => new
                    {
                        BusinessTripId = c.Int(nullable: false, identity: true),
                        PartyId = c.Int(),
                        TypeWorkId = c.Int(),
                        NumberDocument = c.String(maxLength: 100),
                        DateFormulation = c.DateTime(nullable: false),
                        Target = c.String(maxLength: 2000),
                        HeadStructuralDivisionId = c.Int(),
                        HeadOrganizationId = c.Int(),
                        AuthoredId = c.Int(),
                        IsHoliday = c.Boolean(nullable: false),
                        IsOrenburgWork = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BusinessTripId)
                .ForeignKey("dbo.Persons", t => t.AuthoredId)
                .ForeignKey("dbo.Persons", t => t.HeadOrganizationId)
                .ForeignKey("dbo.Persons", t => t.HeadStructuralDivisionId)
                .ForeignKey("dbo.Parties", t => t.PartyId)
                .ForeignKey("dbo.TypeWorks", t => t.TypeWorkId)
                .Index(t => t.PartyId)
                .Index(t => t.TypeWorkId)
                .Index(t => t.HeadStructuralDivisionId)
                .Index(t => t.HeadOrganizationId)
                .Index(t => t.AuthoredId);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        PersonnelNumber = c.String(nullable: false, maxLength: 50),
                        Address = c.String(maxLength: 150),
                        Head = c.String(maxLength: 150),
                        PositionId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Positions", t => t.PositionId)
                .Index(t => t.PositionId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false, maxLength: 10),
                        Nominative = c.String(nullable: false, maxLength: 150),
                        Genitive = c.String(nullable: false, maxLength: 150),
                        Dative = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.PartyPersons",
                c => new
                    {
                        Party_PartyId = c.Int(nullable: false),
                        Person_PersonId = c.Int(nullable: false),
                        IsResponsible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.Party_PartyId, t.Person_PersonId })
                .ForeignKey("dbo.Parties", t => t.Party_PartyId, cascadeDelete: true)
                .ForeignKey("dbo.Persons", t => t.Person_PersonId, cascadeDelete: true)
                .Index(t => t.Party_PartyId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.Parties",
                c => new
                    {
                        PartyId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.PartyId);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        Nominative = c.String(nullable: false, maxLength: 150),
                        Genitive = c.String(nullable: false, maxLength: 150),
                        Dative = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.PositionId);
            
            CreateTable(
                "dbo.Commands",
                c => new
                    {
                        BusinessTripId = c.Int(nullable: false),
                        ResponsibleForEquipmentId = c.Int(),
                        ResponsibleForDataId = c.Int(),
                        ResponsibleForCommunicationId = c.Int(),
                        ResponsibleForTechnicalSecurityId = c.Int(),
                        ResponsibleForFireSecurityId = c.Int(),
                        ResponsibleForIndustrialSecurityId = c.Int(),
                        ResponsibleForRigInspectionId = c.Int(),
                        ResponsibleForDealerId = c.Int(),
                        DateDelivery = c.DateTime(),
                        ResponsibleForReceivingId = c.Int(),
                        ResponsibleForMonitoringId = c.Int(),
                        ResponsibleForInformationId = c.Int(),
                    })
                .PrimaryKey(t => t.BusinessTripId)
                .ForeignKey("dbo.BusinessTrips", t => t.BusinessTripId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForCommunicationId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForDataId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForDealerId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForEquipmentId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForFireSecurityId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForIndustrialSecurityId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForInformationId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForMonitoringId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForReceivingId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForRigInspectionId)
                .ForeignKey("dbo.Persons", t => t.ResponsibleForTechnicalSecurityId)
                .Index(t => t.BusinessTripId)
                .Index(t => t.ResponsibleForEquipmentId)
                .Index(t => t.ResponsibleForDataId)
                .Index(t => t.ResponsibleForCommunicationId)
                .Index(t => t.ResponsibleForTechnicalSecurityId)
                .Index(t => t.ResponsibleForFireSecurityId)
                .Index(t => t.ResponsibleForIndustrialSecurityId)
                .Index(t => t.ResponsibleForRigInspectionId)
                .Index(t => t.ResponsibleForDealerId)
                .Index(t => t.ResponsibleForReceivingId)
                .Index(t => t.ResponsibleForMonitoringId)
                .Index(t => t.ResponsibleForInformationId);
            
            CreateTable(
                "dbo.Directions",
                c => new
                    {
                        DirectionId = c.Int(nullable: false, identity: true),
                        DateBegin = c.DateTime(),
                        DateEnd = c.DateTime(),
                        Reason = c.String(maxLength: 100),
                        OrganizationId = c.Int(),
                        LocationId = c.Int(),
                        BusinessTripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DirectionId)
                .ForeignKey("dbo.BusinessTrips", t => t.BusinessTripId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.LocationId)
                .Index(t => t.BusinessTripId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Address = c.String(maxLength: 250),
                        ShortAddress = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        OrganizationId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        ShortName = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.OrganizationId);
            
            CreateTable(
                "dbo.RequestTransports",
                c => new
                    {
                        BusinessTripId = c.Int(nullable: false),
                        Mileage = c.Double(),
                        TimeWork = c.Double(),
                        ProjectManagerID = c.Int(),
                        TransportCustomerID = c.Int(),
                        DriverId = c.Int(),
                        AddressId = c.Int(),
                        Date = c.DateTime(),
                        TimeHour = c.Double(),
                        DateFormulation = c.DateTime(),
                        ContactInformation = c.String(maxLength: 250),
                        TransportId = c.Int(),
                    })
                .PrimaryKey(t => t.BusinessTripId)
                .ForeignKey("dbo.Locations", t => t.AddressId)
                .ForeignKey("dbo.BusinessTrips", t => t.BusinessTripId)
                .ForeignKey("dbo.Persons", t => t.DriverId)
                .ForeignKey("dbo.Persons", t => t.ProjectManagerID)
                .ForeignKey("dbo.Transports", t => t.TransportId)
                .ForeignKey("dbo.Persons", t => t.TransportCustomerID)
                .Index(t => t.BusinessTripId)
                .Index(t => t.ProjectManagerID)
                .Index(t => t.TransportCustomerID)
                .Index(t => t.DriverId)
                .Index(t => t.AddressId)
                .Index(t => t.TransportId);
            
            CreateTable(
                "dbo.Transports",
                c => new
                    {
                        TransportId = c.Int(nullable: false, identity: true),
                        Mark = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.TransportId);
            
            CreateTable(
                "dbo.TypeWorks",
                c => new
                    {
                        TypeWorkId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        TemplateDecree = c.String(maxLength: 300),
                        TemplateTask = c.String(maxLength: 300),
                        TemplateRequestTransport = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.TypeWorkId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BusinessTrips", "TypeWorkId", "dbo.TypeWorks");
            DropForeignKey("dbo.RequestTransports", "TransportCustomerID", "dbo.Persons");
            DropForeignKey("dbo.RequestTransports", "TransportId", "dbo.Transports");
            DropForeignKey("dbo.RequestTransports", "ProjectManagerID", "dbo.Persons");
            DropForeignKey("dbo.RequestTransports", "DriverId", "dbo.Persons");
            DropForeignKey("dbo.RequestTransports", "BusinessTripId", "dbo.BusinessTrips");
            DropForeignKey("dbo.RequestTransports", "AddressId", "dbo.Locations");
            DropForeignKey("dbo.BusinessTrips", "PartyId", "dbo.Parties");
            DropForeignKey("dbo.BusinessTrips", "HeadStructuralDivisionId", "dbo.Persons");
            DropForeignKey("dbo.BusinessTrips", "HeadOrganizationId", "dbo.Persons");
            DropForeignKey("dbo.Directions", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Directions", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Directions", "BusinessTripId", "dbo.BusinessTrips");
            DropForeignKey("dbo.Commands", "ResponsibleForTechnicalSecurityId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForRigInspectionId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForReceivingId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForMonitoringId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForInformationId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForIndustrialSecurityId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForFireSecurityId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForEquipmentId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForDealerId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForDataId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "ResponsibleForCommunicationId", "dbo.Persons");
            DropForeignKey("dbo.Commands", "BusinessTripId", "dbo.BusinessTrips");
            DropForeignKey("dbo.BusinessTrips", "AuthoredId", "dbo.Persons");
            DropForeignKey("dbo.Persons", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.PartyPersons", "Person_PersonId", "dbo.Persons");
            DropForeignKey("dbo.PartyPersons", "Party_PartyId", "dbo.Parties");
            DropForeignKey("dbo.Persons", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.RequestTransports", new[] { "TransportId" });
            DropIndex("dbo.RequestTransports", new[] { "AddressId" });
            DropIndex("dbo.RequestTransports", new[] { "DriverId" });
            DropIndex("dbo.RequestTransports", new[] { "TransportCustomerID" });
            DropIndex("dbo.RequestTransports", new[] { "ProjectManagerID" });
            DropIndex("dbo.RequestTransports", new[] { "BusinessTripId" });
            DropIndex("dbo.Directions", new[] { "BusinessTripId" });
            DropIndex("dbo.Directions", new[] { "LocationId" });
            DropIndex("dbo.Directions", new[] { "OrganizationId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForInformationId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForMonitoringId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForReceivingId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForDealerId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForRigInspectionId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForIndustrialSecurityId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForFireSecurityId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForTechnicalSecurityId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForCommunicationId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForDataId" });
            DropIndex("dbo.Commands", new[] { "ResponsibleForEquipmentId" });
            DropIndex("dbo.Commands", new[] { "BusinessTripId" });
            DropIndex("dbo.PartyPersons", new[] { "Person_PersonId" });
            DropIndex("dbo.PartyPersons", new[] { "Party_PartyId" });
            DropIndex("dbo.Persons", new[] { "DepartmentId" });
            DropIndex("dbo.Persons", new[] { "PositionId" });
            DropIndex("dbo.BusinessTrips", new[] { "AuthoredId" });
            DropIndex("dbo.BusinessTrips", new[] { "HeadOrganizationId" });
            DropIndex("dbo.BusinessTrips", new[] { "HeadStructuralDivisionId" });
            DropIndex("dbo.BusinessTrips", new[] { "TypeWorkId" });
            DropIndex("dbo.BusinessTrips", new[] { "PartyId" });
            DropTable("dbo.TypeWorks");
            DropTable("dbo.Transports");
            DropTable("dbo.RequestTransports");
            DropTable("dbo.Organizations");
            DropTable("dbo.Locations");
            DropTable("dbo.Directions");
            DropTable("dbo.Commands");
            DropTable("dbo.Positions");
            DropTable("dbo.Parties");
            DropTable("dbo.PartyPersons");
            DropTable("dbo.Departments");
            DropTable("dbo.Persons");
            DropTable("dbo.BusinessTrips");
        }
    }
}
