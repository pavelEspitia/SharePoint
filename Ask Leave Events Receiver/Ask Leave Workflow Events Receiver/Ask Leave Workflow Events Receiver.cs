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
						   if (list.Title == "请假单") //TODO:怎么弄不到流程最后的状态？非要去 Task 的 XML 里面自己找？
						   {
							   SPListItem item = list.Items.GetItemById(wf.ItemId);
							   title = item.Title;
							   Records.DeclareItemAsRecord(item);
						   }
					   }

					   log(site, "请假单流程结束", "消息", "请假单【" + title + "】流程结束，已经声明为记录。");
				   }
				   catch (Exception ex)
				   {
					   log(site, "请假单流程结束", "错误", "请假单【" + title + "】流程结束，声明为记录时出错。"+ex.ToString());
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
