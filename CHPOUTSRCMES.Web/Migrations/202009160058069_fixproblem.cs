namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixproblem : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DOC_UNIQUE_T", "DOC_UNIQUE_T_IDX1");
            DropPrimaryKey("dbo.DOC_UNIQUE_T");
            AlterColumn("dbo.DOC_UNIQUE_T", "DOC_NO", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.DOC_UNIQUE_T", "DOC_PREFIX", c => c.String(nullable: false, maxLength: 30));
            AddPrimaryKey("dbo.DOC_UNIQUE_T", "DOC_NO");
            CreateIndex("dbo.DOC_UNIQUE_T", new[] { "DOC_PREFIX", "DOC_SEQ" }, name: "DOC_UNIQUE_T_IDX1");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DOC_UNIQUE_T", "DOC_UNIQUE_T_IDX1");
            DropPrimaryKey("dbo.DOC_UNIQUE_T");
            AlterColumn("dbo.DOC_UNIQUE_T", "DOC_PREFIX", c => c.String(nullable: false));
            AlterColumn("dbo.DOC_UNIQUE_T", "DOC_NO", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.DOC_UNIQUE_T", "DOC_NO");
            CreateIndex("dbo.DOC_UNIQUE_T", new[] { "DOC_PREFIX", "DOC_SEQ" }, name: "DOC_UNIQUE_T_IDX1");
        }
    }
}
