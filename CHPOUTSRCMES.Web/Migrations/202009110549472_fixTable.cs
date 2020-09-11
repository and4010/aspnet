namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CTR_DETAIL_HT", "CREATED_USER_NAME", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CTR_DETAIL_HT", "LAST_UPDATE_USER_NAME", c => c.String(maxLength: 128));
            AlterColumn("dbo.CTR_DETAIL_T", "CREATED_USER_NAME", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CTR_DETAIL_T", "LAST_UPDATE_USER_NAME", c => c.String(maxLength: 128));
            AlterColumn("dbo.CTR_HEADER_T", "CREATED_USER_NAME", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CTR_HEADER_T", "LAST_UPDATE_USER_NAME", c => c.String(maxLength: 128));
            AlterColumn("dbo.CTR_PICKED_HT", "LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.CTR_PICKED_HT", "BARCODE", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.CTR_PICKED_HT", "LOT_NUMBER", c => c.String(maxLength: 80));
            AlterColumn("dbo.CTR_PICKED_HT", "CREATED_USER_NAME", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CTR_PICKED_HT", "LAST_UPDATE_USER_NAME", c => c.String(maxLength: 128));
            AlterColumn("dbo.CTR_PICKED_T", "LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.CTR_PICKED_T", "BARCODE", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.CTR_PICKED_T", "LOT_NUMBER", c => c.String(maxLength: 80));
            AlterColumn("dbo.CTR_PICKED_T", "CREATED_USER_NAME", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CTR_PICKED_T", "LAST_UPDATE_USER_NAME", c => c.String(maxLength: 128));
            AlterColumn("dbo.DLV_DETAIL_HT", "REAM_WEIGHT", c => c.String(maxLength: 30));
            AlterColumn("dbo.DLV_DETAIL_HT", "LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.DLV_DETAIL_T", "REAM_WEIGHT", c => c.String(maxLength: 30));
            AlterColumn("dbo.DLV_DETAIL_T", "LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.DLV_ORG_T", "LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.DLV_PICKED_HT", "LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.DLV_PICKED_HT", "REAM_WEIGHT", c => c.String(maxLength: 30));
            AlterColumn("dbo.DLV_PICKED_T", "LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.DLV_PICKED_T", "REAM_WEIGHT", c => c.String(maxLength: 30));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PAPER_TYPE", c => c.String(maxLength: 30));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PAPER_TYPE", c => c.String(maxLength: 30));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PAPER_TYPE", c => c.String(maxLength: 30));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PAPER_TYPE", c => c.String(maxLength: 30));
            AlterColumn("dbo.OSP_ORG_T", "PAPER_TYPE", c => c.String(maxLength: 30));
            AlterColumn("dbo.STOCK_HT", "REAM_WEIGHT", c => c.String(maxLength: 30));
            AlterColumn("dbo.STOCK_T", "REAM_WEIGHT", c => c.String(maxLength: 30));
            AlterColumn("dbo.TRF_HEADER_T", "TRANSFER_LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.TRF_MISCELLANEOUS_HEADER_T", "TRANSFER_LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.TRF_OBSOLETE_HEADER_T", "TRANSFER_LOCATOR_CODE", c => c.String(maxLength: 163));
            AlterColumn("dbo.TRF_REASON_HEADER_T", "TRANSFER_LOCATOR_CODE", c => c.String(maxLength: 163));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TRF_REASON_HEADER_T", "TRANSFER_LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.TRF_OBSOLETE_HEADER_T", "TRANSFER_LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.TRF_MISCELLANEOUS_HEADER_T", "TRANSFER_LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.TRF_HEADER_T", "TRANSFER_LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.STOCK_T", "REAM_WEIGHT", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.STOCK_HT", "REAM_WEIGHT", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.OSP_ORG_T", "PAPER_TYPE", c => c.String(maxLength: 4));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PAPER_TYPE", c => c.String(maxLength: 4));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PAPER_TYPE", c => c.String(maxLength: 4));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PAPER_TYPE", c => c.String(maxLength: 4));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PAPER_TYPE", c => c.String(maxLength: 4));
            AlterColumn("dbo.DLV_PICKED_T", "REAM_WEIGHT", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.DLV_PICKED_T", "LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.DLV_PICKED_HT", "REAM_WEIGHT", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.DLV_PICKED_HT", "LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.DLV_ORG_T", "LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.DLV_DETAIL_T", "LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.DLV_DETAIL_T", "REAM_WEIGHT", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.DLV_DETAIL_HT", "LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.DLV_DETAIL_HT", "REAM_WEIGHT", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.CTR_PICKED_T", "LAST_UPDATE_USER_NAME", c => c.String());
            AlterColumn("dbo.CTR_PICKED_T", "CREATED_USER_NAME", c => c.String(nullable: false));
            AlterColumn("dbo.CTR_PICKED_T", "LOT_NUMBER", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.CTR_PICKED_T", "BARCODE", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.CTR_PICKED_T", "LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.CTR_PICKED_HT", "LAST_UPDATE_USER_NAME", c => c.String());
            AlterColumn("dbo.CTR_PICKED_HT", "CREATED_USER_NAME", c => c.String(nullable: false));
            AlterColumn("dbo.CTR_PICKED_HT", "LOT_NUMBER", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.CTR_PICKED_HT", "BARCODE", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.CTR_PICKED_HT", "LOCATOR_CODE", c => c.String(maxLength: 30));
            AlterColumn("dbo.CTR_HEADER_T", "LAST_UPDATE_USER_NAME", c => c.String());
            AlterColumn("dbo.CTR_HEADER_T", "CREATED_USER_NAME", c => c.String(nullable: false));
            AlterColumn("dbo.CTR_DETAIL_T", "LAST_UPDATE_USER_NAME", c => c.String());
            AlterColumn("dbo.CTR_DETAIL_T", "CREATED_USER_NAME", c => c.String(nullable: false));
            AlterColumn("dbo.CTR_DETAIL_HT", "LAST_UPDATE_USER_NAME", c => c.String());
            AlterColumn("dbo.CTR_DETAIL_HT", "CREATED_USER_NAME", c => c.String(nullable: false));
        }
    }
}
