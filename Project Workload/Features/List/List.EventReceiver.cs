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

		//public override void FeatureActivated(SPFeatureReceiverProperties properties) { 
		//}
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
