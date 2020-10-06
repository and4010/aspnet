namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class changeosptables : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OSP_SOA_S1_T");
            DropPrimaryKey("dbo.OSP_SOA_S2_T");
            DropPrimaryKey("dbo.OSP_SOA_S3_T");
            AddColumn("dbo.OSP_SOA_S1_T", "OspSoaS1Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.OSP_SOA_S2_T", "OspSoaS2Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.OSP_SOA_S3_T", "OspSoaS3Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.OSP_SOA_S1_T", "OspSoaS1Id");
            AddPrimaryKey("dbo.OSP_SOA_S2_T", "OspSoaS2Id");
            AddPrimaryKey("dbo.OSP_SOA_S3_T", "OspSoaS3Id");

            var baseDir = AppDomain.CurrentDomain
              .BaseDirectory
              .Replace("\\bin", string.Empty) + "Data\\SQLScript\\";

            DropStoredProcedureIfExists("SP_P219_OspStCreateNew");
            SqlFile(Path.Combine(baseDir, "114_SP_P219_OspStCreateNew.sql"));
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OSP_SOA_S3_T");
            DropPrimaryKey("dbo.OSP_SOA_S2_T");
            DropPrimaryKey("dbo.OSP_SOA_S1_T");
            DropColumn("dbo.OSP_SOA_S3_T", "OspSoaS3Id");
            DropColumn("dbo.OSP_SOA_S2_T", "OspSoaS2Id");
            DropColumn("dbo.OSP_SOA_S1_T", "OspSoaS1Id");
            AddPrimaryKey("dbo.OSP_SOA_S3_T", new[] { "OSP_HEADER_ID", "PROCESS_CODE", "SERVER_CODE", "BATCH_ID" });
            AddPrimaryKey("dbo.OSP_SOA_S2_T", new[] { "OSP_HEADER_ID", "PROCESS_CODE", "SERVER_CODE", "BATCH_ID" });
            AddPrimaryKey("dbo.OSP_SOA_S1_T", new[] { "OSP_HEADER_ID", "PROCESS_CODE", "SERVER_CODE", "BATCH_ID" });
        }

        private void DropFunctionIfExists(string functionName)
        {

            Sql($@"IF EXISTS ( SELECT * FROM sysobjects WHERE id = object_id(N'{functionName}') AND xtype IN (N'FN', N'IF', N'TF') ) DROP FUNCTION {functionName}");
        }

        private void DropStoredProcedureIfExists(string spName)
        {
            Sql($@"IF EXISTS ( SELECT * FROM sysobjects WHERE id = object_id(N'{spName}') AND xtype IN (N'P') ) DROP PROCEDURE {spName}");
        }
    }
}
