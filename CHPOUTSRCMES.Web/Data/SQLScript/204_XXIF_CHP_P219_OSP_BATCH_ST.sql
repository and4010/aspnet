/****** Object:  Table [dbo].[XXIF_CHP_P219_OSP_BATCH_ST]    Script Date: 2020/8/13 上午 11:54:39 ******/
CREATE TABLE [dbo].[XXIF_CHP_P219_OSP_BATCH_ST](
	[PROCESS_CODE] [nvarchar](20) NOT NULL,
	[SERVER_CODE] [nvarchar](20) NOT NULL,
	[BATCH_ID] [nvarchar](20) NOT NULL,
	[BATCH_LINE_ID] [bigint] NOT NULL,
	[STATUS_CODE] [nvarchar](1) NULL,
	[ERROR_MSG] [nvarchar](2000) NULL,
	[PE_BATCH_ID] [bigint] NOT NULL,
	[BATCH_NO] [nvarchar](32) NOT NULL,
	[BATCH_TYPE] [nvarchar](3) NOT NULL,
	[BATCH_STATUS] [bigint] NOT NULL,
	[BATCH_STATUS_DESC] [nvarchar](80) NOT NULL,
	[ORG_ID] [bigint] NOT NULL,
	[ORG_NAME] [nvarchar](240) NOT NULL,
	[ORGANIZATION_ID] [bigint] NOT NULL,
	[ORGANIZATION_CODE] [nvarchar](3) NOT NULL,
	[DUE_DATE] [datetime] NOT NULL,
	[PLAN_START_DATE] [datetime] NOT NULL,
	[PLAN_CMPLT_DATE] [datetime] NOT NULL,
	[PE_CREATED_BY] [bigint] NOT NULL,
	[PE_CREATION_DATE] [datetime] NOT NULL,
	[PE_LAST_UPDATED_BY] [bigint] NOT NULL,
	[PE_LAST_UPDATE_DATE] [datetime] NOT NULL,
	[LINE_TYPE] [nvarchar](1) NOT NULL,
	[LINE_NO] [bigint] NOT NULL,
	[INVENTORY_ITEM_ID] [bigint] NOT NULL,
	[INVENTORY_ITEM_NUMBER] [nvarchar](40) NOT NULL,
	[BASIC_WEIGHT] [nvarchar](30) NULL,
	[SPECIFICATION] [nvarchar](40) NULL,
	[GRAIN_DIRECTION] [nvarchar](30) NULL,
	[ORDER_WEIGHT] [nvarchar](30) NULL,
	[REAM_WT] [nvarchar](30) NULL,
	[PACKING_TYPE] [nvarchar](30) NULL,
	[PLAN_QTY] [decimal](30, 10) NOT NULL,
	[WIP_PLAN_QTY] [decimal](30, 10) NOT NULL,
	[DTL_UOM] [nvarchar](3) NOT NULL,
	[HEADER_ID] [bigint] NULL,
	[ORDER_NUMBER] [bigint] NULL,
	[LINE_ID] [bigint] NULL,
	[LINE_NUMBER] [nvarchar](10) NULL,
	[CUSTOMER_ID] [bigint] NULL,
	[CUSTOMER_NUMBER] [nvarchar](30) NULL,
	[CUSTOMER_NAME] [nvarchar](240) NULL,
	[PR_NUMBER] [bigint] NOT NULL,
	[PR_LINE_NUMBER] [bigint] NOT NULL,
	[REQUISITION_LINE_ID] [bigint] NOT NULL,
	[PO_NUMBER] [bigint] NOT NULL,
	[PO_LINE_NUMBER] [bigint] NOT NULL,
	[PO_LINE_ID] [bigint] NOT NULL,
	[PO_UNIT_PRICE] [decimal](30, 10) NOT NULL,
	[PO_REVISION_NUM] [bigint] NOT NULL,
	[PO_STATUS] [nvarchar](25) NULL,
	[PO_VENDOR_NUM] [nvarchar](30) NULL,
	[OSP_REMARK] [nvarchar](240) NOT NULL,
	[SUBINVENTORY] [nvarchar](20) NOT NULL,
	[LOCATOR_ID] [bigint] NULL,
	[LOCATOR_CODE] [nvarchar](30) NULL,
	[RESERVATION_UOM_CODE] [nvarchar](3) NULL,
	[RESERVATION_QUANTITY] [decimal](30, 10) NULL,
	[LINE_CREATED_BY] [bigint] NOT NULL,
	[LINE_CREATION_DATE] [datetime] NOT NULL,
	[LINE_LAST_UPDATED_BY] [bigint] NOT NULL,
	[LINE_LAST_UPDATE_DATE] [datetime] NOT NULL,
	[TRANSACTION_QUANTITY] [decimal](30, 10) NOT NULL,
	[TRANSACTION_UOM] [nvarchar](3) NOT NULL,
	[PRIMARY_QUANTITY] [decimal](30, 10) NOT NULL,
	[PRIMARY_UOM] [nvarchar](3) NOT NULL,
	[SECONDARY_QUANTITY] [decimal](30, 10) NULL,
	[SECONDARY_UOM] [nvarchar](3) NULL,
	[ATTRIBUTE1] [nvarchar](240) NULL,
	[ATTRIBUTE2] [nvarchar](240) NULL,
	[ATTRIBUTE3] [nvarchar](240) NULL,
	[ATTRIBUTE4] [nvarchar](240) NULL,
	[ATTRIBUTE5] [nvarchar](240) NULL,
	[ATTRIBUTE6] [nvarchar](240) NULL,
	[ATTRIBUTE7] [nvarchar](240) NULL,
	[ATTRIBUTE8] [nvarchar](240) NULL,
	[ATTRIBUTE9] [nvarchar](240) NULL,
	[ATTRIBUTE10] [nvarchar](240) NULL,
	[ATTRIBUTE11] [nvarchar](240) NULL,
	[ATTRIBUTE12] [nvarchar](240) NULL,
	[ATTRIBUTE13] [nvarchar](240) NULL,
	[ATTRIBUTE14] [nvarchar](240) NULL,
	[ATTRIBUTE15] [nvarchar](240) NULL,
	[REQUEST_ID] [bigint] NULL,
	[CREATED_BY] [bigint] NOT NULL,
	[CREATION_DATE] [datetime] NOT NULL,
	[LAST_UPDATED_BY] [bigint] NOT NULL,
	[LAST_UPDATE_DATE] [datetime] NOT NULL,
	[STATUS] [varchar](1) NOT NULL,
 CONSTRAINT [PK_XXIF_CHP_P219_OSP_BATCH_ST] PRIMARY KEY CLUSTERED 
(
	[PROCESS_CODE] ASC,
	[SERVER_CODE] ASC,
	[BATCH_ID] ASC,
	[BATCH_LINE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]