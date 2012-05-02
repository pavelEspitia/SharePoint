using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Project_Workload.Features.List
{
	/// <summary>
	/// 此类用于处理在激活、停用、安装、卸载和升级功能的过程中引发的事件。
	/// </summary>
	/// <remarks>
	/// 附加到此类的 GUID 可能会在打包期间使用，不应进行修改。
	/// </remarks>

	[Guid("c455d8c6-ca45-4442-a9de-99b7d7110b41")]
	public class ListEventReceiver : SPFeatureReceiver
	{
		// 取消对以下方法的注释，以便处理激活某个功能后引发的事件。

		public override void FeatureActivated(SPFeatureReceiverProperties properties)
		{
			SPSite site = properties.Feature.Parent as SPSite;
			SPWeb web = site.RootWeb;
			SPList project_workload_list =web.Lists["Project Workload"];
			SPList project_documents_list = web.Lists["Project Documents"];
			SPFieldChoice document_type = (SPFieldChoice)project_documents_list.Fields["Document Type"];
			int year_from = 2010;
			int month_from = 12;
			int year_to = DateTime.Now.Year;
			int month_to = DateTime.Now.Month;
			for (int year = year_from; year <= year_to; year++)
			{
				int current_month_to = year == year_to ? month_to : 12;
				for (int month = month_from; month <= current_month_to; month++)
				{
					foreach (string choice in document_type.Choices)
					{
						SPListItem workload = project_workload_list.Items.Add();
						workload["Written Year"] = year;
						workload["Written Month"] = month;
						workload["Document Type"] = choice;
						workload["Document Count"] = 0;
						workload.Update();
					}
				}
				month_from = 1;
			}
			//SPListItemCollection project_workload_items = project_workload_list.Items;
			SPListItemCollection project_documents_items = project_documents_list.Items;
			foreach (SPListItem document in project_documents_items)
			{
				DateTime wd = DateTime.Parse(document["Written Date"].ToString());
				SPQuery query = new SPQuery();
				query.Query=string.Concat(
					"<Where><And>",
						"<And>",
							"<Eq><FieldRef Name='Written Year'/>","<Value Type='Integer'>",wd.Year,"</Value></Eq>",
							"<Eq><FieldRef Name='Written Month'/>","<Value Type='Integer'>",wd.Month,"</Value></Eq>",
						"</And>",
						"<Eq><FieldRef Name='Document Type'/>","<Value Type='Text'>",document["Document Type"],"</Value></Eq>",
					"</And></Where>"
					);
				SPListItemCollection workloads = project_workload_list.GetItems(query);
				if (workloads.Count == 0)
				{
					throw new Exception(query.Query + ". Can not be found!");
				}
				else
				{
					SPListItem workload= workloads[0];
					workload["Document Count"] = int.Parse(workload["Document Count"].ToString()) + 1;
					workload.Update();
				}
			}
		}


		// 取消对以下方法的注释，以便处理在停用某个功能前引发的事件。

		//public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
		//{
		//}


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
