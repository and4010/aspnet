namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.USER_T", "Flag", c => c.String(maxLength: 1));
            AddColumn("dbo.USER_T", "Status", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.USER_T", "Status");
            DropColumn("dbo.USER_T", "Flag");
        }
    }
}
