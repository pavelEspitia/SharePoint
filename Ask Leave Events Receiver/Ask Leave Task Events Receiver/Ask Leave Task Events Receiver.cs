using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Ask_Leave_Events_Receiver.Ask_Leave_Task_Events_Receiver
{
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class Ask_Leave_Task_Events_Receiver : SPItemEventReceiver
    {
       /// <summary>
       /// 已添加项.
       /// </summary>
       public override void ItemAdded(SPItemEventProperties properties)
       {
           base.ItemAdded(properties);
		   if (properties.ListTitle == "请假单审批任务")
		   {
			   Guid related_field_guid = new Guid("{58ddda52-c2a3-4650-9178-3bbc1f6e36da}");
			   string link = properties.ListItem[related_field_guid].ToString().Split(',')[0];
			   int id_index = link.IndexOf("ID=");
			   int id = int.Parse(link.Substring(id_index + 3));
			   SPListItem ask_leave_item = properties.OpenWeb().Lists["请假单"].GetItemById(id);
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
							   foreach (SPRoleAssignment ra in ask_leave_item.RoleAssignments)
							   {
								   item.RoleAssignments.Add(ra);
							   }
						   }
						   catch (Exception ex)
						   {
							   log(site, "自动删除项目旧权限", "错误", ex.ToString());
						   }
						   // 分别为各个角色赋予此项目的独特权限：。
						   log(web.Site, "更新项目权限", "消息", "为项目【" + item["Title"] + "】更新权限完成。");
					   }
				   }
			   });
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
