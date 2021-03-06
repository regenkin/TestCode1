/****** Object:  Table [dbo].[Users]    Script Date: 04/14/2011 21:42:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Key] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Nickname] [nvarchar](256) NOT NULL,
	[Type] [int] NOT NULL,
	[Password] [nvarchar](256) NOT NULL,
	[EMail] [nvarchar](256) NOT NULL,
	[InviteCode] [nvarchar](256) NULL,
	[UpperName] [nvarchar](256) NOT NULL,
	[MsgFileLimit] [int] NULL,
	[MsgImageLimit] [int] NULL,
	[AcceptStrangerIM] [int] NULL,
	[IsTemp] [int] NULL,
	[DiskSize] [int] NULL,
	[RegisterTime] [datetime] NOT NULL,
	[HomePage] [nvarchar](256) NULL,
	[HeadIMG] [nvarchar](512) NOT NULL,
	[Remark] [text] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UpperName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRelationship]    Script Date: 04/14/2011 21:42:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRelationship](
	[HostKey] [int] NOT NULL,
	[GuestKey] [int] NOT NULL,
	[Relationship] [int] NOT NULL,
	[RenewTime] [datetime] NOT NULL,
	[Key] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Role]    Script Date: 04/14/2011 21:42:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Role](
	[Key] [int] IDENTITY(1,1) NOT NULL,
	[UserKey] [int] NOT NULL,
	[RoleKey] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 04/14/2011 21:42:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Key] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 04/14/2011 21:42:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[Key] [int] NOT NULL,
	[CreatedTime] [datetime] NULL,
	[Receiver] [int] NULL,
	[Sender] [int] NULL,
	[Content] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Validate]    Script Date: 04/14/2011 21:42:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Validate](@name nvarchar(256), @password nvarchar(256))
as
begin
	select * from Users where UpperName=upper(@name) and Password=@password
end
GO
/****** Object:  StoredProcedure [dbo].[GetUserRoles]    Script Date: 04/14/2011 21:42:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetUserRoles](@name nvarchar(256))
as
begin
	select r.Name as RoleName 
	from Users u,User_Role ur,Roles r 
	where u.UpperName=upper(@name) and u.[Key]=ur.UserKey and ur.RoleKey=r.[Key]
end
GO
/****** Object:  StoredProcedure [dbo].[GetRelationship]    Script Date: 04/14/2011 21:42:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetRelationship](@account1 nvarchar(256), @account2 nvarchar(256))
as
begin
	select *
	from Users host,Users guest,UserRelationship r
	where host.UpperName=upper(@account1) and guest.UpperName=upper(@account2) and r.HostKey=host.[Key] and r.GuestKey=guest.[Key]
end
GO
/****** Object:  StoredProcedure [dbo].[GetGroupManagers]    Script Date: 04/14/2011 21:42:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetGroupManagers](@name nvarchar(256))
as
begin
	select 
		guest.Name as Name
	from 
		UserRelationship r,
		Users host,
		Users guest
	where 
		r.Relationship=2 and
		r.HostKey=host.[Key] and
		r.GuestKey=guest.[Key] and
		host.UpperName=UPPER(@name)
end
GO
/****** Object:  StoredProcedure [dbo].[GetFriends]    Script Date: 04/14/2011 21:42:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetFriends](@name nvarchar(256))
as
begin
	select 
		guest.Name as Name,
		guest.Type as Type,
		r.RenewTime as RenewTime,
		r.Relationship as Relationship
	from 
		UserRelationship r,
		Users host,
		Users guest
	where 
		r.HostKey=host.[Key] and
		r.GuestKey=guest.[Key] and
		host.UpperName=upper(@name)
end
GO
/****** Object:  StoredProcedure [dbo].[GetAllUsers]    Script Date: 04/14/2011 21:42:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetAllUsers]
as
begin
	select [Key], Name, Nickname, EMail, RegisterTime 
	from Users u 
	where u.Type = 0
	order by RegisterTime desc
end
GO
/****** Object:  StoredProcedure [dbo].[GetAllGroups]    Script Date: 04/14/2011 21:42:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetAllGroups]
as
begin
	select u.[Key], u.Name, u.Nickname, u.EMail, u.RegisterTime, c.[Key] Creator
	from Users u, Users c, UserRelationShip ur 
	where u.Type = 1 and u.[Key] = ur.HostKey and c.[Key] = ur.GuestKey and ur.Relationship = 3
	order by u.RegisterTime desc
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccountInfoByName]    Script Date: 04/14/2011 21:42:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetAccountInfoByName](@name nvarchar(256))
as
begin
	select * from Users where UpperName = UPPER(@name)
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccountInfoByID]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetAccountInfoByID](@id int)
as
begin
	select * from Users where [Key] = @id
end
GO
/****** Object:  StoredProcedure [dbo].[FindMessages]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[FindMessages](@user int, @peer int, @from datetime)
as
begin
	if(@peer > 0)
	begin
		select top 100 [Key],Receiver,Sender,Content,CreatedTime
		from Message 
		where Receiver = @user and Sender = @peer and CreatedTime > @from
		order by CreatedTime desc
	end
	else
	begin
		select top 100 temp.[Key], temp.Receiver, temp.Sender, temp.Content, temp.CreatedTime 
		from (
			select [Key], Receiver, Sender, Content, CreatedTime 
			from Message 
			where Receiver = @user and CreatedTime > @from
			union all
			select m.[Key], m.Receiver, m.Sender, m.Content, m.CreatedTime 
			from Message m, UserRelationShip ur, Users u
			where ur.HostKey = @user and ur.GuestKey = u.[Key] and u.Type = 1 and ur.GuestKey = m.Receiver and m.CreatedTime > @from and m.CreatedTime > ur.RenewTime
		) temp
		order by CreatedTime desc
	end
end
GO
/****** Object:  StoredProcedure [dbo].[FindHistory]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[FindHistory](@user int, @peer int, @from datetime, @to datetime)
as
begin
	select top 30 temp.[Key], temp.Receiver, temp.Sender, temp.Content, temp.CreatedTime 
	from (
		select [Key], Receiver, Sender, Content, CreatedTime 
		from Message 
		where Receiver = @user and Sender = @peer and CreatedTime >= @from and CreatedTime <= @to

		union all

		select [Key], Receiver, Sender, Content, CreatedTime 
		from Message 
		where Receiver = @peer and Sender = @user and CreatedTime >= @from and CreatedTime <= @to

		union all

		select 
			m.[Key], m.Receiver, m.Sender, m.Content, m.CreatedTime 
		from 
			Message m, UserRelationShip ur, Users u, Users s
		where 
			ur.HostKey = @user and m.Receiver = @peer 
			and ur.GuestKey = u.[Key] and u.Type = 1 and ur.GuestKey = m.Receiver 
			and CreatedTime >= @from and CreatedTime <= @to
			and m.Sender = s.[Key] and s.UpperName <> 'ADMINISTRATOR' and m.CreatedTime > ur.RenewTime
	) temp
	order by CreatedTime desc
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[DeleteUser](@id int)
as
begin
	delete from UserRelationship where HostKey = @id or GuestKey = @id;
	delete from User_Role where UserKey = @id
	delete from Users where [Key] = @id;
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroup]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[DeleteGroup](@id int)
as
begin
	delete from User_Role where UserKey=@id
	delete from UserRelationship where HostKey=@id or GuestKey=@id
	delete from Users where [Key]=@id
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteFriend]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[DeleteFriend](@user int, @friend int)
as
begin
	delete from UserRelationship
	where (HostKey=@user and GuestKey=@friend) or (HostKey=@friend and GuestKey=@user)
end
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[CreateUser](@name nvarchar(256), @nickname nvarchar(256), @password nvarchar(256), @email nvarchar(256), @inviteCode nvarchar(256))
as
begin
	select [Key] from Users where UpperName=UPPER(@name)
	if(@@rowcount>0)
	begin
		raiserror(N'用户"%s"已存在', 16 ,1, @name)
		return
	end

	insert into Users (Name,UpperName,Password,Nickname,Type,EMail,InviteCode,IsTemp,RegisterTime) 
	values (@name,UPPER(@name),@password,@nickname,0,@email,@inviteCode,0,getdate())

	declare @id int
	set @id = @@identity

	insert into User_Role (UserKey,RoleKey)
	select [Key] as UserKey,2 as RoleKey from Users where [Key]=@id

	insert into UserRelationShip(HostKey, GuestKey,RelationShip,RenewTime)
	select u1.[Key], u2.[Key], 0, getdate() as RenewTime
	from Users u1, Users u2 
	where u1.UpperName='PUBLIC' and u2.[Key]=@id

	insert into UserRelationShip(HostKey, GuestKey,RelationShip,RenewTime)
	select u2.[Key], u1.[Key], 0, getdate() as RenewTime
	from Users u1, Users u2 
	where u1.UpperName='PUBLIC' and u2.[Key]=@id

	insert into UserRelationShip(HostKey, GuestKey,RelationShip,RenewTime)
	select u1.[Key], u2.[Key], 0, getdate() as RenewTime
	from Users u1, Users u2 
	where u1.UpperName='ADMIN' and u2.[Key]=@id

	insert into UserRelationShip(HostKey, GuestKey,RelationShip,RenewTime)
	select u2.[Key], u1.[Key], 0, getdate() as RenewTime
	from Users u1, Users u2 
	where u1.UpperName='ADMIN' and u2.[Key]=@id
							
end
GO
/****** Object:  StoredProcedure [dbo].[CreateTempUser]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[CreateTempUser](@name nvarchar(256), @nickname nvarchar(256))
as
begin
	select [Key] from Users where UpperName=UPPER(@name)
	if(@@rowcount>0)
	begin
		raiserror(N'用户"%s"已存在', 16 ,1, @name)
		return
	end

	insert into Users (Name,UpperName,Password,Nickname,Type,EMail,InviteCode,IsTemp,RegisterTime) 
	values (@name,UPPER(@name),N'',@nickname,0,N'',N'',1,getdate())

	declare @id int
	set @id = @@identity

	insert into User_Role (UserKey,RoleKey)
	select [Key] as UserKey,2 as RoleKey from Users where [Key]=@id
					
end
GO
/****** Object:  StoredProcedure [dbo].[CreateGroup]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[CreateGroup](@creator nvarchar(256), @name nvarchar(256), @nickname nvarchar(256), @inviteCode nvarchar(256))
as
begin
	select [Key] from Users where UpperName=UPPER(@name)
	if(@@rowcount>0)
	begin
		raiserror(N'群"%s"已存在', 16, 1, @name)
		return
	end

	insert into Users (Name,UpperName,Password,Nickname,Type,EMail,InviteCode,IsTemp,RegisterTime) 
	values (@name,UPPER(@name),'',@nickname,1,'',@inviteCode,0,getdate())

	declare @id int
	set @id = @@identity

	insert into User_Role (UserKey,RoleKey)
	select [Key] as UserKey,2 as RoleKey from Users where [Key]=@id
					
	insert into UserRelationship (RenewTime,HostKey,GuestKey,Relationship)
	select getdate() as RenewTime,(select [Key] from Users where UpperName=UPPER(@creator)) as HostKey,(select [Key] from Users where [Key]=@id) as GuestKey,3 as Relationship
					
	insert into UserRelationship (RenewTime,HostKey,GuestKey,Relationship)
	select getdate() as RenewTime,(select [Key] from Users where [Key]=@id) as GuestKey,(select [Key] from Users where UpperName=UPPER(@creator)) as HostKey,3 as Relationship

end
GO
/****** Object:  StoredProcedure [dbo].[AddFriend]    Script Date: 04/14/2011 21:42:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[AddFriend](@user nvarchar(256), @friend nvarchar(256))
as
begin
	insert into UserRelationship (HostKey,GuestKey,Relationship,RenewTime)
	select host.[Key] as HostKey,guest.[Key] as GuestKey,0,getdate()
	from Users host,Users guest
	where 
		(host.UpperName=upper(@user) or host.UpperName=upper(@friend)) and 
		(guest.UpperName=upper(@friend) or guest.UpperName=upper(@user)) and 
		host.[Key]<>guest.[Key]
end
GO
/****** Object:  Default [DF__Users__EMail__07020F21]    Script Date: 04/14/2011 21:42:00 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('') FOR [EMail]
GO
/****** Object:  Default [DF__Users__MsgFileLi__07F6335A]    Script Date: 04/14/2011 21:42:00 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((2048)) FOR [MsgFileLimit]
GO
/****** Object:  Default [DF__Users__MsgImageL__08EA5793]    Script Date: 04/14/2011 21:42:00 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((200)) FOR [MsgImageLimit]
GO
/****** Object:  Default [DF__Users__AcceptStr__09DE7BCC]    Script Date: 04/14/2011 21:42:00 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [AcceptStrangerIM]
GO
/****** Object:  Default [DF__Users__IsTemp__0AD2A005]    Script Date: 04/14/2011 21:42:00 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsTemp]
GO
/****** Object:  Default [DF__Users__DiskSize__0BC6C43E]    Script Date: 04/14/2011 21:42:00 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((200)) FOR [DiskSize]
GO
/****** Object:  Default [DF__Users__HomePage__0CBAE877]    Script Date: 04/14/2011 21:42:00 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('') FOR [HomePage]
GO
/****** Object:  Default [DF__Users__HeadIMG__0DAF0CB0]    Script Date: 04/14/2011 21:42:00 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('') FOR [HeadIMG]
GO
/****** Object:  Default [DF__Users__Remark__0EA330E9]    Script Date: 04/14/2011 21:42:00 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('') FOR [Remark]
GO

DELETE FROM Roles;
INSERT INTO Roles([Key], [Name]) values (1, N'管理员');
INSERT INTO Roles([Key], [Name]) values (2, N'普通用户');

SET IDENTITY_INSERT Users ON
DELETE FROM Users;
INSERT INTO Users ([Key], [Name], Password, Type, Nickname, UpperName, RegisterTime)
values (1, N'sa', N'21232F297A57A5A743894A0E4A801FC3', 0, N'系统管理员', N'SA', getdate());
INSERT INTO Users ([Key], [Name], Password, Type, Nickname, UpperName, RegisterTime)
values (2, N'administrator', N'', 0, N'系统管理员', N'ADMINISTRATOR', getdate());
INSERT INTO Users ([Key], [Name], Password, Type, Nickname, UpperName, RegisterTime)
values (3, N'admin', N'21232F297A57A5A743894A0E4A801FC3', 0, N'系统管理员', N'ADMIN', getdate());
INSERT INTO Users ([Key], [Name], Password, Type, Nickname, UpperName, RegisterTime)
values (4, N'public', N'', 1, N'公共聊天室', N'PUBLIC', getdate());
SET IDENTITY_INSERT Users OFF


DELETE FROM User_Role;
INSERT INTO User_Role(UserKey, RoleKey)
SELECT Users.[Key], Roles.[Key] FROM Users, Roles WHERE UpperName='SA'
INSERT INTO User_Role(UserKey, RoleKey)
SELECT Users.[Key], Roles.[Key] FROM Users, Roles WHERE UpperName='ADMIN'
INSERT INTO User_Role(UserKey, RoleKey)
SELECT Users.[Key], Roles.[Key] FROM Users, Roles WHERE UpperName='ADMINISTRATOR'

DELETE FROM UserRelationship;
INSERT INTO UserRelationship(HostKey, GuestKey, RenewTime, Relationship)
SELECT u1.[Key], u2.[Key], getdate(), 3
FROM Users u1, Users u2
WHERE u1.[Key]<>u2.[Key] and u1.UpperName=N'ADMIN' and u2.UpperName=N'PUBLIC'
INSERT INTO UserRelationship(HostKey, GuestKey, RenewTime, Relationship)
SELECT u1.[Key], u2.[Key], getdate(), 3
FROM Users u1, Users u2
WHERE u1.[Key]<>u2.[Key] and u2.UpperName=N'ADMIN' and u1.UpperName=N'PUBLIC'
