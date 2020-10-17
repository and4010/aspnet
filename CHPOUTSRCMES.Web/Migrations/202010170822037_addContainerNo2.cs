namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addContainerNo2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TRF_INBOUND_PICKED_HT", "CONTAINER_NO", c => c.String(maxLength: 40));
            AddColumn("dbo.TRF_INBOUND_PICKED_T", "CONTAINER_NO", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TRF_INBOUND_PICKED_T", "CONTAINER_NO");
            DropColumn("dbo.TRF_INBOUND_PICKED_HT", "CONTAINER_NO");
        }
    }
}
