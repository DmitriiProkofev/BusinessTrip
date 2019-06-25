namespace Core.BusinessTrip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveCategoryToDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "Category", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Positions", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Positions", "Category", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Departments", "Category");
        }
    }
}
