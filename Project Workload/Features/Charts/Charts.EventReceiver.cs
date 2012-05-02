using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Project_Workload.Features.Feature1
{
	/// <summary>
	/// 此类用于处理在激活、停用、安装、卸载和升级功能的过程中引发的事件。
	/// </summary>
	/// <remarks>
	/// 附加到此类的 GUID 可能会在打包期间使用，不应进行修改。
	/// </remarks>

	[Guid("5cae87f6-f58b-4b3a-ba6b-c8e1eeacc27c")]
	public class Feature1EventReceiver : SPFeatureReceiver
	{
		// 取消对以下方法的注释，以便处理激活某个功能后引发的事件。

		public override void FeatureActivated(SPFeatureReceiverProperties properties)
		{
			//SPWeb web = properties.UserCodeSite.RootWeb;
			//SPList list = web.Lists["Projects Workload"];
			//if (list == null)
			//{
			//    SPListCollection lists = web.Lists;
			//    Guid guid = lists.Add("Projects Workload", "", SPListTemplateType.GenericList);
			//    list = web.Lists[guid];
			//    list.Fields["Title"].StaticName = "Year";
			//    list.Fields.Add("Month", SPFieldType.Text, true);
			//    list.Fields.Add("Initial", SPFieldType.Integer, false);
			//    list.Fields.Add("Scope", SPFieldType.Integer, false);
			//    list.Fields.Add("Design", SPFieldType.Integer, false);
			//}
			
		}


		// 取消对以下方法的注释，以便处理在停用某个功能前引发的事件。

		public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
		{
			//SPWeb web = properties.UserCodeSite.RootWeb;
			//SPList list = web.Lists["Projects Workload"];
			//if (list != null)
			//{
			//    list.Delete();
			//}
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
