#SharePoint 项目及安装部署说明：


欢迎访问 [JonyZhu's Blog](http://www.cnblogs.com/jonyzhu)。


##介绍站点结构

> FBA 设置：  
> aspnetmembership  
> aspnetrolemanager 

###http://www.works.com 
是 Works 公司的公共网站，为互联网匿名用户提供各种信息。匿名 + Windows。  
该站点将部署以下解决方案：
####根站点：
* zTemplates\components.stp --	> 组件库
* zTemplates\news summary.stp --	> 新闻摘要  
	> 可选在 AllItems.aspx 页面加入 List Image Web Part。
* News Events Receiver
* News Carousel  
	> 在需要使用的页面，添加 News Carousel Web Part
* Style Web Parts（可选）
	> Hide Quick Launch Web Part，可以隐藏“快速启动”栏。
####admin 子站（管理中心）
* zTemplates\system log.stp --	> 系统日志  
	> 可选在 AllItems.aspx 页面加入 Color Line Web Part。


###https://www.works.com 
是 Works 公网的扩展，为 Works 的员工发布和更新信息用。FBA + Windows。  
无须额外部署解决方案。  
需要 zCertificates\star.works.com.cer 证书。


###https://intranet.work.com 
是 Works 公司的办公内网。FBA + Windows  
需要 zCertificates\star.works.com.cer 证书。  
该站点将部署以下解决方案：  
####根站点
* Site Groups（可选），基于 Windows 认证时，可以添加 **zUsers/zhuzi	> 目录下的用户到本地用户，然后此解决方案将关联这些用户。


####projects 子站（项目中心）
* Project Resources
* Project List --	> 项目列表  
	> 在默认 DispForm.aspx 页面加入关联的 Status 和 Documents 列表
	> 在默认 DispForm.aspx 页面加入 Child List Style （Web Part）
* Project Status --	> 项目状态列表
* Project Documents --	> 项目文档库。需要手动从已有字段中选择并添加“相关项目”、“文档类型（项目状态）”字段。
* Project Events Receiver  
	> 更新项目、状态、文档的独有权限
	> 更新项目的“反馈”到“反馈汇总”
	> 更新项目文档库目录的日期

####admin 子站（管理中心）
* zTemplates\system log.stp --	> 系统日志。可选在 AllItems.aspx 页面加入 Color Line Web Part。


###https://personal.work.com 
是 Works 公司的员工个人自助网站。FBA + Windows。  
需要 zCertificates\star.works.com.cer 证书。


###https://partner.work.com 
是 Works 公司与合作伙伴共享的协作站点。Federation + FBA + Windows。  
需要 zCertificates\star.works.com.cer 证书。



##配置 FBA
###从头开始建立
* 使用 Aspnet_regsql.exe 命令创建用户数据库，名字叫“SharePointUsers”。
* 使用下面的 SQL 命令创建用户。  
		declare @now datetime
		set @now= GETDATE()
		
		EXEC aspnet_Membership_CreateUser 'SharePoint - www.works.com80','jony','pass@word1','','jony@works.com','','',1,@now,@now,0,0,null
		
		EXEC aspnet_Roles_CreateRole 'SharePoint - www.works.com80', 'Admin' 
		EXEC aspnet_UsersInRoles_AddUsersToRoles 'SharePoint - www.works.com80', 'jony', 'Admin', 8 

* 要激活 FBA，需要在 Web 应用程序、SharePoint 管理中心、STS 服务的 Web.Config 中加入以下设置。  
		<lt;connectionStrings>gt;
			<lt;add name="MyLocalSQLServer" connectionString="data source=.\SQLEXPRESS;Integrated Security=SSPI;Database=SharePointUsers;" />gt;
		<lt;/connectionStrings>gt;
		<lt;system.web>gt;
			<lt;membership defaultProvider="i">gt;
				<lt;providers>gt;
					<lt;add name="i" type="Microsoft.SharePoint.Administration.Claims.SPClaimsAuthMembershipProvider, Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" />gt;
					<lt;add name="aspnetmembership" connectionStringName="MyLocalSQLServer" applicationName="SharePoint - www.works.com80" type="System.Web.Security.SqlMembershipProvider, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />gt;		
				<lt;/providers>gt;
			<lt;/membership>gt;
			<lt;roleManager defaultProvider="c" enabled="true">gt;
				<lt;providers>gt;
					<lt;add name="c" type="Microsoft.SharePoint.Administration.Claims.SPClaimsAuthRoleProvider, Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" />gt;
					<lt;add name="aspnetrolemanager" connectionStringName="MyLocalSQLServer" applicationName="SharePoint - www.works.com80" type="System.Web.Security.SqlRoleProvider, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />gt;		
				<lt;/providers>gt;
			<lt;/roleManager>gt;
		<lt;/system.web>gt;


###从现有备份还原

