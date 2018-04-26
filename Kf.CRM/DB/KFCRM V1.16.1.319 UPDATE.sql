--修复1.16.0.316版本SQL执行出错
IF ( select count (*) from syscolumns where id = object_id('param_city') and name='city_order' ) = 0 
	ALTER TABLE [dbo].[Param_City] ADD City_order int 
GO

UPDATE Param_City SET City_order = id where City_order IS NULL
GO

--修复1.16.0.316版本category字段错误
IF ( select count (*) from syscolumns where id = object_id('CRM_product') and name='catery_id' ) > 0
	EXEC sp_rename'[dbo].[CRM_product].catery_id','category_id','column';
GO

IF ( select count (*) from syscolumns where id = object_id('CRM_product') and name='catery_name' ) > 0
	EXEC sp_rename '[dbo].[CRM_product].catery_name','category_name','column';
GO

IF ( select count (*) from syscolumns where id = object_id('CRM_product_catery') ) > 0 
	EXEC sp_rename   'CRM_product_catery' ,'CRM_product_category';
GO

IF ( select count (*) from syscolumns where id = object_id('CRM_product_category') and name='product_catery' ) > 0
	EXEC sp_rename '[dbo].[CRM_product_category].product_catery','product_category','column';
GO

IF ( select count (*) from syscolumns where id = object_id('Personal_Calendar') and name='Catery' ) > 0
	EXEC sp_rename '[dbo].[Personal_Calendar].Catery','Category','column';
GO

UPDATE Sys_Menu SET
	[Menu_url] = N'crm/Product/product_category.aspx'
WHERE Menu_id = 44
GO

--销售漏斗

IF (select count (*) from sys_menu where Menu_id = 88 ) = 0
BEGIN
	set identity_insert sys_menu on
	
		INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (88, N'销售漏斗', 0, N'无', 5, N'', N'../images/icon/4.png', NULL, 15, N'sys')
		INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (89, N'销售漏斗', 88, N'销售漏斗', 5, N'reportform/funnel/CRM_Report_funnel.aspx', N'../images/icon/4.png', NULL, 20, N'sys')

	set identity_insert sys_menu off
END

