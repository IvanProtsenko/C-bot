namespace CSharpVKbot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        timestamp = c.DateTime(nullable: false),
                        Attendant_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Students", t => t.Attendant_ID)
                .Index(t => t.Attendant_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendances", "Attendant_ID", "dbo.Students");
            DropIndex("dbo.Attendances", new[] { "Attendant_ID" });
            DropTable("dbo.Attendances");
        }
    }
}
