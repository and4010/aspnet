namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class add_docnum_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DOC_UNIQUE_T",
                c => new
                    {
                        DOC_NO = c.String(nullable: false, maxLength: 50),
                        DOC_PREFIX = c.String(nullable: false, maxLength: 30),
                        DOC_SEQ = c.Int(nullable: false),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DOC_NO)
                .Index(t => new { t.DOC_PREFIX, t.DOC_SEQ }, name: "DOC_UNIQUE_T_IDX1");

            var baseDir = AppDomain.CurrentDomain
              .BaseDirectory
              .Replace("\\bin", string.Empty) + "Data\\SQLScript\\";
            SqlFile(Path.Combine(baseDir, "015_SP_GenerateDocNum.sql"));

        }
        
        public override void Down()
        {
            DropStoredProcedure("SP_GenerateDocNum");
            DropIndex("dbo.DOC_UNIQUE_T", "DOC_UNIQUE_T_IDX1");
            DropTable("dbo.DOC_UNIQUE_T");
        }


    }
}
