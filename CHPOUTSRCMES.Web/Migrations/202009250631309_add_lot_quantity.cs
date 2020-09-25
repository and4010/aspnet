namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_lot_quantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OSP_COTANGENT_HT", "LOT_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AddColumn("dbo.OSP_COTANGENT_T", "LOT_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AddColumn("dbo.OSP_PICKED_IN_HT", "LOT_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AddColumn("dbo.OSP_PICKED_IN_T", "LOT_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AddColumn("dbo.OSP_PICKED_OUT_HT", "LOT_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AddColumn("dbo.OSP_PICKED_OUT_T", "LOT_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AddColumn("dbo.STK_TXN_T", "LOT_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AddColumn("dbo.STOCK_HT", "LOT_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AddColumn("dbo.STOCK_T", "LOT_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.STOCK_T", "LOT_QUANTITY");
            DropColumn("dbo.STOCK_HT", "LOT_QUANTITY");
            DropColumn("dbo.STK_TXN_T", "LOT_QUANTITY");
            DropColumn("dbo.OSP_PICKED_OUT_T", "LOT_QUANTITY");
            DropColumn("dbo.OSP_PICKED_OUT_HT", "LOT_QUANTITY");
            DropColumn("dbo.OSP_PICKED_IN_T", "LOT_QUANTITY");
            DropColumn("dbo.OSP_PICKED_IN_HT", "LOT_QUANTITY");
            DropColumn("dbo.OSP_COTANGENT_T", "LOT_QUANTITY");
            DropColumn("dbo.OSP_COTANGENT_HT", "LOT_QUANTITY");
        }
    }
}
