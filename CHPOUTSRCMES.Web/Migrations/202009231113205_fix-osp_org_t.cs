namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixosp_org_t : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OSP_ORG_T", "PR_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "PR_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "REQUISITION_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "PO_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "PO_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "PO_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "PO_UNIT_PRICE", c => c.Decimal(precision: 30, scale: 10));
            AlterColumn("dbo.OSP_ORG_T", "PO_REVISION_NUM", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "SUBINVENTORY", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OSP_ORG_T", "SUBINVENTORY", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.OSP_ORG_T", "PO_REVISION_NUM", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "PO_UNIT_PRICE", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_ORG_T", "PO_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "PO_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "PO_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "REQUISITION_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "PR_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "PR_NUMBER", c => c.Long(nullable: false));
        }
    }
}
