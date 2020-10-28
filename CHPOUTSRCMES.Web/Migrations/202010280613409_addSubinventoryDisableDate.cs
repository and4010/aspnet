namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSubinventoryDisableDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SUBINVENTORY_TMP_T", "SUBINVENTORY_DISABLE_DATE", c => c.DateTime());
            AddColumn("dbo.SUBINVENTORY_T", "SUBINVENTORY_DISABLE_DATE", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SUBINVENTORY_T", "SUBINVENTORY_DISABLE_DATE");
            DropColumn("dbo.SUBINVENTORY_TMP_T", "SUBINVENTORY_DISABLE_DATE");
        }
    }
}
