/****** Object:  Table [dbo].[XXIF_CHP_P222_SUB_TRANSFER_ST]    Script Date: 2020/8/13 上午 11:54:40 ******/
CREATE TABLE [dbo].[XXIF_CHP_P222_SUB_TRANSFER_ST](
	[PROCESS_CODE] [nvarchar](20) NOT NULL,
	[SERVER_CODE] [nvarchar](20) NOT NULL,
	[BATCH_ID] [nvarchar](20) NOT NULL,
	[BATCH_LINE_ID] [bigint] NOT NULL,
	[STATUS_CODE] [nvarchar](1) NULL,
	[ERROR_MSG] [nvarchar](2000) NULL,
	[ORG_ID] [bigint] NOT NULL,
	[ORG_NAME] [nvarchar](240) NOT NULL,
	[ORGANIZATION_ID] [bigint] NOT NULL,
	[ORGANIZATION_CODE] [nvarchar](3) NOT NULL,
	[SHIPMENT_NUMBER] [nvarchar](30) NULL,
	[SUBINVENTORY_CODE] [nvarchar](20) NOT NULL,
	[LOCATOR_ID] [bigint] NULL,
	[LOCATOR_CODE] [nvarchar](30) NULL,
	[TRANSACTION_DATE] [datetime] NOT NULL,
	[TRANSACTION_TYPE_ID] [bigint] NOT NULL,
	[TRANSACTION_TYPE_NAME] [nvarchar](80) NOT NULL,
	[TRANSFER_ORG_ID] [bigint] NULL,
	[TRANSFER_ORG_NAME] [nvarchar](240) NULL,
	[TRANSFER_ORGANIZATION_ID] [bigint] NULL,
	[TRANSFER_ORGANIZATION_CODE] [nvarchar](3) NULL,
	[TRANSFER_SUBINVENTORY_CODE] [nvarchar](20) NULL,
	[TRANSFER_LOCATOR_ID] [bigint] NULL,
	[TRANSFER_LOCATOR_CODE] [nvarchar](30) NULL,
	[INVENTORY_ITEM_ID] [bigint] NOT NULL,
	[ITEM_NUMBER] [nvarchar](40) NOT NULL,
	[ITEM_DESCRIPTION] [nvarchar](240) NOT NULL,
	[TRANSACTION_UOM] [nvarchar](3) NOT NULL,
	[TRANSACTION_QUANTITY] [decimal](30, 10) NOT NULL,
	[PRIMARY_UOM] [nvarchar](3) NOT NULL,
	[PRIMARY_QUANTITY] [decimal](30, 10) NOT NULL,
	[SECONDARY_UOM_CODE] [nvarchar](3) NULL,
	[SECONDARY_QUANTITY] [decimal](30, 10) NULL,
	[ROLL_NUMBER] [nvarchar](80) NULL,
	[ROLL_QUANTITY] [decimal](30, 10) NULL,
	[ATTRIBUTE1] [nvarchar](150) NULL,
	[ATTRIBUTE2] [nvarchar](150) NULL,
	[ATTRIBUTE3] [nvarchar](150) NULL,
	[ATTRIBUTE4] [nvarchar](150) NULL,
	[ATTRIBUTE5] [nvarchar](150) NULL,
	[ATTRIBUTE6] [nvarchar](150) NULL,
	[ATTRIBUTE7] [nvarchar](150) NULL,
	[ATTRIBUTE8] [nvarchar](150) NULL,
	[ATTRIBUTE9] [nvarchar](150) NULL,
	[ATTRIBUTE10] [nvarchar](150) NULL,
	[ATTRIBUTE11] [nvarchar](150) NULL,
	[ATTRIBUTE12] [nvarchar](150) NULL,
	[ATTRIBUTE13] [nvarchar](150) NULL,
	[ATTRIBUTE14] [nvarchar](150) NULL,
	[ATTRIBUTE15] [nvarchar](150) NULL,
	[REQUEST_ID] [bigint] NULL,
	[CREATED_BY] [bigint] NULL,
	[CREATION_DATE] [datetime] NULL,
	[LAST_UPDATED_BY] [bigint] NULL,
	[LAST_UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_XXIF_CHP_P222_SUB_TRANSFER_ST] PRIMARY KEY CLUSTERED 
(
	[PROCESS_CODE] ASC,
	[SERVER_CODE] ASC,
	[BATCH_ID] ASC,
	[BATCH_LINE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]