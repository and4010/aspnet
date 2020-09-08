namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class add_dlv_soa_t : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DLV_SOA_T",
                c => new
                    {
                        TRIP_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TRIP_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });
            var baseDir = AppDomain.CurrentDomain
                  .BaseDirectory
                  .Replace("\\bin", string.Empty) + "Data\\SQLScript\\";
            SqlFile(Path.Combine(baseDir, "115_SP_P218_CtrStUpload.sql"));
            SqlFile(Path.Combine(baseDir, "116_SP_P221_DlvStUpload.sql"));
        }
        
        public override void Down()
        {
            DropTable("dbo.DLV_SOA_T");
        }
    }
}
