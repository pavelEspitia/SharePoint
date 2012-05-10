using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Ask_Leave_Events_Receiver.Ask_Leave_List_Events_Receiver
{
	/// <summary>
	/// 列表项事件
	/// </summary>
	public class Ask_Leave_List_Events_Receiver : SPItemEventReceiver
	{
		/// <summary>
		/// 已添加项.
		/// </summary>
		public override void ItemAdded(SPItemEventProperties properties)
		{
			this.EventFiringEnabled = false;

			base.ItemAdded(properties);
			update_permission(properties, properties.Web.SiteUsers.GetByID(properties.CurrentUserId));
		}

		/// <summary>
		/// 已更新项.
		/// </summary>
		public override void ItemUpdated(SPItemEventProperties properties)
		{
			this.EventFiringEnabled = false;

			base.ItemUpdated(properties);
			update_permission(properties,  properties.Web.SiteUsers.GetByID(properties.CurrentUserId));
		}


		protected void update_permission(SPItemEventProperties properties,SPUser user)
		{

			SPSecurity.RunWithElevatedPrivileges(delegate
			{
				using (SPSite site = new SPSite(properties.SiteId))
				{
					using (SPWeb web = site.OpenWeb("/admin"))
					{
						SPListItem item = web.Lists[properties.ListId].Items.GetItemById(properties.ListItemId);
						item.BreakRoleInheritance(false);
						try
						{
							// 清理旧权限。
							int role_index = 0;
							while (item.RoleAssignments.Count > 1)
							{
								SPRoleAssignment role = item.RoleAssignments[role_index];
								if (role.Member.LoginName.ToLower() == "sharepoint\\system")
								{
									role_index++;
								}
								else
								{
									item.RoleAssignments.Remove(role_index);
								}
							}
						}
						catch (Exception ex)
						{
							log(site, "自动删除项目旧权限", "错误", ex.ToString());
						}
						// 分别为各个角色赋予此项目的独特权限：。
						assign_role(site, item, user, "", web.RoleDefinitions["参与讨论"]);
						assign_roles(web, item, "相关经理", "", web.RoleDefinitions["批准"]);
						log(web.Site, "更新项目权限", "消息", "为项目【" + item["Title"] + "】更新权限完成。");
					}
				}
			});
		}
		protected void assign_role(SPSite site, SPListItem item, SPUser user, string field_guid, SPRoleDefinition definition)
		{
			try
			{
				bind_role(item, user, definition);
			}
			catch (Exception ex)
			{
				log(site, "为项目【" + item["Title"] + "】的【" + user.Name + "】授权时发生错误", "错误", ex.ToString());
			}
		}

		protected void assign_roles(SPWeb web, SPListItem item, string field_name, string field_guid, SPRoleDefinition definition)
		{
			try
			{
				string[] users = item[field_name].ToString().Split(new char[] { ';', '#' }, StringSplitOptions.RemoveEmptyEntries);

				for (int i = 0; i < users.Length; i += 2)
				{
					bind_role(item, web.SiteUsers.GetByID(int.Parse(users[i])), definition);
				}

			}
			catch (Exception ex)
			{
				log(web.Site, "为项目【" + item["Title"] + "】的【" + field_name + "】授权时发生错误", "错误", ex.ToString());
			}
		}

		protected void bind_role(SPListItem item, SPPrincipal principal, SPRoleDefinition definition)
		{
			try
			{
				SPRoleAssignment assignment = new SPRoleAssignment(principal);
				assignment.RoleDefinitionBindings.Add(definition);
				item.RoleAssignments.Add(assignment);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected void log(SPSite site, string title, string type, string content)
		{
			using (SPWeb web = site.OpenWeb("/admin"))
			{
				SPList log_list = web.Lists["系统日志"];
				SPListItem log_item = log_list.AddItem();
				log_item["Title"] = title;
				log_item["日志类型"] = type;
				log_item["日志详情"] = content;
				log_item["相关系统"] = "请假管理";
				log_item.Update();
			}
		}


	}
}
