namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixosp_tables2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PR_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PR_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "REQUISITION_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_UNIT_PRICE", c => c.Decimal(precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_REVISION_NUM", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "CREATION_DATE", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PR_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PR_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "REQUISITION_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_UNIT_PRICE", c => c.Decimal(precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_REVISION_NUM", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PR_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PR_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "REQUISITION_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_UNIT_PRICE", c => c.Decimal(precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_REVISION_NUM", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PR_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PR_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "REQUISITION_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_LINE_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_UNIT_PRICE", c => c.Decimal(precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_REVISION_NUM", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_REVISION_NUM", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_UNIT_PRICE", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PO_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "REQUISITION_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PR_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "PR_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_REVISION_NUM", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_UNIT_PRICE", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PO_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "REQUISITION_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PR_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "PR_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_REVISION_NUM", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_UNIT_PRICE", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PO_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "REQUISITION_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PR_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "PR_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "CREATION_DATE", c => c.DateTime());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_REVISION_NUM", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_UNIT_PRICE", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PO_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "REQUISITION_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PR_LINE_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "PR_NUMBER", c => c.Long(nullable: false));
        }
    }
}
