namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class add_several_soa_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OSP_SOA_S1_T",
                c => new
                    {
                        OSP_HEADER_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        STATUS_CODE = c.String(maxLength: 1),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.OSP_HEADER_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });
            
            CreateTable(
                "dbo.OSP_SOA_S2_T",
                c => new
                    {
                        OSP_HEADER_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        STATUS_CODE = c.String(maxLength: 1),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.OSP_HEADER_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });
            
            CreateTable(
                "dbo.OSP_SOA_S3_T",
                c => new
                    {
                        OSP_HEADER_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        STATUS_CODE = c.String(maxLength: 1),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                        LAST_UPDATE_BY = c.String(maxLength: 128),
                        LAST_UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.OSP_HEADER_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });
            
            CreateTable(
                "dbo.TRF_INVENTORY_SOA_T",
                c => new
                    {
                        TRANSFER_INVENTORY_HEADER_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        STATUS_CODE = c.String(maxLength: 1),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TRANSFER_INVENTORY_HEADER_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });
            
            CreateTable(
                "dbo.TRF_MISCELLANEOUS_SOA_T",
                c => new
                    {
                        TRANSFER_MISCELLANEOUS_HEADER_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        STATUS_CODE = c.String(maxLength: 1),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TRANSFER_MISCELLANEOUS_HEADER_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });
            
            CreateTable(
                "dbo.TRF_OBSOLETE_SOA_T",
                c => new
                    {
                        TRANSFER_OBSOLETE_HEADER_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        STATUS_CODE = c.String(maxLength: 1),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TRANSFER_OBSOLETE_HEADER_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });
            
            CreateTable(
                "dbo.TRF_REASON_SOA_T",
                c => new
                    {
                        TRANSFER_REASON_SOA_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        STATUS_CODE = c.String(maxLength: 1),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TRANSFER_REASON_SOA_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });
            
            CreateTable(
                "dbo.TRF_SOA_T",
                c => new
                    {
                        TRANSFER_HEADER_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        STATUS_CODE = c.String(maxLength: 1),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TRANSFER_HEADER_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });


            var baseDir = AppDomain.CurrentDomain
                  .BaseDirectory
                  .Replace("\\bin", string.Empty) + "Data\\SQLScript\\";

            DropFunctionIfExists("ConvertOspLabelDesc");
            SqlFile(Path.Combine(baseDir, "005_ConvertOspLabelDesc.sql"));

            DropFunctionIfExists("CheckOspLabelSpec");
            SqlFile(Path.Combine(baseDir, "006_CheckOspLabelSpec.sql"));

            DropFunctionIfExists("CheckOspLabelSize");
            SqlFile(Path.Combine(baseDir, "007_CheckOspLabelSize.sql"));

            DropFunctionIfExists("OspLabelKnife");
            SqlFile(Path.Combine(baseDir, "008_OspLabelKnife.sql"));

            DropFunctionIfExists("OspOutSourcCut");
            SqlFile(Path.Combine(baseDir, "009_OspOutSourcCut.sql"));

            DropFunctionIfExists("OspCutMaterial");
            SqlFile(Path.Combine(baseDir, "010_OspCutMaterial.sql"));

            DropFunctionIfExists("PurchaseHeadaer");
            SqlFile(Path.Combine(baseDir, "011_PurchaseHeadaer.sql"));

            DropFunctionIfExists("PurchaseDetail");
            SqlFile(Path.Combine(baseDir, "012_PurchaseDetail.sql"));

            DropFunctionIfExists("PurchaseReason");
            SqlFile(Path.Combine(baseDir, "013_PurchaseReason.sql"));

            DropFunctionIfExists("OspCutStock");
            SqlFile(Path.Combine(baseDir, "014_OspCutStock.sql"));
        }
        
        public override void Down()
        {
            DropFunctionIfExists("ConvertOspLabelDesc");

            DropFunctionIfExists("CheckOspLabelSpec");

            DropFunctionIfExists("CheckOspLabelSize");

            DropFunctionIfExists("OspLabelKnife");

            DropFunctionIfExists("OspOutSourcCut");

            DropFunctionIfExists("OspCutMaterial");

            DropFunctionIfExists("PurchaseHeadaer");

            DropFunctionIfExists("PurchaseDetail");

            DropFunctionIfExists("PurchaseReason");

            DropFunctionIfExists("OspCutStock");

            DropTable("dbo.TRF_SOA_T");
            DropTable("dbo.TRF_REASON_SOA_T");
            DropTable("dbo.TRF_OBSOLETE_SOA_T");
            DropTable("dbo.TRF_MISCELLANEOUS_SOA_T");
            DropTable("dbo.TRF_INVENTORY_SOA_T");
            DropTable("dbo.OSP_SOA_S3_T");
            DropTable("dbo.OSP_SOA_S2_T");
            DropTable("dbo.OSP_SOA_S1_T");
        }

        private void DropFunctionIfExists(string functionName)
        {
            Sql(
$@"IF EXISTS ( SELECT * FROM sysobjects WHERE id = object_id(N'{functionName}') AND xtype IN (N'FN', N'IF', N'TF') )
    DROP FUNCTION {functionName}");
        }
    }
}
