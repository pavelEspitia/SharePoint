using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Microsoft.Office.RecordsManagement.RecordsRepository;

namespace Ask_Leave_Events_Receiver.Ask_Leave_Workflow_Events_Receiver
{
    /// <summary>
    /// 列表工作流事件
    /// </summary>
    public class Ask_Leave_Workflow_Events_Receiver : SPWorkflowEventReceiver
    {
       /// <summary>
       /// 工作流已完成.
       /// </summary>
       public override void WorkflowCompleted(SPWorkflowEventProperties properties)
       {
		   base.WorkflowCompleted(properties);
		   if (properties.CompletionType != SPWorkflowEventCompletionType.Completed) return;
		   SPSecurity.RunWithElevatedPrivileges(delegate
		   {
			   using (SPSite site = new SPSite(properties.WebUrl))
			   {
				   string title="";
				   try
				   {
					   using (SPWeb web = site.OpenWeb())
					   {
						   SPWorkflow wf = new SPWorkflow(web, properties.InstanceId);
						   SPList list = web.Lists[wf.ListId];
						   if (list.Title == "请假单") 
						   {
							   SPListItem item = list.Items.GetItemById(wf.ItemId);
							   title = item.Title;
							   if (!Records.IsRecord(item))
							   {
								   Records.DeclareItemAsRecord(item);
								   log(site, "请假单流程结束", "消息", "请假单【" + title + "】流程结束，已经声明为记录。");

								   if (item["请假单审批流程"].ToString() == "16") // 通过了才扣年假，对吧。
								   {
									   if (item["假别"].ToString() == "年假")
									   {
										   SPList annual_list = web.Lists["年假汇总"];
										   SPQuery query = new SPQuery();
										   DateTime date = DateTime.Parse(item["开始日期"].ToString());
										   string user_id = item["创建者"].ToString().Split(new char[] { ';', '#' })[0];
										   query.Query = "<Where><And><Eq><FieldRef Name='_x5e74__x4efd_'/><Value Type='Integer'>" + date.Year + "</Value></Eq><Eq><FieldRef Name='_x4eba__x5458_' LookupId='TRUE'/><Value Type='User'>" + user_id + "</Value></Eq></And></Where>";
										   SPListItemCollection annual_items = annual_list.GetItems(query);
										   if (annual_items.Count > 0)
										   {
											   SPListItem annual_item = annual_items[0];
											   int annual_days_left = int.Parse(annual_item["剩余年假天数"].ToString());
											   int leave_days = int.Parse(item["请假天数"].ToString());
											   annual_item["剩余年假天数"] = annual_days_left - leave_days;
											   annual_item.Update();
											   log(site, "年假汇总更新", "消息", "请假单【" + title + "】流程结束，对应的年假剩余天数已经更新。");
										   }
									   }
								   }
							   }
						   }
					   }

				   }
				   catch (Exception ex)
				   {
					   log(site, "请假单流程结束", "错误", "请假单【" + title + "】流程结束，声明为记录时出错。" + ex.ToString());
				   }
			   }
		   });
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
