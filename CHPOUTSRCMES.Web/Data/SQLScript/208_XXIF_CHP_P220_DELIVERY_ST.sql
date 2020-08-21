/****** Object:  Table [dbo].[XXIF_CHP_P220_DELIVERY_ST]    Script Date: 2020/8/13 上午 11:54:39 ******/
CREATE TABLE [dbo].[XXIF_CHP_P220_DELIVERY_ST](
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
	[TRIP_CAR] [nvarchar](240) NOT NULL,
	[TRIP_ID] [bigint] NOT NULL,
	[TRIP_NAME] [nvarchar](30) NOT NULL,
	[TRIP_ACTUAL_SHIP_DATE] [datetime] NOT NULL,
	[DELIVERY_ID] [bigint] NOT NULL,
	[DELIVERY_NAME] [nvarchar](30) NOT NULL,
	[CUSTOMER_ID] [bigint] NOT NULL,
	[CUSTOMER_NUMBER] [nvarchar](30) NOT NULL,
	[CUSTOMER_NAME] [nvarchar](500) NOT NULL,
	[CUSTOMER_LOCATION_CODE] [nvarchar](500) NOT NULL,
	[SHIP_CUSTOMER_ID] [bigint] NOT NULL,
	[SHIP_CUSTOMER_NUMBER] [nvarchar](30) NOT NULL,
	[SHIP_CUSTOMER_NAME] [nvarchar](500) NOT NULL,
	[SHIP_LOCATION_CODE] [nvarchar](500) NOT NULL,
	[FREIGHT_TERMS_NAME] [nvarchar](30) NULL,
	[ORDER_HEADER_ID] [bigint] NOT NULL,
	[ORDER_NUMBER] [bigint] NOT NULL,
	[ORDER_LINE_ID] [bigint] NOT NULL,
	[ORDER_SHIP_NUMBER] [nvarchar](240) NOT NULL,
	[DELIVERY_DETAIL_ID] [bigint] NOT NULL,
	[PACKING_TYPE] [nvarchar](30) NULL,
	[INVENTORY_ITEM_ID] [bigint] NOT NULL,
	[ITEM_NUMBER] [nvarchar](40) NOT NULL,
	[ITEM_DESCRIPTION] [nvarchar](240) NOT NULL,
	[PAPER_TYPE] [nvarchar](30) NOT NULL,
	[BASIC_WEIGHT] [nvarchar](30) NOT NULL,
	[SPECIFICATION] [nvarchar](30) NOT NULL,
	[GRAIN_DIRECTION] [nvarchar](30) NULL,
	[SUBINVENTORY_CODE] [nvarchar](10) NOT NULL,
	[LOCATOR_ID] [bigint] NULL,
	[LOCATOR_CODE] [nvarchar](30) NULL,
	[SRC_REQUESTED_QUANTITY] [decimal](30, 10) NOT NULL,
	[SRC_REQUESTED_QUANTITY_UOM] [nvarchar](3) NOT NULL,
	[REQUESTED_QUANTITY] [decimal](30, 10) NOT NULL,
	[REQUESTED_QUANTITY_UOM] [nvarchar](3) NOT NULL,
	[REQUESTED_QUANTITY2] [decimal](30, 10) NULL,
	[REQUESTED_QUANTITY_UOM2] [nvarchar](3) NULL,
	[BATCH_NO] [nvarchar](32) NULL,
	[INVENTORY_ITEM_NUMBER] [nvarchar](40) NULL,
	[REMARK] [nvarchar](240) NULL,
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
 CONSTRAINT [PK_XXIF_CHP_P220_DELIVERY_ST] PRIMARY KEY CLUSTERED 
(
	[PROCESS_CODE] ASC,
	[SERVER_CODE] ASC,
	[BATCH_ID] ASC,
	[BATCH_LINE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]