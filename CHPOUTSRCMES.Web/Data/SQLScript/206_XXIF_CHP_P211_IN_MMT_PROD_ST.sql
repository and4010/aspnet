/****** Object:  Table [dbo].[XXIF_CHP_P211_IN_MMT_PROD_ST]    Script Date: 2020/8/13 上午 11:54:39 ******/
CREATE TABLE [dbo].[XXIF_CHP_P211_IN_MMT_PROD_ST](
	[PROCESS_CODE] [nvarchar](20) NOT NULL,
	[SERVER_CODE] [nvarchar](20) NOT NULL,
	[BATCH_ID] [nvarchar](20) NOT NULL,
	[BATCH_LINE_ID] [bigint] NOT NULL,
	[STATUS_CODE] [nvarchar](1) NULL,
	[ERROR_MSG] [nvarchar](2000) NULL,
	[ORGCODE] [nvarchar](3) NOT NULL,
	[RXID] [bigint] NOT NULL,
	[PREVIOUS_RXID] [bigint] NULL,
	[BATCH_NO] [nvarchar](32) NOT NULL,
	[MACHINE_NO] [nvarchar](5) NULL,
	[ITEM_NO] [nvarchar](40) NOT NULL,
	[SUBINVENTORY_CODE] [nvarchar](10) NOT NULL,
	[LOCATOR] [nvarchar](50) NULL,
	[TRANSACTION_QUANTITY] [decimal](30, 10) NULL,
	[TRANSACTION_UOM] [nvarchar](3) NULL,
	[SECONDARY_TRANSACTION_QUANTITY] [decimal](30, 10) NULL,
	[SECONDARY_UOM_CODE] [nvarchar](3) NULL,
	[TRANSACTION_DATE] [datetime] NOT NULL,
	[LOT_NUMBER] [nvarchar](80) NULL,
	[LOT_ATTRIBUTE1] [nvarchar](20) NULL,
	[LOT_ATTRIBUTE2] [nvarchar](20) NULL,
	[LOT_ATTRIBUTE3] [nvarchar](20) NULL,
	[LOT_ATTRIBUTE4] [float] NULL,
	[LOT_ATTRIBUTE5] [float] NULL,
	[LOT_ATTRIBUTE6] [nvarchar](20) NULL,
	[LOT_ATTRIBUTE7] [nvarchar](20) NULL,
	[LOT_ATTRIBUTE8] [nvarchar](20) NULL,
	[LOT_ATTRIBUTE9] [nvarchar](20) NULL,
	[LOT_ATTRIBUTE10] [nvarchar](20) NULL,
	[ORDER_NUMBER] [bigint] NULL,
	[LINE_NUMBER] [nvarchar](20) NULL,
	[STATUS] [nvarchar](1) NOT NULL,
	[RESERVATION_ID] [bigint] NULL,
	[TRANSACTION_ID] [bigint] NULL,
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
	[LAST_UPDATE_LOGIN] [bigint] NULL,
 CONSTRAINT [PK_XXIF_CHP_P211_IN_MMT_PROD_ST] PRIMARY KEY CLUSTERED 
(
	[PROCESS_CODE] ASC,
	[SERVER_CODE] ASC,
	[BATCH_ID] ASC,
	[BATCH_LINE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]