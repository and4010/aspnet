namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addosptables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OSP_HEADER_MOD_T",
                c => new
                    {
                        OSP_HEADER_ID = c.Long(nullable: false),
                        ORG_OSP_HEADER_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.OSP_HEADER_ID, t.ORG_OSP_HEADER_ID });
            
            CreateTable(
                "dbo.OSP_SOA_DTL_S1_T",
                c => new
                    {
                        OSP_HEADER_ID = c.Long(nullable: false),
                        OspSoaDtlS1Id = c.Long(nullable: false, identity: true),
                        OspSoaS1Id = c.Long(nullable: false),
                        BATCH_LINE_ID = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.OspSoaDtlS1Id);
            
            CreateTable(
                "dbo.OSP_SOA_DTL_S2_T",
                c => new
                    {
                        OSP_HEADER_ID = c.Long(nullable: false),
                        OspSoaDtlS2Id = c.Long(nullable: false, identity: true),
                        OspSoaS2Id = c.Long(nullable: false),
                        BATCH_LINE_ID = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.OspSoaDtlS2Id);
            
            CreateTable(
                "dbo.OSP_SOA_DTL_S3_T",
                c => new
                    {
                        OSP_HEADER_ID = c.Long(nullable: false),
                        OspSoaDtlS3Id = c.Long(nullable: false, identity: true),
                        OspSoaS3Id = c.Long(nullable: false),
                        BATCH_LINE_ID = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.OspSoaDtlS3Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OSP_SOA_DTL_S3_T");
            DropTable("dbo.OSP_SOA_DTL_S2_T");
            DropTable("dbo.OSP_SOA_DTL_S1_T");
            DropTable("dbo.OSP_HEADER_MOD_T");
        }
    }
}
