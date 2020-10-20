namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_ctr_soa_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CTR_SOA_T", "STATUS_CODE", c => c.String(maxLength: 1));
            AddColumn("dbo.CTR_SOA_T", "LAST_UPDATE_BY", c => c.String(maxLength: 128));
            AddColumn("dbo.CTR_SOA_T", "LAST_UPDATE_DATE", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CTR_SOA_T", "LAST_UPDATE_DATE");
            DropColumn("dbo.CTR_SOA_T", "LAST_UPDATE_BY");
            DropColumn("dbo.CTR_SOA_T", "STATUS_CODE");
        }
    }
}
