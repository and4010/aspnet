namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lotNumber_allow_null : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OSP_COTANGENT_HT", "LOT_NUMBER", c => c.String(maxLength: 80));
            AlterColumn("dbo.OSP_COTANGENT_T", "LOT_NUMBER", c => c.String(maxLength: 80));
            AlterColumn("dbo.OSP_PICKED_IN_HT", "LOT_NUMBER", c => c.String(maxLength: 80));
            AlterColumn("dbo.OSP_PICKED_IN_T", "LOT_NUMBER", c => c.String(maxLength: 80));
            AlterColumn("dbo.OSP_PICKED_OUT_HT", "LOT_NUMBER", c => c.String(maxLength: 80));
            AlterColumn("dbo.OSP_PICKED_OUT_T", "LOT_NUMBER", c => c.String(maxLength: 80));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OSP_PICKED_OUT_T", "LOT_NUMBER", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.OSP_PICKED_OUT_HT", "LOT_NUMBER", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.OSP_PICKED_IN_T", "LOT_NUMBER", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.OSP_PICKED_IN_HT", "LOT_NUMBER", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.OSP_COTANGENT_T", "LOT_NUMBER", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.OSP_COTANGENT_HT", "LOT_NUMBER", c => c.String(nullable: false, maxLength: 80));
        }
    }
}
