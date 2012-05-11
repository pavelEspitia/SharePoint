﻿#SharePoint 项目及安装部署说明：


欢迎访问 [JonyZhu's Blog](http://www.cnblogs.com/jonyzhu)。


##介绍站点结构


###Works 公网 

<table>
	<tr>
		<th nowrap>网址</th><td>http://www.works.com</td>
	</tr>
	<tr>
		<th nowrap>介绍</th>
		<td>
			<ul>
				<li>是 Works 公司的公共网站，为互联网匿名用户提供各种信息。匿名 + Windows。</li>
			</ul>
		</td>
	</tr>
	<tr>
		<th nowrap>子站</th>
		<td>
			<table>
				<tr>
					<th nowrap>名称</th>
					<td>根站点 /</td>
				</tr>
				<tr>
					<th nowrap>解决方案</th>
					<td> 
						<ul>
							<li>zTemplates\news summary.stp。“新闻摘要”列表模板。</li>
							<li>News Events Receiver。新闻 Wiki 页更事件解析。</li>
							<li>News Carousel。新闻滚动特效组件。</li>
							<li>Hide Quick Launch Web Part，可以隐藏“快速启动”栏。</li>
							<li>Lightbox Web Part，可以指定引入 LightBox2 图片浏览工具。</li>
						</ul> 
					</td>
				</tr>
				<tr>
					<th nowrap>权限</th>
					<td> </td>
				</tr>
				<tr>
					<th nowrap>列表</th>
					<td> 
						<table>
							<tr><th nowrap>名称</th><td>图片库</td></tr>
						</table>
						<table>
							<tr><th nowrap>名称</th><td>新闻页</td></tr>
							<tr><th nowrap>设置</th><td>
									<ul>
										<li>新闻 Wiki 页面库。</li>
										<li>需要“彩色标题 3”（标题）、“表格样式”（图片）、“注释 2”（摘要）、“注释 1”（分类）才能构成一个完整的新闻。</li>
									</ul>
								</td>
							</tr>
						</table>
						<table>
							<tr><th nowrap>名称</th><td>新闻摘要</td></tr>
							<tr><th nowrap>设置</th><td>
									<ul>
										<li>可选在 AllItems.aspx 页面加入 List Image Web Part。</li>
									</ul>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<th nowrap>名称</th>
					<td>管理中心 /admin</td>
				</tr>
				<tr>
					<th nowrap>解决方案</th>
					<td> 
						<ul>
							<li>zTemplates\system log.stp。“系统日志”列表模板。</li>
							<li>zTemplates\components.stp。“组件库”文档模板。</li>
						</ul> 
					</td>
				</tr>
				<tr>
					<th nowrap>权限</th>
					<td> </td>
				</tr>
				<tr>
					<th nowrap>列表</th>
					<td> 
						<table>
							<tr><th nowrap>名称</th><td>系统日志</td></tr>
							<tr><th nowrap>设置</th><td>
									<ul>
										<li>可选在 AllItems.aspx 页面加入 Color Line Web Part。</li>
										<li>可选在 AllItems.aspx 页面加入 Child List Style。</li>
									</ul>
								</td>
							</tr>
						</table>
						<table>
							<tr><th nowrap>名称</th><td>组件库</td></tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>

###Works 公网（员工专用）

<table>
	<tr>
		<th nowrap>网址</th><td>https://www.works.com</td>
	</tr>
	<tr>
		<th nowrap>介绍</th>
		<td>
			<ul>
				<li>是 Works 公网的扩展，为 Works 的员工发布和更新信息用。FBA + Windows。</li>
				<li>无须额外部署解决方案。</li>
				<li>需要 zCertificates\star.works.com.cer 证书。</li>
			</ul>
		</td>
	</tr>
</table>


###Works 内网 

<table>
	<tr>
		<th nowrap>网址</th><td>https://intranet.works.com</td>
	</tr>
	<tr>
		<th nowrap>介绍</th>
		<td>
			<ul>
				<li>是 Works 公司的办公内网。</li>
				<li>经典 Windows 验证，未安装 DNS 时需要开启明文密码。</li>
				<li>需要 zCertificates\star.works.com.cer 证书。</li>
			</ul>
		</td>
	</tr>
	<tr>
		<th nowrap>子站</th>
		<td>
			<table>
				<tr>
					<th nowrap>名称</th>
					<td>根站点 /</td>
				</tr>
				<tr>
					<th nowrap>解决方案</th>
					<td> 
						<ul>
							<li>Site Groups。（可选），基于 Windows 认证时，可以添加 <b>zUsers/zhuzi</b> 目录下的用户到本地用户，然后此解决方案将关联这些用户。</li>
							<li>Style Web Parts。各种样式 Web Part。</li>
						</ul> 
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<th nowrap>名称</th>
					<td>项目中心 /projects</td>
				</tr>
				<tr>
					<th nowrap>解决方案</th>
					<td> 
						<ul>
							<li>Project Resources。为列表提供中文和英文的字段名国际化支持。</li>
							<li>Project List。自动创建“项目列表”。</li>
							<li>Project Status。自动创建“项目状态列表”。</li>
							<li>Project Documents。自动创建“项目文档库”。</li>
							<li>Project Events Receiver。
							<ul>
								<li>处理项目信息更新时间，设置对应权限。</li>
								<li>更新项目的“反馈”到“反馈汇总”。</li>
								<li>更新项目文档库目录的日期。</li>
							</ul>
							</li>
						</ul> 
					</td>
				</tr>
				<tr>
					<th nowrap>权限</th>
					<td>断开继承。仅项目管理办公室可以有“参与讨论”权限。其他用户权限都清除，待项目创建后，关联用户权限将自动添加。</td>
				</tr>
				<tr>
					<th nowrap>列表</th>
					<td>
						<table>
							<tr><th nowrap>名称</th><td>项目列表 Projects</td></tr>
							<tr>
								<th nowrap>设置</th>
								<td>
									<ul>
										<li>在默认 DispForm.aspx 页面加入关联的 Status 和 Documents 列表。</li>
										<li>在默认 DispForm.aspx 页面加入 Child List Style （Web Part）。</li>
									</ul>
								</td>
							</tr>
						</table>
						<table>
							<tr><th nowrap>名称</th><td>项目状态列表 Project Status</td></tr>
							<tr>
								<th nowrap>设置</th>
								<td>
									<ul>
										<li>添加“相关项目ID”字段。</li>
									</ul>
								</td>
							</tr>
						</table>
						<table>
							<tr><th nowrap>名称</th><td>项目文档库 Project Documents</td></tr>
							<tr>
								<th nowrap>设置</th>
								<td>
									<ul>
										<li>从已有字段中选择并添加“相关项目”、“文档类型（项目状态）”字段。</li>
										<li>添加“相关项目ID”字段。</li>
									</ul>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<th nowrap>名称</th>
					<td>行政管理 /admin</td>
				</tr>
				<tr>
					<th nowrap>解决方案</th>
					<td>
						<ul>
							<li>zTemplates\system log.stp。“系统日志”列表模板。</li>
							<li>zTemplates\annual leave.stp。“年假汇总”列表模板。</li>
							<li>Customized Fields。“相关经理”字段。</li>
							<li>zTemplates\ask leave.stp。“请假单”列表模板。</li>
							<li>zTemplates\components.stp。“组件库”文档模板。</li>
							<li>Ask Leave Events Receiver。请假相关的事件、权限、状态更新处理。</li>
						</ul> 
					</td>
				</tr>
				<tr>
					<th nowrap>权限</th>
					<td> </td>
				</tr>
				<tr>
					<th nowrap>列表</th>
					<td> 
						<table>
							<tr><th nowrap>名称</th><td>系统日志</td></tr>
							<tr><th nowrap>设置</th><td>
									<ul>
										<li>可选在 AllItems.aspx 页面加入 Color Line Web Part。</li>
										<li>可选在 AllItems.aspx 页面加入 Child List Style。</li>
									</ul>
								</td>
							</tr>
						</table>
						<table>
							<tr><th nowrap>名称</th><td>年假汇总</td></tr>
							<tr><th nowrap>说明</th><td>记录每个人在每年的总可用年假天数和剩余年假天数。</td></tr>
							<tr>
								<th nowrap>权限</th>
								<td>
									<ul>
										<li>断开继承。仅“行政”、“HR”有关人员有“参与讨论”权限，个人自己有“读取”权限。</li>
									</ul>
								</td>
							</tr>
						</table>
						<table>
							<tr><th nowrap>名称</th><td>请假单</td></tr>
							<tr>
								<th nowrap>设置</th>
								<td>
									<ul>
										<li>需要断开权限继承，让组织中所有人都可以提交请假申请。</li>
										<li>需要添加“相关经理”字段。</li>
										<li>请假先找项目经理，找不到才给职能经理。</li>
										<li>部署 <b>zWorkflow/ask leave.vwi</b> 工作流。</li>
										<li>部署 <b>zForms/ask leave initial form.xsn</b>、<b>zForms/ask leave task form.xsn</b>。</li>
									</ul>
								</td>
							</tr>
						</table>
						<table>
							<tr><th nowrap>名称</th><td>请假单审批任务</td></tr>
						</table>
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<th nowrap>名称</th>
					<td>我的网站 /sites/my</td>
				</tr>
				<tr>
					<th nowrap>介绍</th>
					<td> 
						<ul>
							<li>是 Works 公司的员工个人自助网站。</li>
						</ul> 
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>


###合作中心网站 

<table>
	<tr>
		<th nowrap>网址</th>
		<td>https://partner.works.com</td>
	</tr>
	<tr>
		<th nowrap>介绍</th>
		<td> 
			<ul>
				<li>是 Works 公司与合作伙伴共享的协作站点。</li>
				<li>需要 zCertificates\star.works.com.cer 证书。</li>
			</ul> 
		</td>
	</tr>
</table>


##配置 FBA


> FBA 设置：  
> aspnetmembership (WorksMembership)  
> aspnetrolemanager (WorksRole)  
> 难搞、不好同步，放弃。 5/11/2012


###从头开始建立
* 使用 Aspnet_regsql.exe 命令创建用户数据库，名字叫“SharePointUsers”。
* 使用下面的 SQL 命令创建用户。  


> declare @now datetime  
> set @now= GETDATE()  
> EXEC aspnet_Membership_CreateUser 'SharePoint - www.works.com80','jony','pass@word1','','jony@works.com','','',1,@now,@now,0,0,null  
> EXEC aspnet_Roles_CreateRole 'SharePoint - www.works.com80', 'Admin'   
> EXEC aspnet_UsersInRoles_AddUsersToRoles 'SharePoint - www.works.com80', 'jony', 'Admin', 8   


* 要激活 FBA，需要在 Web 应用程序、SharePoint 管理中心、STS 服务的 Web.Config 中加入以下设置。  


> &lt;connectionStrings&gt;  
> 	&lt;add name="MyLocalSQLServer" connectionString="data source=.\SQLEXPRESS;Integrated Security=SSPI;Database=SharePointUsers;" /&gt;  
> &lt;/connectionStrings&gt;  
> &lt;system.web&gt;  
> 	&lt;membership defaultProvider="i"&gt;  
> 		&lt;providers&gt;  
> 			&lt;add name="i" type="Microsoft.SharePoint.Administration.Claims.SPClaimsAuthMembershipProvider, Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" /&gt;  
> 			&lt;add name="aspnetmembership" connectionStringName="MyLocalSQLServer" applicationName="SharePoint - www.works.com80" type="System.Web.Security.SqlMembershipProvider, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /&gt;		  
> 		&lt;/providers&gt;  
> 	&lt;/membership&gt;  
> 	&lt;roleManager defaultProvider="c" enabled="true"&gt;  
> 		&lt;providers&gt;  
> 			&lt;add name="c" type="Microsoft.SharePoint.Administration.Claims.SPClaimsAuthRoleProvider, Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" /&gt;  
> 			&lt;add name="aspnetrolemanager" connectionStringName="MyLocalSQLServer" applicationName="SharePoint - www.works.com80" type="System.Web.Security.SqlRoleProvider, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /&gt;		  
> 		&lt;/providers&gt;  
> 	&lt;/roleManager&gt;  
> &lt;/system.web&gt;


###从现有备份还原
> 暂不支持。