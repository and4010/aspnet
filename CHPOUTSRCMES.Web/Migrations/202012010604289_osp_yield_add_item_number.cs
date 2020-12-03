namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class osp_yield_add_item_number : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_IN_ITEM_NUMBER", c => c.String(maxLength: 40));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_OUT_ITEM_NUMBER", c => c.String(maxLength: 40));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "COTANGENT_ITEM_NUMBER", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "COTANGENT_ITEM_NUMBER");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_OUT_ITEM_NUMBER");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_IN_ITEM_NUMBER");
        }
    }
}
