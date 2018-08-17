namespace CSharpVKbot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _003 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendances", "Attendant_ID", "dbo.Students");
            DropForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons");
            DropIndex("dbo.Attendances", new[] { "Attendant_ID" });
            DropIndex("dbo.Attendances", new[] { "Lesson_ID" });
            AlterColumn("dbo.Attendances", "Attendant_ID", c => c.Guid(nullable: false));
            AlterColumn("dbo.Attendances", "Lesson_ID", c => c.Guid(nullable: false));
            AlterColumn("dbo.Lessons", "Topic", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.Attendances", "Attendant_ID");
            CreateIndex("dbo.Attendances", "Lesson_ID");
            AddForeignKey("dbo.Attendances", "Attendant_ID", "dbo.Students", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons");
            DropForeignKey("dbo.Attendances", "Attendant_ID", "dbo.Students");
            DropIndex("dbo.Attendances", new[] { "Lesson_ID" });
            DropIndex("dbo.Attendances", new[] { "Attendant_ID" });
            AlterColumn("dbo.Lessons", "Topic", c => c.String(maxLength: 255));
            AlterColumn("dbo.Attendances", "Lesson_ID", c => c.Guid());
            AlterColumn("dbo.Attendances", "Attendant_ID", c => c.Guid());
            CreateIndex("dbo.Attendances", "Lesson_ID");
            CreateIndex("dbo.Attendances", "Attendant_ID");
            AddForeignKey("dbo.Attendances", "Lesson_ID", "dbo.Lessons", "ID");
            AddForeignKey("dbo.Attendances", "Attendant_ID", "dbo.Students", "ID");
        }
    }
}
