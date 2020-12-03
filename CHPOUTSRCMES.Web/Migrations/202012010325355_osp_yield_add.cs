namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class osp_yield_add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_IN_SECONDARY_QUANTITY", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_IN_PRIMARY_UOM", c => c.String(maxLength: 3));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_IN_SECONDARY_UOM", c => c.String(maxLength: 3));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_OUT_PRIMARY_QUANTITY", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_OUT_SECONDARY_QUANTITY", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_OUT_PRIMARY_UOM", c => c.String(maxLength: 3));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_Out_SECONDARY_UOM", c => c.String(maxLength: 3));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "COTANGENT_SECONDARY_QUANTITY", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "COTANGENT_PRIMARY_UOM", c => c.String(maxLength: 3));
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "COTANGENT_SECONDARY_UOM", c => c.String(maxLength: 3));
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "PRIMARY_UOM");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OSP_YIELD_VARIANCE_T", "PRIMARY_UOM", c => c.String(maxLength: 3));
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "COTANGENT_SECONDARY_UOM");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "COTANGENT_PRIMARY_UOM");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "COTANGENT_SECONDARY_QUANTITY");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_Out_SECONDARY_UOM");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_OUT_PRIMARY_UOM");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_OUT_SECONDARY_QUANTITY");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_OUT_PRIMARY_QUANTITY");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_IN_SECONDARY_UOM");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_IN_PRIMARY_UOM");
            DropColumn("dbo.OSP_YIELD_VARIANCE_T", "DETAIL_IN_SECONDARY_QUANTITY");
        }
    }
}
