namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        CustomerEmail = c.String(),
                        Id = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        AppointmentDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.SpaServices", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Bio = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.SpaServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        Duration = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpaServiceEmployees",
                c => new
                    {
                        SpaService_Id = c.Int(nullable: false),
                        Employee_EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SpaService_Id, t.Employee_EmployeeId })
                .ForeignKey("dbo.SpaServices", t => t.SpaService_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_EmployeeId, cascadeDelete: true)
                .Index(t => t.SpaService_Id)
                .Index(t => t.Employee_EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "Id", "dbo.SpaServices");
            DropForeignKey("dbo.Appointments", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.SpaServiceEmployees", "Employee_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.SpaServiceEmployees", "SpaService_Id", "dbo.SpaServices");
            DropIndex("dbo.SpaServiceEmployees", new[] { "Employee_EmployeeId" });
            DropIndex("dbo.SpaServiceEmployees", new[] { "SpaService_Id" });
            DropIndex("dbo.Appointments", new[] { "EmployeeId" });
            DropIndex("dbo.Appointments", new[] { "Id" });
            DropTable("dbo.SpaServiceEmployees");
            DropTable("dbo.SpaServices");
            DropTable("dbo.Employees");
            DropTable("dbo.Appointments");
        }
    }
}
