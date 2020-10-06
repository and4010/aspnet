namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixsoatables : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.OSP_SOA_DTL_S1_T", name: "OspSoaDtlS1Id", newName: "OSP_SOA_DTL_S1_ID");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S1_T", name: "OspSoaS1Id", newName: "OSP_SOA_S1_ID");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S2_T", name: "OspSoaDtlS2Id", newName: "OSP_SOA_DTL_S2_ID");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S2_T", name: "OspSoaS2Id", newName: "OSP_SOA_S2_ID");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S3_T", name: "OspSoaDtlS3Id", newName: "OSP_SOA_DTL_S3_ID");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S3_T", name: "OspSoaS3Id", newName: "OSP_SOA_S3_ID");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.OSP_SOA_DTL_S3_T", name: "OSP_SOA_S3_ID", newName: "OspSoaS3Id");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S3_T", name: "OSP_SOA_DTL_S3_ID", newName: "OspSoaDtlS3Id");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S2_T", name: "OSP_SOA_S2_ID", newName: "OspSoaS2Id");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S2_T", name: "OSP_SOA_DTL_S2_ID", newName: "OspSoaDtlS2Id");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S1_T", name: "OSP_SOA_S1_ID", newName: "OspSoaS1Id");
            RenameColumn(table: "dbo.OSP_SOA_DTL_S1_T", name: "OSP_SOA_DTL_S1_ID", newName: "OspSoaDtlS1Id");
        }
    }
}
