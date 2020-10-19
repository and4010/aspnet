namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_osp_soa_dtl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OSP_SOA_DTL_S1_T", "INVENTORY_ITEM_ID", c => c.Long(nullable: false));
            AddColumn("dbo.OSP_SOA_DTL_S2_T", "INVENTORY_ITEM_ID", c => c.Long(nullable: false));
            AddColumn("dbo.OSP_SOA_DTL_S3_T", "INVENTORY_ITEM_ID", c => c.Long(nullable: false));
            DropColumn("dbo.OSP_SOA_DTL_S1_T", "BATCH_LINE_ID");
            DropColumn("dbo.OSP_SOA_DTL_S2_T", "BATCH_LINE_ID");
            DropColumn("dbo.OSP_SOA_DTL_S3_T", "BATCH_LINE_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OSP_SOA_DTL_S3_T", "BATCH_LINE_ID", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.OSP_SOA_DTL_S2_T", "BATCH_LINE_ID", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.OSP_SOA_DTL_S1_T", "BATCH_LINE_ID", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.OSP_SOA_DTL_S3_T", "INVENTORY_ITEM_ID");
            DropColumn("dbo.OSP_SOA_DTL_S2_T", "INVENTORY_ITEM_ID");
            DropColumn("dbo.OSP_SOA_DTL_S1_T", "INVENTORY_ITEM_ID");
        }
    }
}
