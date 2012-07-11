using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Expense_Report.Expense_Report_List_Events
{
	/// <summary>
	/// 列表项事件
	/// </summary>
	public class Expense_Report_List_Events : SPItemEventReceiver {
		public override void ItemAdding(SPItemEventProperties properties) {
			base.ItemAdding(properties);
		}
		/// <summary>
		/// 已添加项.
		/// </summary>
		public override void ItemAdded(SPItemEventProperties properties) {
			this.EventFiringEnabled = false;
			base.ItemAdded(properties);

			if (properties.List.Title.ToLower().Equals("expense report")) {
				SPList list = properties.List;
				SPListItem item = properties.ListItem;
				string country = item["Country"].ToString();
				string user_name = properties.Web.CurrentUser.Name;

				SPFolder folder = properties.Web.GetFolder(item.ParentList.RootFolder.Url + "/" + country);
				SPListItem country_folder;
				if (!folder.Exists) {
					country_folder = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder,country);
					country_folder.Update();
				} else {
					country_folder = folder.Item;
				}

				SPFolder sub_folder = null;
				foreach (SPFolder sf in folder.SubFolders) {
					if (sf.Name == user_name) {
						sub_folder = sf;
						break;
					}
				}
				SPListItem user_folder;
				if (sub_folder == null) {
					user_folder = list.Items.Add(folder.ServerRelativeUrl, SPFileSystemObjectType.Folder,user_name);
					user_folder.Update();
				} else {
					user_folder = sub_folder.Item;
				}

				try {
					string newFolder = string.Format("{0}/{1}/{2}",  item.ParentList.RootFolder.Url, country, user_name);
					SPListItem item_new = list.AddItem(newFolder, SPFileSystemObjectType.File);
					item_new["Title"] = item["Title"];
					item_new["Amount"] = item["Amount"];
					item_new["Country"] = item["Country"];
					item_new["Approvers"] = item["Approvers"];
					item_new["Created"] = item["Created"];
					item_new["Created By"] = item["Created By"];
					foreach (string file_name in item.Attachments) {
						SPFile file = item.ParentList.ParentWeb.GetFile(
							item.Attachments.UrlPrefix + file_name);
						byte[] data = file.OpenBinary();
						item_new.Attachments.Add(file_name, data);
					}
					item_new.Update();
					//item.CopyTo(properties.WebUrl+"/"+newFolder);
					item.Delete();

					// Start the workflow
					SPWorkflowAssociationCollection workflows = list.WorkflowAssociations;
					SPWorkflowAssociation workflow = workflows.GetAssociationByName("Expense Report Approval Workflow", System.Globalization.CultureInfo.CurrentCulture);
					properties.Web.Site.WorkflowManager.StartWorkflow(item_new, workflow, workflow.AssociationData, true);
				}
				catch (Exception ex) {
					item["Error"] = ex.ToString();
					item.Update();
				}
			}
		}
	}
}
