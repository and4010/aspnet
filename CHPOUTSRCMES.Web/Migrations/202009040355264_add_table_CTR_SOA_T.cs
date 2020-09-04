namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_table_CTR_SOA_T : DbMigration
    {
        public override void Up()
        { 
            CreateTable(
                "dbo.CTR_SOA_T",
                c => new
                    {
                        CTR_HEADER_ID = c.Long(nullable: false),
                        PROCESS_CODE = c.String(nullable: false, maxLength: 20),
                        SERVER_CODE = c.String(nullable: false, maxLength: 20),
                        BATCH_ID = c.String(nullable: false, maxLength: 20),
                        CREATED_BY = c.String(nullable: false, maxLength: 128),
                        CREATION_DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.CTR_HEADER_ID, t.PROCESS_CODE, t.SERVER_CODE, t.BATCH_ID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CTR_SOA_T");
        }
    }
}
