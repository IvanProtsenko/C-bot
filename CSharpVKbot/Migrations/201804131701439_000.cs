namespace CSharpVKbot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _000 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        VKID = c.Int(nullable: false),
                        FirstName = c.String(maxLength: 255),
                        LastName = c.String(maxLength: 255),
                        Email = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.VKID, unique: true)
                .Index(t => t.Email, unique: true, name: "IX_EMAIL");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Students", "IX_EMAIL");
            DropIndex("dbo.Students", new[] { "VKID" });
            DropTable("dbo.Students");
        }
    }
}
