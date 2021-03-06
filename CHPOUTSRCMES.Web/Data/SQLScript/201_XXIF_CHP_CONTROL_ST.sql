/****** Object:  Table [dbo].[XXIF_CHP_CONTROL_ST]    Script Date: 2020/8/13 上午 11:54:38 ******/
CREATE TABLE [dbo].[XXIF_CHP_CONTROL_ST](
	[PROCESS_CODE] [nvarchar](20) NOT NULL,
	[SERVER_CODE] [nvarchar](20) NOT NULL,
	[BATCH_ID] [nvarchar](20) NOT NULL,
	[ROW_NUM] [int] NOT NULL,
	[PROCESS_DATE] [datetime] NOT NULL,
	[REQUEST_ID] [bigint] NULL,
	[STATUS_CODE] [nvarchar](1) NULL,
	[ERROR_MSG] [nvarchar](2000) NULL,
	[SOA_PULLING_FLAG] [nvarchar](30) NULL,
	[SOA_ERROR_MSG] [nvarchar](2000) NULL,
	[SOA_PROCESS_CODE] [nvarchar](30) NULL,
	[CREATED_BY] [bigint] NOT NULL,
	[CREATION_DATE] [datetime] NOT NULL,
	[LAST_UPDATED_BY] [bigint] NOT NULL,
	[LAST_UPDATE_DATE] [datetime] NOT NULL,
	[LAST_UPDATE_LOGIN] [bigint] NULL,
 CONSTRAINT [PK_XXIF_CHP_CONTROL_ST] PRIMARY KEY CLUSTERED 
(
	[PROCESS_CODE] ASC,
	[SERVER_CODE] ASC,
	[BATCH_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]