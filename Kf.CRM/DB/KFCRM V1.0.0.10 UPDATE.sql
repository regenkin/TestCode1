--1
UPDATE [dbo].[CRM_Contact] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[CRM_contract] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[CRM_Customer] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[CRM_Follow] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[CRM_invoice] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[CRM_order] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[CRM_product] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[CRM_product_category] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[CRM_receive] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[hr_department] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[hr_employee] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[hr_position] SET [isDelete] = 0 WHERE [isDelete] is null
UPDATE [dbo].[hr_post] SET [isDelete] = 0 WHERE [isDelete] is null

--2
UPDATE CRM_product_category SET product_icon=REPLACE(product_category,'../','') 
UPDATE hr_department SET d_icon=REPLACE(d_icon,'../','') 
UPDATE Sys_App SET App_icon=REPLACE(App_icon,'../','') 
UPDATE Sys_Button SET Btn_icon=REPLACE(Btn_icon,'../','') 
UPDATE Sys_Menu SET Menu_icon=REPLACE(Menu_icon,'../','') 