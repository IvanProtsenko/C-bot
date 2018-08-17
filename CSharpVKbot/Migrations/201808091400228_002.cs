namespace CSharpVKbot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _002 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Topic = c.String(maxLength: 255),
                        BeginLesson = c.DateTime(nullable: false),
                        EndLesson = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Attendances", "Lesson_ID", c => c.Guid());
            CreateIndex("dbo.Attendances", "Lesson_ID");
            AddForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons");
            DropIndex("dbo.Attendances", new[] { "Lesson_ID" });
            DropColumn("dbo.Attendances", "Lesson_ID");
            DropTable("dbo.Lessons");
        }
    }
}
