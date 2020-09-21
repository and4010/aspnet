namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixProcessTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "LAST_UPDATE_BY", c => c.String(maxLength: 128));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "LAST_UPDATE_DATE", c => c.DateTime());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "LAST_UPDATE_BY", c => c.String(maxLength: 128));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "LAST_UPDATE_DATE", c => c.DateTime());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "LAST_UPDATE_BY", c => c.String());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "LAST_UPDATE_BY", c => c.String(maxLength: 128));
            AlterColumn("dbo.OSP_ORG_T", "LAST_UPDATE_BY", c => c.String(maxLength: 128));
            AlterColumn("dbo.OSP_ORG_T", "LAST_UPDATE_DATE", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OSP_ORG_T", "LAST_UPDATE_DATE", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "LAST_UPDATE_BY", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "LAST_UPDATE_BY", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "LAST_UPDATE_BY", c => c.String(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "LAST_UPDATE_DATE", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "LAST_UPDATE_BY", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "LAST_UPDATE_DATE", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "LAST_UPDATE_BY", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
