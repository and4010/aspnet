namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge_branch_pon : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TRF_FILEINFO_T",
                c => new
                    {
                        TRF_FILEINFO_ID = c.Long(nullable: false, identity: true),
                        TRANSFER_REASON_ID = c.Long(nullable: false),
                        TRANSFER_REASON_HEADER_ID = c.Long(nullable: false),
                        TRF_FILE_ID = c.Long(nullable: false),
                        FILE_TYPE = c.String(nullable: false, maxLength: 10),
                        FILENAME = c.String(nullable: false, maxLength: 250),
                        SIZE = c.Long(nullable: false),
                        SEQ = c.Long(nullable: false),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TRF_FILEINFO_ID);
            
            CreateTable(
                "dbo.TRF_FILES_T",
                c => new
                    {
                        TRF_FILE_ID = c.Long(nullable: false, identity: true),
                        FILE_INSTANCE = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.TRF_FILE_ID);
            
            CreateTable(
                "dbo.TRF_REASON_HEADER_T",
                c => new
                    {
                        TRANSFER_REASON_HEADER_ID = c.Long(nullable: false, identity: true),
                        ORG_ID = c.Long(nullable: false),
                        ORGANIZATION_ID = c.Long(nullable: false),
                        ORGANIZATION_CODE = c.String(nullable: false, maxLength: 3),
                        SHIPMENT_NUMBER = c.String(maxLength: 128),
                        SUBINVENTORY_CODE = c.String(nullable: false, maxLength: 20),
                        LOCATOR_ID = c.Long(),
                        LOCATOR_CODE = c.String(maxLength: 163),
                        SEGMENT3 = c.String(maxLength: 40),
                        NUMBER_STATUS = c.String(nullable: false, maxLength: 10),
                        TRANSACTION_DATE = c.DateTime(nullable: false),
                        TRANSACTION_TYPE_ID = c.Long(nullable: false),
                        TRANSACTION_TYPE_NAME = c.String(nullable: false, maxLength: 80),
                        TRANSFER_ORG_ID = c.Long(),
                        TRANSFER_ORGANIZATION_ID = c.Long(),
                        TRANSFER_ORGANIZATION_CODE = c.String(maxLength: 3),
                        TRANSFER_SUBINVENTORY_CODE = c.String(maxLength: 20),
                        TRANSFER_LOCATOR_ID = c.Long(),
                        TRANSFER_LOCATOR_CODE = c.String(maxLength: 30),
                        TO_ERP = c.String(maxLength: 10),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_USER_NAME = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.TRANSFER_REASON_HEADER_ID);
            
            CreateTable(
                "dbo.TRF_REASON_HT",
                c => new
                    {
                        TRANSFER_REASON_HIS_ID = c.Long(nullable: false, identity: true),
                        TRANSFER_REASON_ID = c.Long(nullable: false),
                        TRANSFER_REASON_HEADER_ID = c.Long(nullable: false),
                        INVENTORY_ITEM_ID = c.Long(nullable: false),
                        ITEM_NUMBER = c.String(nullable: false, maxLength: 40),
                        ITEM_DESCRIPTION = c.String(nullable: false, maxLength: 240),
                        BARCODE = c.String(nullable: false, maxLength: 20),
                        STOCK_ID = c.Long(nullable: false),
                        PRIMARY_UOM = c.String(nullable: false, maxLength: 3),
                        PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        SECONDARY_UOM = c.String(maxLength: 3),
                        SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        LOT_NUMBER = c.String(maxLength: 80),
                        LOT_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        REASON_CODE = c.String(maxLength: 10),
                        REASON_DESC = c.String(maxLength: 50),
                        NOTE = c.String(maxLength: 240),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_USER_NAME = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.TRANSFER_REASON_HIS_ID);
            
            CreateTable(
                "dbo.TRF_REASON_T",
                c => new
                    {
                        TRANSFER_REASON_ID = c.Long(nullable: false, identity: true),
                        TRANSFER_REASON_HEADER_ID = c.Long(nullable: false),
                        INVENTORY_ITEM_ID = c.Long(nullable: false),
                        ITEM_NUMBER = c.String(nullable: false, maxLength: 40),
                        ITEM_DESCRIPTION = c.String(nullable: false, maxLength: 240),
                        BARCODE = c.String(nullable: false, maxLength: 20),
                        STOCK_ID = c.Long(nullable: false),
                        PRIMARY_UOM = c.String(nullable: false, maxLength: 3),
                        PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        SECONDARY_UOM = c.String(maxLength: 3),
                        SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        LOT_NUMBER = c.String(maxLength: 80),
                        LOT_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        REASON_CODE = c.String(maxLength: 10),
                        REASON_DESC = c.String(maxLength: 50),
                        NOTE = c.String(maxLength: 240),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_USER_NAME = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.TRANSFER_REASON_ID);



            
            AlterColumn("dbo.DLV_PICKED_HT", "PACKING_TYPE", c => c.String(maxLength: 30));
            AlterColumn("dbo.DLV_PICKED_T", "PACKING_TYPE", c => c.String(maxLength: 30));
            AlterColumn("dbo.TRF_DETAIL_HT", "PACKING_TYPE", c => c.String(maxLength: 30));
            AlterColumn("dbo.TRF_DETAIL_T", "PACKING_TYPE", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TRF_DETAIL_T", "PACKING_TYPE", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.TRF_DETAIL_HT", "PACKING_TYPE", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.DLV_PICKED_T", "PACKING_TYPE", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.DLV_PICKED_HT", "PACKING_TYPE", c => c.String(nullable: false, maxLength: 30));
            DropTable("dbo.TRF_REASON_T");
            DropTable("dbo.TRF_REASON_HT");
            DropTable("dbo.TRF_REASON_HEADER_T");
            DropTable("dbo.TRF_FILES_T");
            DropTable("dbo.TRF_FILEINFO_T");
        }
    }
}
