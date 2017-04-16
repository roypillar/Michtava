namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1604 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SchoolClassHomeworks", "Homework_Id", "dbo.Homework");
            DropForeignKey("dbo.Questions", "Homework_Id", "dbo.Homework");
            DropForeignKey("dbo.Answers", "Answer_To_Id", "dbo.Homework");
            DropForeignKey("dbo.Questions", "Policy_Id", "dbo.Policies");
            DropForeignKey("dbo.Homework", "Text_Id", "dbo.Texts");
            DropIndex("dbo.Answers", new[] { "Answer_To_Id" });
            DropIndex("dbo.Homework", new[] { "Text_Id" });
            DropIndex("dbo.Questions", new[] { "Policy_Id" });
            DropIndex("dbo.Questions", new[] { "Homework_Id" });
            DropIndex("dbo.SchoolClassHomeworks", new[] { "Homework_Id" });
            DropPrimaryKey("dbo.Answers");
            DropPrimaryKey("dbo.Homework");
            DropPrimaryKey("dbo.Subjects");
            DropPrimaryKey("dbo.Questions");
            DropPrimaryKey("dbo.Policies");
            DropPrimaryKey("dbo.Texts");
            DropPrimaryKey("dbo.SchoolClassHomeworks");
            AddColumn("dbo.Texts", "FileName", c => c.String());
            AddColumn("dbo.Texts", "Subject_Id", c => c.Guid());

            DropColumn("dbo.Answers", "Id");
            AddColumn("dbo.Answers", "Id", c => c.Guid(nullable: false, identity: true));

            DropColumn("dbo.Homework", "Id");
            AddColumn("dbo.Homework", "Id", c => c.Guid(nullable: false, identity: true));

            DropColumn("dbo.Subjects", "Id");
            AddColumn("dbo.Subjects", "Id", c => c.Guid(nullable: false, identity: true));

            DropColumn("dbo.Texts", "Id");
            AddColumn("dbo.Texts", "Id", c => c.Guid(nullable: false, identity: true));



            DropColumn("dbo.Answers", "Answer_To_Id");
            AddColumn("dbo.Answers", "Answer_To_Id", c => c.Guid(nullable: false));


            DropColumn("dbo.Homework", "Text_Id");
            AddColumn("dbo.Homework", "Text_Id", c => c.Guid(nullable: false));


            //AlterColumn("dbo.Homework", "Text_Id", c => c.Guid());


            AlterColumn("dbo.Questions", "Id", c => c.Int(nullable: false, identity: true));//for now
           
            AlterColumn("dbo.Questions", "Policy_Id", c => c.Int());



            DropColumn("dbo.Questions", "Homework_Id");
            AddColumn("dbo.Questions", "Homework_Id", c => c.Guid(nullable: false));


            //AlterColumn("dbo.Questions", "Homework_Id", c => c.Guid());



            AlterColumn("dbo.Policies", "Id", c => c.Int(nullable: false, identity: true));


            DropColumn("dbo.SchoolClassHomeworks", "Homework_Id");
            AddColumn("dbo.SchoolClassHomeworks", "Homework_Id", c => c.Guid(nullable: false));

            //AlterColumn("dbo.SchoolClassHomeworks", "Homework_Id", c => c.Guid(nullable: false));


            AddPrimaryKey("dbo.Answers", "Id");
            AddPrimaryKey("dbo.Homework", "Id");
            AddPrimaryKey("dbo.Subjects", "Id");
            AddPrimaryKey("dbo.Questions", "Id");
            AddPrimaryKey("dbo.Policies", "Id");
            AddPrimaryKey("dbo.Texts", "Id");
            AddPrimaryKey("dbo.SchoolClassHomeworks", new[] { "SchoolClass_Id", "Homework_Id" });
            CreateIndex("dbo.Answers", "Answer_To_Id");
            CreateIndex("dbo.Homework", "Text_Id");
            CreateIndex("dbo.Questions", "Policy_Id");
            CreateIndex("dbo.Questions", "Homework_Id");
            CreateIndex("dbo.Texts", "Subject_Id");
            CreateIndex("dbo.SchoolClassHomeworks", "Homework_Id");
            AddForeignKey("dbo.Texts", "Subject_Id", "dbo.Subjects", "Id");
            AddForeignKey("dbo.SchoolClassHomeworks", "Homework_Id", "dbo.Homework", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Questions", "Homework_Id", "dbo.Homework", "Id");
            AddForeignKey("dbo.Answers", "Answer_To_Id", "dbo.Homework", "Id");
            AddForeignKey("dbo.Questions", "Policy_Id", "dbo.Policies", "Id");
            AddForeignKey("dbo.Homework", "Text_Id", "dbo.Texts", "Id");
            DropColumn("dbo.Texts", "Format");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Texts", "Format", c => c.Int(nullable: false));
            DropForeignKey("dbo.Homework", "Text_Id", "dbo.Texts");
            DropForeignKey("dbo.Questions", "Policy_Id", "dbo.Policies");
            DropForeignKey("dbo.Answers", "Answer_To_Id", "dbo.Homework");
            DropForeignKey("dbo.Questions", "Homework_Id", "dbo.Homework");
            DropForeignKey("dbo.SchoolClassHomeworks", "Homework_Id", "dbo.Homework");
            DropForeignKey("dbo.Texts", "Subject_Id", "dbo.Subjects");
            DropIndex("dbo.SchoolClassHomeworks", new[] { "Homework_Id" });
            DropIndex("dbo.Texts", new[] { "Subject_Id" });
            DropIndex("dbo.Questions", new[] { "Homework_Id" });
            DropIndex("dbo.Questions", new[] { "Policy_Id" });
            DropIndex("dbo.Homework", new[] { "Text_Id" });
            DropIndex("dbo.Answers", new[] { "Answer_To_Id" });
            DropPrimaryKey("dbo.SchoolClassHomeworks");
            DropPrimaryKey("dbo.Texts");
            DropPrimaryKey("dbo.Policies");
            DropPrimaryKey("dbo.Questions");
            DropPrimaryKey("dbo.Subjects");
            DropPrimaryKey("dbo.Homework");
            DropPrimaryKey("dbo.Answers");
            AlterColumn("dbo.SchoolClassHomeworks", "Homework_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Texts", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Policies", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Questions", "Homework_Id", c => c.Int());
            AlterColumn("dbo.Questions", "Policy_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Questions", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Subjects", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Homework", "Text_Id", c => c.Int());
            AlterColumn("dbo.Homework", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Answers", "Answer_To_Id", c => c.Int());
            AlterColumn("dbo.Answers", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Texts", "Subject_Id");
            DropColumn("dbo.Texts", "FileName");
            AddPrimaryKey("dbo.SchoolClassHomeworks", new[] { "SchoolClass_Id", "Homework_Id" });
            AddPrimaryKey("dbo.Texts", "Id");
            AddPrimaryKey("dbo.Policies", "Id");
            AddPrimaryKey("dbo.Questions", "Id");
            AddPrimaryKey("dbo.Subjects", "Id");
            AddPrimaryKey("dbo.Homework", "Id");
            AddPrimaryKey("dbo.Answers", "Id");
            CreateIndex("dbo.SchoolClassHomeworks", "Homework_Id");
            CreateIndex("dbo.Questions", "Homework_Id");
            CreateIndex("dbo.Questions", "Policy_Id");
            CreateIndex("dbo.Homework", "Text_Id");
            CreateIndex("dbo.Answers", "Answer_To_Id");
            AddForeignKey("dbo.Homework", "Text_Id", "dbo.Texts", "Id");
            AddForeignKey("dbo.Questions", "Policy_Id", "dbo.Policies", "Id");
            AddForeignKey("dbo.Answers", "Answer_To_Id", "dbo.Homework", "Id");
            AddForeignKey("dbo.Questions", "Homework_Id", "dbo.Homework", "Id");
            AddForeignKey("dbo.SchoolClassHomeworks", "Homework_Id", "dbo.Homework", "Id", cascadeDelete: true);
        }
    }
}
