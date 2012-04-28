using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Site_Groups.Features.Groups
{
	/// <summary>
	/// 此类用于处理在激活、停用、安装、卸载和升级功能的过程中引发的事件。
	/// </summary>
	/// <remarks>
	/// 附加到此类的 GUID 可能会在打包期间使用，不应进行修改。
	/// </remarks>

	[Guid("65f414a4-2dcf-456c-838f-54021972c5b8")]
	public class GroupsEventReceiver : SPFeatureReceiver
	{
		// 取消对以下方法的注释，以便处理激活某个功能后引发的事件。

		public override void FeatureActivated(SPFeatureReceiverProperties properties)
		{
			SPSite site = properties.Feature.Parent as SPSite;
			SPWeb web = site.RootWeb;
			try
			{
				SPGroup group = create_group("经理办公室", web);
				add_user(group, "er.li");
				group = create_group("人力资源部", web);
				add_user(group, "zhou.zhuang");
				add_user(group, "yukou.lie");
				group = create_group("行政管理部", web);
				add_user(group, "qiu.kong");
				add_user(group, "yangming.wang");
				group = create_group("财务部", web);
				add_user(group, "fei.han");
				group = create_group("市场销售部", web);
				add_user(group, "di.mo");
				add_user(group, "sheng.meng");
				group = create_group("项目管理办公室", web);
				add_user(group, "guzi.gui");
				group = create_group("项目一部", web);
				add_user(group, "qin.su");
				add_user(group, "xi.deng");
				add_user(group, "long.gongsun");
				group = create_group("项目二部", web);
				add_user(group, "yi.zhang");
				add_user(group, "bin.sun");
				add_user(group, "que.bian");
				web.Description = "Groups updated at: " + System.DateTime.Now.ToLongDateString();
				web.Update();
			}
			catch (Exception ex)
			{
				web.Description = ex.ToString();
				web.Update();
			}
		}

		protected void add_user(SPGroup group, string user_account)
		{
			SPUser user = group.ParentWeb.EnsureUser(user_account);
			group.AddUser(user);
		}

		protected SPGroup create_group(string group_name, SPWeb web)
		{
			SPUser user = web.EnsureUser("Administrator");
			web.SiteGroups.Add(group_name, user, null, group_name);
			SPGroup group = web.SiteGroups[group_name];
			web.Update();

			SPRoleAssignment assignment = new SPRoleAssignment(group);
			SPRoleDefinition role = web.RoleDefinitions["读取"];
			assignment.RoleDefinitionBindings.Add(role);
			web.RoleAssignments.Add(assignment);

			return group;
		}

		// 取消对以下方法的注释，以便处理在停用某个功能前引发的事件。

		public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
		{
			SPSite site = properties.Feature.Parent as SPSite;
			SPWeb web = site.RootWeb;
			try
			{
				web.SiteGroups.Remove("经理办公室");
				web.SiteGroups.Remove("人力资源部");
				web.SiteGroups.Remove("行政管理部");
				web.SiteGroups.Remove("财务部");
				web.SiteGroups.Remove("市场销售部");
				web.SiteGroups.Remove("项目管理办公室");
				web.SiteGroups.Remove("项目一部");
				web.SiteGroups.Remove("项目二部");
				web.Update();
			}
			catch (Exception ex)
			{
				web.Description = ex.ToString();
				web.Update();
			}
		}


		// 取消对以下方法的注释，以便处理在安装某个功能后引发的事件。

		//public override void FeatureInstalled(SPFeatureReceiverProperties properties)
		//{
		//}


		// 取消对以下方法的注释，以便处理在卸载某个功能前引发的事件。

		//public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
		//{
		//}

		// 取消对以下方法的注释，以便处理在升级某个功能时引发的事件。

		//public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
		//{
		//}
	}
}
