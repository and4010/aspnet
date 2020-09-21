namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class fixosp_tables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "ORDER_HEADER_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "ORDER_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "ORDER_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "CUSTOMER_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "RESERVATION_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "ORDER_HEADER_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "ORDER_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "ORDER_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "CUSTOMER_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "RESERVATION_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "CREATION_DATE", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "ORDER_HEADER_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "ORDER_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "ORDER_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "CUSTOMER_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "RESERVATION_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "LAST_UPDATE_BY", c => c.String(maxLength: 128));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "ORDER_HEADER_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "ORDER_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "ORDER_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "CUSTOMER_ID", c => c.Long());
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "RESERVATION_QUANTITY", c => c.Decimal(precision: 30, scale: 10));
            AlterColumn("dbo.OSP_ORG_T", "ORDER_HEADER_ID", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "ORDER_NUMBER", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "ORDER_LINE_ID", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "CUSTOMER_ID", c => c.Long());
            AlterColumn("dbo.OSP_ORG_T", "RESERVATION_QUANTITY", c => c.Decimal(precision: 30, scale: 10));

            var baseDir = AppDomain.CurrentDomain
              .BaseDirectory
              .Replace("\\bin", string.Empty) + "Data\\SQLScript\\";

            DropFunctionIfExists("DeliveryPickingList");
            SqlFile(Path.Combine(baseDir, "018_DeliveryPickingList.sql"));

            DropFunctionIfExists("InboundPickingList");
            SqlFile(Path.Combine(baseDir, "019_InboundPickingList.sql"));

            DropFunctionIfExists("OutboundPickingList");
            SqlFile(Path.Combine(baseDir, "020_OutboundPickingList.sql"));

        }
        
        public override void Down()
        {
            DropFunctionIfExists("DeliveryPickingList");

            DropFunctionIfExists("InboundPickingList");

            DropFunctionIfExists("OutboundPickingList");

            AlterColumn("dbo.OSP_ORG_T", "RESERVATION_QUANTITY", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_ORG_T", "CUSTOMER_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "ORDER_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "ORDER_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_ORG_T", "ORDER_HEADER_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "RESERVATION_QUANTITY", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "CUSTOMER_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "ORDER_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "ORDER_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_T", "ORDER_HEADER_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "LAST_UPDATE_BY", c => c.String());
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "RESERVATION_QUANTITY", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "CUSTOMER_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "ORDER_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "ORDER_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_OUT_HT", "ORDER_HEADER_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "CREATION_DATE", c => c.DateTime());
            AlterColumn("dbo.OSP_DETAIL_IN_T", "RESERVATION_QUANTITY", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "CUSTOMER_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "ORDER_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "ORDER_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_T", "ORDER_HEADER_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "RESERVATION_QUANTITY", c => c.Decimal(nullable: false, precision: 30, scale: 10));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "CUSTOMER_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "ORDER_LINE_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "ORDER_NUMBER", c => c.Long(nullable: false));
            AlterColumn("dbo.OSP_DETAIL_IN_HT", "ORDER_HEADER_ID", c => c.Long(nullable: false));
        }

        private void DropFunctionIfExists(string functionName)
        {
            Sql(
$@"IF EXISTS ( SELECT * FROM sysobjects WHERE id = object_id(N'{functionName}') AND xtype IN (N'FN', N'IF', N'TF') )
    DROP FUNCTION {functionName}");
        }
    }


}
