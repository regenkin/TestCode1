
/****** Object:  Table [dbo].[dt_article_link]    Script Date: 06/23/2017 08:41:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dt_article_link]') AND type in (N'U'))
DROP TABLE [dbo].[dt_article_link]
GO

USE [qds157513325_db]
GO

/****** Object:  Table [dbo].[dt_article_link]    Script Date: 06/23/2017 08:41:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[dt_article_link](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[site_id] [int] NOT NULL,
	[channel_id] [int] NOT NULL,
	[article_id] [int] NOT NULL,
	[link_site_id] [int] NOT NULL,
	[link_channel_id] [int] NOT NULL,
	[link_article_id] [int] NOT NULL,
	[link_category_id] [int] NOT NULL,
	[add_time] [datetime] NOT NULL,
 CONSTRAINT [PK_dt_article_link_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'dt_article_link', @level2type=N'COLUMN',@level2name=N'article_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联内容通道Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'dt_article_link', @level2type=N'COLUMN',@level2name=N'link_channel_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联内容Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'dt_article_link', @level2type=N'COLUMN',@level2name=N'link_article_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联内容类别Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'dt_article_link', @level2type=N'COLUMN',@level2name=N'link_category_id'
GO


