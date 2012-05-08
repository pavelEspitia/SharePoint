declare @now datetime
declare @user varchar(50)
declare @app varchar(50)
declare @role varchar(50)
declare @email varchar(100)
set @now= GETDATE()
set @app='www.works.com'
set @user='jony'
set @role='Admin'
set @email = lower(@user) + '@works.com'
EXEC aspnet_Membership_CreateUser @app,@user,'pass@word1','',@email,'','',1,@now,@now,0,0,null
EXEC aspnet_Roles_CreateRole @app, @role
EXEC aspnet_UsersInRoles_AddUsersToRoles @app, @user, @role, 8