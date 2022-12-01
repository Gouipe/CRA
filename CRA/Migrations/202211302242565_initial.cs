namespace CRA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.LigneSaisies",
                c => new
                    {
                        Ligne_id = c.Int(nullable: false, identity: true),
                        MissionDay = c.DateTime(nullable: false),
                        SendingDay = c.DateTime(nullable: false),
                        Comment = c.String(),
                        FractionDay = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Employee_EmployeeId = c.Int(nullable: false),
                        Mission_Mission_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Ligne_id)
                .ForeignKey("dbo.Employees", t => t.Employee_EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Missions", t => t.Mission_Mission_id, cascadeDelete: true)
                .Index(t => t.Employee_EmployeeId)
                .Index(t => t.Mission_Mission_id);
            
            CreateTable(
                "dbo.Missions",
                c => new
                    {
                        Mission_id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Libelle = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Mission_id);
            
            CreateTable(
                "dbo.MissionEmployees",
                c => new
                    {
                        Mission_Mission_id = c.Int(nullable: false),
                        Employee_EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Mission_Mission_id, t.Employee_EmployeeId })
                .ForeignKey("dbo.Missions", t => t.Mission_Mission_id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_EmployeeId, cascadeDelete: true)
                .Index(t => t.Mission_Mission_id)
                .Index(t => t.Employee_EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LigneSaisies", "Mission_Mission_id", "dbo.Missions");
            DropForeignKey("dbo.MissionEmployees", "Employee_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.MissionEmployees", "Mission_Mission_id", "dbo.Missions");
            DropForeignKey("dbo.LigneSaisies", "Employee_EmployeeId", "dbo.Employees");
            DropIndex("dbo.MissionEmployees", new[] { "Employee_EmployeeId" });
            DropIndex("dbo.MissionEmployees", new[] { "Mission_Mission_id" });
            DropIndex("dbo.LigneSaisies", new[] { "Mission_Mission_id" });
            DropIndex("dbo.LigneSaisies", new[] { "Employee_EmployeeId" });
            DropTable("dbo.MissionEmployees");
            DropTable("dbo.Missions");
            DropTable("dbo.LigneSaisies");
            DropTable("dbo.Employees");
        }
    }
}
