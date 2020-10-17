namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addContainerNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.STOCK_T", "CONTAINER_NO", c => c.String(maxLength: 40));
            AddColumn("dbo.TRF_REASON_HEADER_T", "CTR_HEADER_ID", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TRF_REASON_HEADER_T", "CTR_HEADER_ID");
            DropColumn("dbo.STOCK_T", "CONTAINER_NO");
        }
    }
}
