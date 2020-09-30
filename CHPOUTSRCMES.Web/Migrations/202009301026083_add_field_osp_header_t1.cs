namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_field_osp_header_t1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OSP_HEADER_T", "SRC_BATCH_NO", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OSP_HEADER_T", "SRC_BATCH_NO", c => c.String(nullable: false, maxLength: 32));
        }
    }
}
