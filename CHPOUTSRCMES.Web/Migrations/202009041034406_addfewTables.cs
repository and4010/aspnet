namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfewTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TRF_INVENTORY_HEADER_T",
                c => new
                    {
                        TRANSFER_INVENTORY_HEADER_ID = c.Long(nullable: false, identity: true),
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
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_USER_NAME = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.TRANSFER_INVENTORY_HEADER_ID);
            
            CreateTable(
                "dbo.TRF_INVENTORY_HT",
                c => new
                    {
                        TRANSFER_INVENTORY_HIS_ID = c.Long(nullable: false, identity: true),
                        TRANSFER_INVENTORY_ID = c.Long(nullable: false),
                        TRANSFER_INVENTORY_HEADER_ID = c.Long(nullable: false),
                        INVENTORY_ITEM_ID = c.Long(nullable: false),
                        ITEM_NUMBER = c.String(nullable: false, maxLength: 40),
                        ITEM_DESCRIPTION = c.String(nullable: false, maxLength: 240),
                        BARCODE = c.String(nullable: false, maxLength: 20),
                        STOCK_ID = c.Long(nullable: false),
                        PRIMARY_UOM = c.String(nullable: false, maxLength: 3),
                        TRANSFER_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        ORIGINAL_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        AFTER_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        SECONDARY_UOM = c.String(maxLength: 3),
                        TRANSFER_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        ORIGINAL_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        AFTER_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        LOT_NUMBER = c.String(maxLength: 80),
                        LOT_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        NOTE = c.String(maxLength: 240),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_USER_NAME = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.TRANSFER_INVENTORY_HIS_ID);
            
            CreateTable(
                "dbo.TRF_INVENTORY_T",
                c => new
                    {
                        TRANSFER_INVENTORY_ID = c.Long(nullable: false, identity: true),
                        TRANSFER_INVENTORY_HEADER_ID = c.Long(nullable: false),
                        INVENTORY_ITEM_ID = c.Long(nullable: false),
                        ITEM_NUMBER = c.String(nullable: false, maxLength: 40),
                        ITEM_DESCRIPTION = c.String(nullable: false, maxLength: 240),
                        BARCODE = c.String(nullable: false, maxLength: 20),
                        STOCK_ID = c.Long(nullable: false),
                        PRIMARY_UOM = c.String(nullable: false, maxLength: 3),
                        TRANSFER_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        ORIGINAL_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        AFTER_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        SECONDARY_UOM = c.String(maxLength: 3),
                        TRANSFER_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        ORIGINAL_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        AFTER_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        LOT_NUMBER = c.String(maxLength: 80),
                        LOT_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        NOTE = c.String(maxLength: 240),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_USER_NAME = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.TRANSFER_INVENTORY_ID);
            
            CreateTable(
                "dbo.TRF_OBSOLETE_HEADER_T",
                c => new
                    {
                        TRANSFER_OBSOLETE_HEADER_ID = c.Long(nullable: false, identity: true),
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
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_USER_NAME = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.TRANSFER_OBSOLETE_HEADER_ID);
            
            CreateTable(
                "dbo.TRF_OBSOLETE_HT",
                c => new
                    {
                        TRANSFER_OBSOLETE_HIS_ID = c.Long(nullable: false, identity: true),
                        TRANSFER_OBSOLETE_ID = c.Long(nullable: false),
                        TRANSFER_OBSOLETE_HEADER_ID = c.Long(nullable: false),
                        INVENTORY_ITEM_ID = c.Long(nullable: false),
                        ITEM_NUMBER = c.String(nullable: false, maxLength: 40),
                        ITEM_DESCRIPTION = c.String(nullable: false, maxLength: 240),
                        BARCODE = c.String(nullable: false, maxLength: 20),
                        STOCK_ID = c.Long(nullable: false),
                        PRIMARY_UOM = c.String(nullable: false, maxLength: 3),
                        TRANSFER_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        ORIGINAL_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        AFTER_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        SECONDARY_UOM = c.String(maxLength: 3),
                        TRANSFER_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        ORIGINAL_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        AFTER_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        LOT_NUMBER = c.String(maxLength: 80),
                        LOT_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        NOTE = c.String(maxLength: 240),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_USER_NAME = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.TRANSFER_OBSOLETE_HIS_ID);
            
            CreateTable(
                "dbo.TRF_OBSOLETE_T",
                c => new
                    {
                        TRANSFER_OBSOLETE_ID = c.Long(nullable: false, identity: true),
                        TRANSFER_OBSOLETE_HEADER_ID = c.Long(nullable: false),
                        INVENTORY_ITEM_ID = c.Long(nullable: false),
                        ITEM_NUMBER = c.String(nullable: false, maxLength: 40),
                        ITEM_DESCRIPTION = c.String(nullable: false, maxLength: 240),
                        BARCODE = c.String(nullable: false, maxLength: 20),
                        STOCK_ID = c.Long(nullable: false),
                        PRIMARY_UOM = c.String(nullable: false, maxLength: 3),
                        TRANSFER_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        ORIGINAL_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        AFTER_PRIMARY_QUANTITY = c.Decimal(nullable: false, precision: 30, scale: 10),
                        SECONDARY_UOM = c.String(maxLength: 3),
                        TRANSFER_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        ORIGINAL_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        AFTER_SECONDARY_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        LOT_NUMBER = c.String(maxLength: 80),
                        LOT_QUANTITY = c.Decimal(precision: 30, scale: 10),
                        NOTE = c.String(maxLength: 240),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATED_USER_NAME = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_USER_NAME = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.TRANSFER_OBSOLETE_ID);

        }
        
        public override void Down()
        {
            DropTable("dbo.TRF_OBSOLETE_T");
            DropTable("dbo.TRF_OBSOLETE_HT");
            DropTable("dbo.TRF_OBSOLETE_HEADER_T");
            DropTable("dbo.TRF_INVENTORY_T");
            DropTable("dbo.TRF_INVENTORY_HT");
            DropTable("dbo.TRF_INVENTORY_HEADER_T");
        }
    }
}
