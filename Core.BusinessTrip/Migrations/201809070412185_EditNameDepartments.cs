namespace Core.BusinessTrip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditNameDepartments : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Departments", "Genitive");
            DropColumn("dbo.Departments", "Dative");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Departments", "Dative", c => c.String(nullable: false, maxLength: 250));
            AddColumn("dbo.Departments", "Genitive", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
