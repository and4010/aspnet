namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class fixtrf_soa_t : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TRF_REASON_SOA_T", name: "TRANSFER_REASON_SOA_ID", newName: "TRANSFER_REASON_HEADER_ID");
            DropPrimaryKey("dbo.TRF_SOA_T");
            AddColumn("dbo.TRF_SOA_T", "TRANSFER_TYPE", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.TRF_SOA_T", new[] { "TRANSFER_HEADER_ID", "TRANSFER_TYPE", "PROCESS_CODE", "SERVER_CODE", "BATCH_ID" });

            var baseDir = AppDomain.CurrentDomain
              .BaseDirectory
              .Replace("\\bin", string.Empty) + "Data\\SQLScript\\";

            DropFunctionIfExists("InboundFlatPickingList");

            SqlFile(Path.Combine(baseDir, "019_InboundFlatPickingList.sql"));

            DropFunctionIfExists("OutboundPickingList");

            SqlFile(Path.Combine(baseDir, "020_OutboundPickingList.sql"));

            DropFunctionIfExists("InboundRollPickingList");

            SqlFile(Path.Combine(baseDir, "021_InboundRollPickingList.sql"));

        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TRF_SOA_T");
            DropColumn("dbo.TRF_SOA_T", "TRANSFER_TYPE");
            AddPrimaryKey("dbo.TRF_SOA_T", new[] { "TRANSFER_HEADER_ID", "PROCESS_CODE", "SERVER_CODE", "BATCH_ID" });
            RenameColumn(table: "dbo.TRF_REASON_SOA_T", name: "TRANSFER_REASON_HEADER_ID", newName: "TRANSFER_REASON_SOA_ID");

            DropFunctionIfExists("InboundFlatPickingList");

            DropFunctionIfExists("OutboundPickingList");

            DropFunctionIfExists("InboundRollPickingList");

        }

        private void DropFunctionIfExists(string functionName)
        {
            Sql($@"IF EXISTS ( SELECT * FROM sysobjects WHERE id = object_id(N'{functionName}') AND xtype IN (N'FN', N'IF', N'TF') ) DROP FUNCTION {functionName}");
        }
    }
}
