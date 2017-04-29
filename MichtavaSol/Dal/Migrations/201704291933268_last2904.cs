namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class last2904 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Policy_Id", "dbo.Policies");
            DropIndex("dbo.Questions", new[] { "Policy_Id" });
            DropPrimaryKey("dbo.Questions");
            DropPrimaryKey("dbo.Policies");

            DropColumn("dbo.Questions", "Id");
            AddColumn("dbo.Questions", "Id", c => c.Guid(nullable: false, identity: true));

            DropColumn("dbo.Policies", "Id");
            AddColumn("dbo.Policies", "Id", c => c.Guid(nullable: false, identity: true));


            DropColumn("dbo.Questions", "Policy_Id");
            AddColumn("dbo.Questions", "Policy_Id", c => c.Guid(nullable: false));

            AddPrimaryKey("dbo.Questions", "Id");
            AddPrimaryKey("dbo.Policies", "Id");
            CreateIndex("dbo.Questions", "Policy_Id");
            AddForeignKey("dbo.Questions", "Policy_Id", "dbo.Policies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Policy_Id", "dbo.Policies");
            DropIndex("dbo.Questions", new[] { "Policy_Id" });
            DropPrimaryKey("dbo.Policies");
            DropPrimaryKey("dbo.Questions");
            AlterColumn("dbo.Policies", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Questions", "Policy_Id", c => c.Int());
            AlterColumn("dbo.Questions", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Policies", "Id");
            AddPrimaryKey("dbo.Questions", "Id");
            CreateIndex("dbo.Questions", "Policy_Id");
            AddForeignKey("dbo.Questions", "Policy_Id", "dbo.Policies", "Id");
        }
    }
}
