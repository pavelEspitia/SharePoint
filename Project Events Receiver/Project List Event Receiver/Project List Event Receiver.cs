using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Project_Events_Receiver.Project_List_Event_Receiver
{
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class Project_List_Event_Receiver : SPItemEventReceiver
    {
		/// <summary>
		/// 已添加项.
		/// </summary>
		public override void ItemAdded(SPItemEventProperties properties)
		{
			this.EventFiringEnabled = false;
			base.ItemAdded(properties);
			update_permission(properties);
		}

		/// <summary>
		/// 已更新项.
		/// </summary>
		public override void ItemUpdated(SPItemEventProperties properties)
		{
			this.EventFiringEnabled = false;
			base.ItemUpdated(properties);
			update_permission(properties);
		}

		protected void update_permission(SPItemEventProperties properties)
		{
			SPSecurity.RunWithElevatedPrivileges(delegate
			{
				using (SPSite site = new SPSite(properties.SiteId))
				{
					using (SPWeb web = site.OpenWeb("/projects"))
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
						assign_role(site, item, "ProjectManager", "{3c3ac491-4910-4ddb-b28f-1e7328ff26d5}", web.RoleDefinitions["设计"]);
						assign_roles(web, item, "ProjectMembers", "{7494dd0e-3c92-4556-b1cb-dd9e10b10d7f}", web.RoleDefinitions["读取"]);
						assign_roles(web, item, "ProjectSupervisers", "{89ae4bfd-6243-4dce-aa79-c980aeff7584}", web.RoleDefinitions["参与讨论"]);
						assign_roles(web, item, "ProjectVisitors", "{72bddba3-d5d3-465b-ac1b-331a1c403fe9}", web.RoleDefinitions["读取"]);
						log(web.Site, "更新项目权限", "消息", "为项目【" + item["Title"] + "】更新权限完成。");
					}
				}
			});
		}

		protected void assign_role(SPSite site, SPListItem item, string field_name, string field_guid, SPRoleDefinition definition)
		{
			try
			{
				string value = item[field_name].ToString();
				SPFieldUserValue field_user_value = (SPFieldUserValue)item.Fields[new Guid(field_guid)].GetFieldValue(value);
				bind_role(item, field_user_value.User, definition);
			}
			catch (Exception ex)
			{
				log(site, "为项目【" + item["Title"] + "】的【" + field_name + "】授权时发生错误", "错误", ex.ToString());
			}
		}

		protected void assign_roles(SPWeb web, SPListItem item, string field_name, string field_guid, SPRoleDefinition definition)
		{
			try
			{
				string value = item[field_name].ToString();
				SPFieldUserValueCollection field_user_value = (SPFieldUserValueCollection)item.Fields[new Guid(field_guid)].GetFieldValue(value);
				foreach (SPFieldUserValue user_value in field_user_value)
				{
					if (user_value.User == null)
					{
						SPGroup group = web.SiteGroups.GetByID(user_value.LookupId);
						bind_role(item, group, definition);
					}
					else
					{
						bind_role(item, user_value.User, definition);
					}
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
				log_item["相关系统"] = "项目中心";
				log_item.Update();
			}
		}

	}
}
