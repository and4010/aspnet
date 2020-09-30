namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class add_field_osp_header_t : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OSP_HEADER_T", "SRC_OSP_HEADER_ID", c => c.Long());
            AddColumn("dbo.OSP_HEADER_T", "SRC_BATCH_NO", c => c.String(nullable: false, maxLength: 32));
            
            var baseDir = AppDomain.CurrentDomain
              .BaseDirectory
              .Replace("\\bin", string.Empty) + "Data\\SQLScript\\";

            DropFunctionIfExists("GetProcessNameByCode");
            SqlFile(Path.Combine(baseDir, "022_GetProcessNameByCode.sql"));

            DropStoredProcedureIfExists("SP_P222_TrfStUpload");
            SqlFile(Path.Combine(baseDir, "121_SP_P222_TrfStUpload.sql"));

            DropStoredProcedureIfExists("SP_P222_TrfRsnStUpload");
            SqlFile(Path.Combine(baseDir, "122_SP_P222_TrfRsnStUpload.sql"));
        }

        public override void Down()
        {
            DropColumn("dbo.OSP_HEADER_T", "SRC_BATCH_NO");
            DropColumn("dbo.OSP_HEADER_T", "SRC_OSP_HEADER_ID");
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
