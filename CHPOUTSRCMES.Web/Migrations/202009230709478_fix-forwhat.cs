namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class fixforwhat : DbMigration
    {
        public override void Up()
        {
            var baseDir = AppDomain.CurrentDomain
              .BaseDirectory
              .Replace("\\bin", string.Empty) + "Data\\SQLScript\\";

            DropStoredProcedureIfExists("SP_P222_TrfInvStUpload");
            DropStoredProcedureIfExists("SP_P222_TrfMiscStUpload");
            DropStoredProcedureIfExists("SP_P222_TrfObsStUpload");
            DropStoredProcedureIfExists("SP_P222_TrfRsnStUpload");
            DropStoredProcedureIfExists("SP_P222_TrfStUpload");

            SqlFile(Path.Combine(baseDir, "121_SP_P222_TrfStUpload.sql"));
            SqlFile(Path.Combine(baseDir, "122_SP_P222_TrfRsnStUpload.sql"));
            SqlFile(Path.Combine(baseDir, "123_SP_P222_TrfInvStUpload.sql"));
            SqlFile(Path.Combine(baseDir, "124_SP_P222_TrfMiscStUpload.sql"));
            SqlFile(Path.Combine(baseDir, "125_SP_P222_TrfObsStUpload.sql"));

        }
        
        public override void Down()
        {
            DropStoredProcedureIfExists("SP_P222_TrfInvStUpload");
            DropStoredProcedureIfExists("SP_P222_TrfMiscStUpload");
            DropStoredProcedureIfExists("SP_P222_TrfObsStUpload");
            DropStoredProcedureIfExists("SP_P222_TrfRsnStUpload");
            DropStoredProcedureIfExists("SP_P222_TrfStUpload");
        }

        private void DropStoredProcedureIfExists(string spName)
        {
            Sql($@"IF EXISTS ( SELECT * FROM sysobjects WHERE id = object_id(N'{spName}') AND xtype IN (N'P') ) DROP PROCEDURE {spName}");
        }
    }
}
