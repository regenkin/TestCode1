--sort
ALTER TABLE hr_position ALTER COLUMN position_order int
ALTER TABLE Param_SysParam ALTER COLUMN params_order int
ALTER TABLE Param_SysParam_Type ALTER COLUMN params_order int
ALTER TABLE Sys_Button ALTER COLUMN Btn_order int
ALTER TABLE Sys_role ALTER COLUMN RoleSort int

--tool_batch
CREATE TABLE [dbo].[tool_batch](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[batch_type] [varchar](50) NULL,
	[o_dep_id] [int] NULL,
	[o_dep] [varchar](250) NULL,
	[o_emp_id] [int] NULL,
	[o_emp] [varchar](250) NULL,
	[c_dep_id] [int] NULL,
	[c_dep] [varchar](250) NULL,
	[c_emp_id] [int] NULL,
	[c_emp] [varchar](250) NULL,
	[b_count] [int] NULL,
	[create_id] [int] NULL,
	[create_name] [varchar](250) NULL,
	[create_date] [datetime] NULL,
 CONSTRAINT [PK_tool_batch] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--menu
TRUNCATE TABLE [dbo].[Sys_Menu]

SET IDENTITY_INSERT [dbo].[Sys_Menu] ON
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (1, N'个人工作', 0, N'无', 1, N'', N'images/icon/37.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (2, N'我的便签', 1, N'个人工作', 1, N'personal/personal/notes.aspx', N'images/icon/33.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (3, N'客户管理', 0, N'无', 2, N'', N'images/icon/37.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (4, N'客户列表', 3, N'客户管理', 2, N'crm/Customer/customer.aspx', N'images/icon/61.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (5, N'联系人管理', 3, N'客户管理', 2, N'crm/Customer/customer_contact.aspx', N'images/icon/38.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (6, N'跟进管理', 3, N'客户管理', 2, N'crm/Customer/customer_follow.aspx', N'images/icon/3.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (7, N'日程安排', 1, N'个人工作', 1, N'personal/personal/Calendar.aspx', N'images/icon/29.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (8, N'邮箱', 0, N'无', -1, N'', N'images/icon/48.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (9, N'收件箱', 8, N'邮箱', 1, N'', N'images/icon/47.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (10, N'草稿箱', 8, N'邮箱', 1, N'', N'images/icon/48.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (11, N'已发送', 8, N'邮箱', 1, N'', N'images/icon/43.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (12, N'信息中心', 0, N'无', 1, N'', N'images/icon/56.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (13, N'新闻', 12, N'信息中心', 1, N'personal/message/news.aspx', N'images/icon/57.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (14, N'公告', 12, N'信息中心', 1, N'personal/message/notice.aspx', N'images/icon/58.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (15, N'论坛', -1, N'信息中心', 1, N'', N'images/icon/62.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (16, N'知识库', -1, N'信息中心', 1, N'', N'images/icon/68.png', NULL, 40, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (17, N'人事架构', 0, N'无', 3, N'', N'images/icon/37.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (18, N'人事资料', -1, N'无', 3, N'', N'images/icon/37.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (19, N'人事培训', -1, N'无', 3, N'', N'images/icon/37.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (20, N'人事报表', 0, N'无', -1, N'', N'images/icon/37.png', NULL, 40, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (21, N'组织架构', 17, N'人事架构', 3, N'hr/hr_department.aspx', N'images/icon/67.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (22, N'职务管理', 17, N'人事架构', 3, N'hr/hr_position.aspx', N'images/icon/68.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (23, N'岗位管理', 17, N'人事架构', 3, N'hr/hr_post.aspx', N'images/icon/49.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (24, N'员工管理', 17, N'人事架构', 3, N'hr/hr_employee.aspx', N'images/icon/37.png', NULL, 40, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (25, N'人事异动', -1, N'人事资料', 3, N'', N'images/icon/43.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (26, N'劳动合同', 19, N'人事资料', 3, N'hr/hr_contract.aspx', N'images/icon/24.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (27, N'社保管理', -1, N'人事资料', 3, N'', N'images/icon/29.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (28, N'培训记录', -1, N'人事培训', 3, N'', N'images/icon/24.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (29, N'讲师团队', -1, N'人事培训', 3, N'', N'images/icon/38.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (30, N'系统管理', 0, N'无', 6, N'', N'images/icon/77.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (31, N'角色授权', 30, N'系统管理', 6, N'System/Sys_role.aspx', N'images/icon/70.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (32, N'日志管理', 30, N'系统管理', 6, N'system/sys_log.aspx', N'images/icon/51.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (33, N'合同订单', 0, N'无', 2, N'', N'images/icon/5.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (34, N'订单管理', 33, N'销售管理', 2, N'crm/sale/Order.aspx', N'images/icon/27.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (35, N'合同管理', 33, N'销售管理', 2, N'crm/sale/contract.aspx', N'images/icon/22.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (36, N'收款管理', 45, N'财务管理', 2, N'crm/finance/receive.aspx', N'images/icon/39.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (37, N'开票管理', 45, N'财务管理', 2, N'crm/finance/invoice.aspx', N'images/icon/28.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (38, N'产品管理', 0, N'无', 2, N'', N'images/icon/67.png', NULL, 40, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (39, N'产品列表', 38, N'产品管理', 2, N'crm/Product/product.aspx', N'images/icon/67.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (40, N'数据年报', 0, N'无', 5, N'', N'images/icon/53.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (41, N'客户统计年报', 40, N'CRM报表中心', 5, N'reportform/crm/CRM_report_year.aspx', N'images/icon/53.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (42, N'跟进统计年报', 40, N'CRM报表中心', 5, N'reportform/crm/Follow_report_year.aspx', N'images/icon/54.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (43, N'参数配置', 30, N'系统管理', 6, N'system/Param_SysParam.aspx', N'images/icon/77.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (44, N'产品类别', 38, N'产品管理', 2, N'crm/Product/product_category.aspx', N'images/icon/82.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (45, N'财务管理', 0, N'无', 2, N'', N'images/icon/56.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (46, N'CRM回收站', 0, N'无', 4, N'', N'images/icon/12.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (47, N'客户回收', 46, N'CRM回收站', 4, N'toolbar/recycle/crm/customer.aspx', N'images/icon/94.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (48, N'联系人回收', 46, N'CRM回收站', -1, N'toolbar/recycle/crm/contact.aspx', N'images/icon/37.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (49, N'跟进回收', 46, N'CRM回收站', -1, N'toolbar/recycle/crm/follow.aspx', N'images/icon/3.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (50, N'订单回收', 46, N'CRM回收站', -1, N'toolbar/recycle/crm/order.aspx', N'images/icon/27.png', NULL, 40, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (51, N'合同回收', 46, N'CRM回收站', -1, N'toolbar/recycle/crm/contract.aspx', N'images/icon/22.png', NULL, 50, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (52, N'收款单回收', 46, N'CRM回收站', -1, N'toolbar/recycle/crm/receive.aspx', N'images/icon/39.png', NULL, 60, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (53, N'发票回收', 46, N'CRM回收站', -1, N'toolbar/recycle/crm/invoice.aspx', N'images/icon/28.png', NULL, 70, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (54, N'产品类别回收', 46, N'CRM回收站', -1, N'toolbar/recycle/crm/product_category.aspx', N'images/icon/81.png', NULL, 80, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (55, N'产品回收', 46, N'CRM回收站', -1, N'toolbar/recycle/crm/product.aspx', N'images/icon/67.png', NULL, 90, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (56, N'收款流水', 45, N'财务管理', 2, N'crm/finance/receive_list.aspx', N'images/icon/26.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (57, N'发票列表', 45, N'财务管理', 2, N'crm/finance/invoice_list.aspx', N'images/icon/24.png', NULL, 40, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (58, N'批量操作', 0, N'无', 4, N'', N'images/icon/37.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (59, N'按钮管理', 60, N'配置信息', 6, N'system/sys_button.aspx', N'images/icon/85.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (60, N'配置信息', 0, N'无', -1, N'', N'images/icon/37.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (61, N'系统信息', 61, N'配置信息', 6, N'system/sys_info.aspx', N'images/icon/49.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (62, N'应收与未收', 45, N'财务管理', 2, N'crm/finance/receiveanduncollected.aspx', N'/images/icon/54.png', NULL, 50, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (63, N'同比与环比', 0, N'无', 5, N'', N'images/icon/59.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (64, N'【客户】新增', 63, N'同比与环比', 5, N'reportform/Compared/customer_add.aspx', N'images/icon/37.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (65, N'【客户】类型', 63, N'同比与环比', 5, N'reportform/Compared/customer_type.aspx', N'images/icon/33.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (66, N'【客户】级别', 63, N'同比与环比', 5, N'reportform/Compared/customer_level.aspx', N'images/icon/82.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (67, N'【客户】来源', 63, N'同比与环比', 5, N'reportform/Compared/customer_source.aspx', N'images/icon/83.png', NULL, 40, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (68, N'【客户】跟进', 63, N'同比与环比', 5, N'reportform/Compared/customer_follow.aspx', N'images/icon/81.png', NULL, 50, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (69, N'【员工】客户新增', 63, N'同比与环比', 5, N'reportform/Compared/emp_customer_add.aspx', N'images/icon/37.png', NULL, 60, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (70, N'【员工】客户跟进', 63, N'同比与环比', 5, N'reportform/Compared/emp_customer_follow.aspx', N'images/icon/38.png', NULL, 70, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (71, N'【员工】客户成交', 63, N'同比与环比', 5, N'reportform/Compared/emp_customer_contract.aspx', N'images/icon/94.png', NULL, 80, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (72, N'员工分析', 0, N'无', 5, N'', N'images/icon/93.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (73, N'【员工】客户新增统计', 72, N'员工分析', 5, N'reportform/emp/customer_add.aspx', N'images/icon/37.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (74, N'【员工】客户跟进统计', 72, N'员工分析', 5, N'reportform/emp/customer_follow.aspx', N'images/icon/38.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (75, N'【员工】客户成交统计', 72, N'员工分析', 5, N'reportform/emp/customer_contract.aspx', N'images/icon/94.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (76, N'目录管理', 60, N'配置信息', 6, N'system/sys_menu.aspx', N'images/icon/81.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (77, N'收件箱', 77, N'邮件', -1, N'personal/Mail/Mail_inbox.aspx', N'images/icon/39.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (78, N'已发送', 77, N'邮件', -1, N'personal/Mail/mail_send.aspx', N'images/icon/40.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (79, N'垃圾箱', 77, N'邮件', -1, N'personal/Mail/mail_dustbin.aspx', N'images/icon/4.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (80, N'批量转客户', 58, N'人事管理回收站', 4, N'toolbar/batch/batch.aspx', N'images/icon/64.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (81, N'职务回收', 58, N'人事管理回收站', -1, N'toolbar/Recycle/HR/hr_position.aspx', N'images/icon/68.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (82, N'岗位回收', 58, N'人事管理回收站', -1, N'toolbar/Recycle/HR/hr_post.aspx', N'images/icon/49.png', NULL, 30, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (83, N'员工回收', 58, N'人事管理回收站', -1, N'toolbar/Recycle/HR/hr_employee.aspx', N'images/icon/37.png', NULL, 40, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (84, N'城市管理', 30, N'系统管理', 6, N'system/Param_City.aspx', N'images/icon/64.png', NULL, 40, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (85, N'系统信息', 0, N'无', 6, N'', N'images/icon/77.png', NULL, 20, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (86, N'系统信息', 85, N'系统信息', 6, N'system/sys_info.aspx', N'images/icon/77.png', NULL, 10, N'sys')
INSERT [dbo].[Sys_Menu] ([Menu_id], [Menu_name], [parentid], [parentname], [App_id], [Menu_url], [Menu_icon], [Menu_handler], [Menu_order], [Menu_type]) VALUES (87, N'版本信息', 85, N'系统信息', 6, N'system/sys_version.aspx', N'images/icon/78.png', NULL, 20, N'sys')
SET IDENTITY_INSERT [dbo].[Sys_Menu] OFF

--button
TRUNCATE TABLE [dbo].[Sys_Button]
SET IDENTITY_INSERT [dbo].[Sys_Button] ON
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (1, N'新增', N'images/icon/11.png', N'add()', 76, N'目录管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (2, N'修改', N'images/icon/33.png', N'edit()', 76, N'目录管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (3, N'删除', N'images/icon/12.png', N'del()', 76, N'目录管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (4, N'新增', N'images/icon/11.png', N'add()', 59, N'按钮管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (5, N'修改', N'images/icon/33.png', N'edit()', 59, N'按钮管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (6, N'删除', N'images/icon/12.png', N'del()', 59, N'按钮管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (7, N'批量新增', N'images/icon/69.png', N'batch()', 59, N'按钮管理', N'40')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (11, N'新增', N'images/icon/11.png', N'add()', 14, N'公告', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (12, N'修改', N'images/icon/33.png', N'edit()', 14, N'公告', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (13, N'删除', N'images/icon/12.png', N'del()', 14, N'公告', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (14, N'删除', N'images/icon/12.png', N'del()', 4, N'客户列表', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (15, N'新增', N'images/icon/11.png', N'add()', 4, N'客户列表', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (16, N'修改', N'images/icon/33.png', N'edit()', 4, N'客户列表', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (17, N'新增跟进', N'images/icon/11.png', N'addfollow()', 6, N'跟进管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (18, N'修改跟进', N'images/icon/33.png', N'editfollow()', 6, N'跟进管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (19, N'删除跟进', N'images/icon/12.png', N'delfollow()', 6, N'跟进管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (20, N'删除', N'images/icon/12.png', N'del()', 21, N'组织架构', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (21, N'新增', N'images/icon/11.png', N'add()', 21, N'组织架构', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (22, N'修改', N'images/icon/33.png', N'edit()', 21, N'组织架构', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (23, N'新增', N'images/icon/11.png', N'add()', 22, N'职务管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (24, N'修改', N'images/icon/33.png', N'edit()', 22, N'职务管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (25, N'删除', N'images/icon/12.png', N'del()', 22, N'职务管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (26, N'删除', N'images/icon/12.png', N'del()', 23, N'岗位管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (27, N'修改', N'images/icon/33.png', N'edit()', 23, N'岗位管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (28, N'新增', N'images/icon/11.png', N'add()', 23, N'岗位管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (29, N'新增', N'images/icon/11.png', N'add()', 24, N'员工管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (30, N'删除', N'images/icon/12.png', N'del()', 24, N'员工管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (31, N'修改', N'images/icon/33.png', N'edit()', 24, N'员工管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (32, N'新增', N'images/icon/11.png', N'add()', 31, N'角色授权', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (33, N'删除', N'images/icon/12.png', N'del()', 31, N'角色授权', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (34, N'修改', N'images/icon/33.png', N'edit()', 31, N'角色授权', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (35, N'操作权限', N'images/icon/91.png', N'authorized()', 31, N'角色授权', N'40')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (36, N'包含人员', N'images/icon/37.png', N'role_emp()', 31, N'角色授权', N'60')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (37, N'数据权限', N'images/icon/92.png', N'data_authorized()', 31, N'角色授权', N'50')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (38, N'新增', N'images/icon/11.png', N'add()', 5, N'联系人管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (39, N'修改', N'images/icon/33.png', N'edit()', 5, N'联系人管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (40, N'删除', N'images/icon/12.png', N'del()', 5, N'联系人管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (41, N'删除', N'images/icon/12.png', N'del()', 34, N'订单管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (42, N'新增', N'images/icon/11.png', N'add()', 34, N'订单管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (43, N'修改', N'images/icon/33.png', N'edit()', 34, N'订单管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (44, N'修改', N'images/icon/33.png', N'edit()', 35, N'合同管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (45, N'新增', N'images/icon/11.png', N'add()', 35, N'合同管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (46, N'删除', N'images/icon/12.png', N'del()', 35, N'合同管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (47, N'新增', N'images/icon/11.png', N'add()', 36, N'收款管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (48, N'删除', N'images/icon/12.png', N'del()', 36, N'收款管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (49, N'修改', N'images/icon/33.png', N'edit()', 36, N'收款管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (50, N'新增', N'images/icon/11.png', N'add()', 37, N'开票管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (51, N'删除', N'images/icon/12.png', N'del()', 37, N'开票管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (52, N'修改', N'images/icon/33.png', N'edit()', 37, N'开票管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (53, N'修改', N'images/icon/33.png', N'edit()', 44, N'产品类别', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (54, N'删除', N'images/icon/12.png', N'del()', 44, N'产品类别', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (55, N'新增', N'images/icon/11.png', N'add()', 44, N'产品类别', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (56, N'新增', N'images/icon/11.png', N'add()', 39, N'产品列表', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (57, N'修改', N'images/icon/33.png', N'edit()', 39, N'产品列表', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (58, N'删除', N'images/icon/12.png', N'del()', 39, N'产品列表', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (59, N'恢复', N'images/icon/2.png', N'regain()', 47, N'客户回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (60, N'彻底删除', N'images/icon/50.png', N'del()', 47, N'客户回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (61, N'恢复', N'images/icon/2.png', N'regain()', 48, N'联系人回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (62, N'彻底删除', N'images/icon/50.png', N'del()', 48, N'联系人回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (63, N'恢复', N'images/icon/2.png', N'regain()', 49, N'跟进回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (64, N'彻底删除', N'images/icon/50.png', N'del()', 49, N'跟进回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (65, N'恢复', N'images/icon/2.png', N'regain()', 50, N'订单回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (66, N'彻底删除', N'images/icon/50.png', N'del()', 50, N'订单回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (67, N'恢复', N'images/icon/2.png', N'regain()', 51, N'合同回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (68, N'彻底删除', N'images/icon/50.png', N'del()', 51, N'合同回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (69, N'恢复', N'images/icon/2.png', N'regain()', 52, N'收款单回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (70, N'彻底删除', N'images/icon/50.png', N'del()', 52, N'收款单回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (71, N'恢复', N'images/icon/2.png', N'regain()', 53, N'发票回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (72, N'彻底删除', N'images/icon/50.png', N'del()', 53, N'发票回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (73, N'恢复', N'images/icon/2.png', N'regain()', 54, N'产品类别回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (74, N'彻底删除', N'images/icon/50.png', N'del()', 54, N'产品类别回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (75, N'恢复', N'images/icon/2.png', N'regain()', 55, N'产品回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (76, N'彻底删除', N'images/icon/50.png', N'del()', 55, N'产品回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (77, N'批量转客户', N'images/icon/2.png', N'batch_cus()', 80, N'组织架构回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (78, N'批量转订单', N'images/icon/50.png', N'batch_order()', 80, N'组织架构回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (79, N'恢复', N'images/icon/2.png', N'regain()', 81, N'职务回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (80, N'彻底删除', N'images/icon/50.png', N'del()', 81, N'职务回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (81, N'恢复', N'images/icon/2.png', N'regain()', 82, N'岗位回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (82, N'彻底删除', N'images/icon/50.png', N'del()', 82, N'岗位回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (83, N'恢复', N'images/icon/2.png', N'regain()', 83, N'员工回收', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (84, N'彻底删除', N'images/icon/50.png', N'del()', 83, N'员工回收', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (85, N'修改', N'images/icon/33.png', N'edit()', 43, N'参数配置', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (86, N'删除', N'images/icon/12.png', N'del()', 43, N'参数配置', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (87, N'新增', N'images/icon/11.png', N'add()', 43, N'参数配置', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (88, N'新增', N'images/icon/11.png', N'add()', 84, N'城市管理', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (89, N'修改', N'images/icon/33.png', N'edit()', 84, N'城市管理', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (90, N'删除', N'images/icon/12.png', N'del()', 84, N'城市管理', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (92, N'修改密码', N'images/icon/36.png', N'changepwd()', 24, N'员工管理', N'40')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (93, N'新增', N'images/icon/11.png', N'add()', 13, N'新闻', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (94, N'修改', N'images/icon/33.png', N'edit()', 13, N'新闻', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (95, N'删除', N'images/icon/12.png', N'del()', 13, N'新闻', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (96, N'新增', N'images/icon/11.png', N'add()', -1, N'客户列表', N'10')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (97, N'删除', N'images/icon/12.png', N'del()', -1, N'客户列表', N'30')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (98, N'修改', N'images/icon/33.png', N'edit()', -1, N'客户列表', N'20')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (99, N'导入', N'images/icon/4.png', N'toimport()', 4, N'客户列表', N'40')
INSERT [dbo].[Sys_Button] ([Btn_id], [Btn_name], [Btn_icon], [Btn_handler], [Menu_id], [Menu_name], [Btn_order]) VALUES (100, N'导出', N'images/icon/43.png', N'toexport()', 4, N'客户列表', N'50')
SET IDENTITY_INSERT [dbo].[Sys_Button] OFF

--sys_info
CREATE TABLE [dbo].[sys_info](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sys_key] [varchar](50) NULL,
	[sys_value] [varchar](max) NULL,
 CONSTRAINT [PK_sys_info] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[sys_info] ON
INSERT [dbo].[sys_info] ([id], [sys_key], [sys_value]) VALUES (1, N'sys_guid', NEWID())
INSERT [dbo].[sys_info] ([id], [sys_key], [sys_value]) VALUES (2, N'sys_name', N'小黄豆软件集团')
INSERT [dbo].[sys_info] ([id], [sys_key], [sys_value]) VALUES (3, N'sys_logo', N'images/logo/logo.png')
INSERT [dbo].[sys_info] ([id], [sys_key], [sys_value]) VALUES (4, N'sys_version', N'v1.15.0.1')
INSERT [dbo].[sys_info] ([id], [sys_key], [sys_value]) VALUES (5, N'sys_vinfo', NULL)
SET IDENTITY_INSERT [dbo].[sys_info] OFF

--contract_attachment
CREATE TABLE [dbo].[CRM_contract_attachment](
	[contract_id] [int] NULL,
	[page_id] [varchar](250) NULL,
	[file_id] [varchar](250) NULL,
	[file_name] [varchar](250) NULL,
	[real_name] [varchar](250) NULL,
	[file_size] [int] NULL,
	[create_id] [int] NULL,
	[create_name] [varchar](250) NULL,
	[create_date] [datetime] NULL
) ON [PRIMARY]
GO


