using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace List_Update_Folder_Time.Item_Changed_Event
{
	/// <summary>
	/// 列表项事件
	/// </summary>
	public class ItemChangedEvent : SPItemEventReceiver {
		/// <summary>
		/// 已添加项.
		/// </summary>
		public override void ItemAdded(SPItemEventProperties properties) {
			base.ItemAdded(properties);
			process_event(properties);
		}

		/// <summary>
		/// 已更新项.
		/// </summary>
		public override void ItemUpdated(SPItemEventProperties properties) {
			base.ItemUpdated(properties);
			process_event(properties);
		}

		/*
		/// <summary>
		/// 已向该项添加附件.
		/// </summary>
		public override void ItemAttachmentAdded(SPItemEventProperties properties)
		{
			base.ItemAttachmentAdded(properties);
			process_event(properties);
		}
		 */

		protected void process_event(SPItemEventProperties properties) {
			if (properties.ListItem.Url.Contains("/")) {
				string url = properties.ListItem.Url.Substring(0, properties.ListItem.Url.LastIndexOf('/'));
				foreach (SPListItem item in properties.List.Folders) {
					if (item.Folder.Url.Equals(url)) {
						update_folder_time(item.Folder);
						break;
					}
				}
			}
		}
		protected void update_folder_time(SPFolder current_folder) {
			if (current_folder == null || !current_folder.Url.Contains("/")) return;
			current_folder.SetProperty("Name", current_folder.Name);
			current_folder.Update();
			update_folder_time(current_folder.ParentFolder);
		}
	}
}
