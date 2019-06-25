namespace Core.BusinessTrip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditNumberDepartments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Persons", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Persons", new[] { "DepartmentId" });
            AlterColumn("dbo.Persons", "DepartmentId", c => c.Int());
            AlterColumn("dbo.Departments", "Number", c => c.String(maxLength: 10));
            CreateIndex("dbo.Persons", "DepartmentId");
            AddForeignKey("dbo.Persons", "DepartmentId", "dbo.Departments", "DepartmentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Persons", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Persons", new[] { "DepartmentId" });
            AlterColumn("dbo.Departments", "Number", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Persons", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Persons", "DepartmentId");
            AddForeignKey("dbo.Persons", "DepartmentId", "dbo.Departments", "DepartmentId", cascadeDelete: true);
        }
    }
}
