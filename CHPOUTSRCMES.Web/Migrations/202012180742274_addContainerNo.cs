namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addContainerNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.STOCK_HT", "CONTAINER_NO", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            DropColumn("dbo.STOCK_HT", "CONTAINER_NO");
        }
    }
}
