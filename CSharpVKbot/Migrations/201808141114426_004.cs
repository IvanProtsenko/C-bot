namespace CSharpVKbot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _004 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons");
            DropIndex("dbo.Attendances", new[] { "Lesson_ID" });
            AlterColumn("dbo.Attendances", "Lesson_ID", c => c.Guid(nullable: false));
            CreateIndex("dbo.Attendances", "Lesson_ID");
            AddForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons");
            DropIndex("dbo.Attendances", new[] { "Lesson_ID" });
            AlterColumn("dbo.Attendances", "Lesson_ID", c => c.Guid());
            CreateIndex("dbo.Attendances", "Lesson_ID");
            AddForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons", "ID");
        }
    }
}
