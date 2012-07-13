using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using Microsoft.SharePoint.Client;
using MSDN.Samples.ClaimsAuth;
using System.Threading;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace Excel_Importer {
	public partial class Form1 : System.Windows.Forms.Form {
		public Form1() {
			InitializeComponent();
		}
		private delegate void ui_call_back(string value);
		private void update_record_count(string value) {
				lbCounter.Text = value;
		}
		Web site = null;
		ClientContext clientContext = null;
		List list = null;
		bool is_running = false;
		private void cmdImport_Click(object sender, EventArgs e) {
			if (cmdImport.Text == "Import") {
				lbCounter.Text = "0 / 0";
				is_running = true;
				cmdImport.Text = "Stop";
				string list_items = cmbLists.SelectedItem.ToString() + ";" + cmbSheets.SelectedItem.ToString();
				ThreadPool.QueueUserWorkItem(new WaitCallback(import),list_items);
			} else {
				is_running = false;
				cmdImport.Text = "Import";
			}
		}
		private string GetFolderCI(ClientContext clientContext, string list_name,string site_url, String folderName) {
			Folder existingFolder = null;

			//List list = site.Lists.GetByTitle(list_name);
			//clientContext.ExecuteQuery();


			if (list != null) {
				FolderCollection folders = list.RootFolder.Folders;
				String folderUrl = String.Format("{0}/Lists/{1}/{2}", site_url, list_name, folderName);
				IEnumerable<Folder> existingFolders = clientContext.LoadQuery(
					folders.Include(
					folder => folder.ServerRelativeUrl)
					);
				clientContext.ExecuteQuery();
				foreach (Folder f in existingFolders) {
					if (f.ServerRelativeUrl.ToLower().Equals(folderUrl.ToLower())) {
						existingFolder = f;
						break;
					}
				}

				if (existingFolder == null) {
					ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
					itemCreateInfo.UnderlyingObjectType = FileSystemObjectType.Folder;
					Microsoft.SharePoint.Client.ListItem olistItem = list.AddItem(itemCreateInfo);
					olistItem["Title"] = folderName;
					olistItem.Update();
					clientContext.ExecuteQuery();
					return folderUrl;
				}
				else {
					return existingFolder.ServerRelativeUrl;
				}
			}
			else {
				return null;
			}

		}
		private void import(Object stateInfo) {
			string [] list_items = stateInfo.ToString().Split(';');
			clientContext.Load(site);
			list = site.Lists.GetByTitle(list_items[0]);
			clientContext.ExecuteQuery();

			string commandStr = "select * from [" + list_items[1] + "]";
			OleDbDataAdapter command = new OleDbDataAdapter(commandStr, get_conn_string());
			DataTable data = new DataTable();
			command.Fill(data);
			this.Invoke(
				new ui_call_back(update_record_count),
				new object[]{"0 / " + data.Rows.Count}
			);

			int count = 0;
			DataTable table=new DataTable();
			Microsoft.SharePoint.Client.ListItem old_ListItem = null;
			ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
			foreach (DataRow row in data.Rows) {
				if (!is_running) return;

				// Folder
				string newFolder = null;
				foreach (DataGridViewRow mapping_row in dgMapping.Rows) {
					if (mapping_row.Cells[3].Value != null) {
						string folder_name = row[mapping_row.Cells[0].Value.ToString()].ToString().Replace('&','-');
						string folder = GetFolderCI(clientContext, list_items[0], site.ServerRelativeUrl, folder_name);
						if (folder != null) {
							newFolder = folder;
							break; // TODO: support 1 folder only
						}
					}
				}
				if (newFolder != null) {
					itemCreateInfo.FolderUrl = newFolder;
				} 
				Microsoft.SharePoint.Client.ListItem listItem = list.AddItem(itemCreateInfo);

				// Item Value
				foreach (DataGridViewRow mapping_row in dgMapping.Rows) {
					if (mapping_row.Cells[1].Value!=null) {
						if (mapping_row.Cells[2].Value != null) {
							if (string.IsNullOrEmpty(table.TableName)) {
								table.TableName = mapping_row.Cells[2].Value.ToString();
							}
							else {
								if (table.TableName != mapping_row.Cells[2].Value.ToString()) {
									listItem[table.TableName] = this.ToHtmlTable(table);
									table.TableName = null;
									table.Clear();
									table.Columns.Clear();
									table.TableName = mapping_row.Cells[2].Value.ToString();
								}
							}
							DataColumn tcol = new DataColumn(mapping_row.Cells[0].Value.ToString());
							table.Columns.Add(tcol);
							string[] values = row[mapping_row.Cells[0].Value.ToString()].ToString().Split(',');
							if (table.Rows.Count == 0) {
								for (int i = 0; i < values.Length; i++) {
									table.Rows.Add(new object[] { values[i] });
								}
							} else {
								for(int i=0;i<Math.Min(values.Length,table.Rows.Count);i++){
									table.Rows[i][tcol] = values[i];
								}
							}
						} else {
							if (table.Rows.Count>0) {
								old_ListItem[table.TableName] = this.ToHtmlTable(table);
								old_ListItem.Update();
								table.TableName = null;
								table.Clear();
								table.Columns.Clear();
							} 
							listItem[mapping_row.Cells[1].Value.ToString()] = row[mapping_row.Cells[0].Value.ToString()].ToString();
						}
					}
				}
				listItem.Update();
				old_ListItem = listItem;
				count++;
				this.Invoke(
					new ui_call_back(update_record_count),
					new object[]{count+" / " + data.Rows.Count}
				);
				if (count % 20 == 0) {
					clientContext.ExecuteQuery();
				}
			}
			clientContext.ExecuteQuery();
		}

		private void cmdCheckExcelFile_Click(object sender, EventArgs e) {
			OleDbConnection conn = new OleDbConnection(get_conn_string());
			conn.Open();
			DataTable sheet_names = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
			cmbSheets.Items.Clear();
			foreach (DataRow row in sheet_names.Rows) {
				cmbSheets.Items.Add(row[2].ToString().Replace("'",""));
			}
			if (sheet_names.Rows.Count > 0) {
				cmbSheets.SelectedIndex = 0;
			}
			conn.Close();
		}
		private string get_conn_string() {
			return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+txtExcel.Text+";Extended Properties=Excel 8.0;";
		}
		private string get_site_url() {
			if (chkIsDevelop.Checked)
				return "https://intranet.works.com/finance";
			else
				return txtSiteUrl.Text;
		}
		private void cmdValidateSharePointSite_Click(object sender, EventArgs e) {
			clientContext = new ClientContext(get_site_url());
			if (chkUseClaims.Checked) {
				clientContext = ClaimClientContext.GetAuthenticatedContext(get_site_url());
				//clientContext.ExecutingWebRequest +=
				//    new EventHandler<WebRequestEventArgs>(ctx_MixedAuthRequest);
				//clientContext.AuthenticationMode = ClientAuthenticationMode.Default;
				//clientContext.Credentials = System.Net.CredentialCache.DefaultCredentials;
			}
			site = clientContext.Web;
			ListCollection lists = site.Lists;
			clientContext.Load(lists);
			clientContext.ExecuteQuery();
			cmbLists.Items.Clear();
			foreach (List list in lists) {
				cmbLists.Items.Add(list.Title);
			}
			if (lists.Count > 0) {
				cmbLists.SelectedIndex = 0;
			}
		}

		private void cmbLists_SelectedIndexChanged(object sender, EventArgs e) {
		}

		private void cmbSheets_SelectedIndexChanged(object sender, EventArgs e) {
		}

		private void cmdMap_Click(object sender, EventArgs e) {
			dgMapping.Rows.Clear();

			string commandStr = "select top 1 * from ["+cmbSheets.SelectedItem.ToString()+"]";
			OleDbDataAdapter command = new OleDbDataAdapter(commandStr,get_conn_string());
			DataTable data = new DataTable();
			command.Fill(data);

			foreach (DataColumn col in data.Columns) {
				int row_index = dgMapping.Rows.Add();
				DataGridViewRow row = dgMapping.Rows[row_index];
				row.Cells[0].Value = col.ColumnName;
				//row.Cells[3].Value = true;
			}


			List list = site.Lists.GetByTitle(cmbLists.SelectedItem.ToString());
			FieldCollection fields = list.Fields;
			clientContext.Load(fields);
			clientContext.ExecuteQuery();

			foreach (DataGridViewRow row in dgMapping.Rows) {
				DataGridViewComboBoxCell col = new DataGridViewComboBoxCell();
				for(int i=0;i<fields.Count;i++){
					if (!fields[i].FromBaseType || 
						fields[i].InternalName == "Title"||
						fields[i].InternalName=="Created"||
						fields[i].InternalName=="Author"||
						fields[i].InternalName=="Modified"||
						fields[i].InternalName=="Editor"
						) {
						string item_string = fields[i].InternalName;
						col.Items.Add(item_string);
						if (fields[i].Title == row.Cells[0].ToString() ||
							fields[i].InternalName == row.Cells[0].ToString()) {
						}
					}
				}
				row.Cells[1] = col;
			}
		}
		//void ctx_MixedAuthRequest(object sender, WebRequestEventArgs e) {
		//    try {
		//        //Add the header that tells SharePoint to use Windows authentication.
		//        e.WebRequestExecutor.RequestHeaders.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
		//    }
		//    catch (Exception ex) {
		//        MessageBox.Show("Error setting authentication header: " + ex.Message);
		//    }
		//}

		/// <summary>
		/// Convert DataTable to Html Table.
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <returns>Html string</returns>
		private string ToHtmlTable(DataTable dt) {
			GridView gridView = new GridView();
			gridView.AutoGenerateColumns = true;
			gridView.DataSource = dt;
			gridView.DataBind();

			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			HtmlTextWriter htw = new HtmlTextWriter(sw);

			gridView.RenderControl(htw);

			return sb.ToString();
		}
	}
}
