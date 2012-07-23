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
using System.Text.RegularExpressions;

namespace Excel_Importer {
	public partial class Form1 : System.Windows.Forms.Form {
		public Form1() {
			InitializeComponent();
		}

		// define delegate function for UI updating.
		private delegate void ui_call_back(string value);
		private void update_record_count(string value) {
				lbCounter.Text = value;
		}
		private void update_message(string value) {
				lbMessage.Text = value;
		}

		// Define global variables.
		Web _site = null;
		ClientContext _clientContext = null;
		List _list = null;
		bool _is_running = false;
		private ui_call_back _update_message = null;
		private ui_call_back _update_counter = null;
		int _count = 0;
		char _separator = '|';

		/// <summary>
		/// Trigle the Import action.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdImport_Click(object sender, EventArgs e) {
			try {
				if (cmdImport.Text == "Import") {
					lbCounter.Text = "0 / 0";
					_is_running = true;
					cmdImport.Text = "Pause";
					string ui_selections = cmbLists.SelectedItem.ToString().Trim() + ";" + cmbSheets.SelectedItem.ToString().Trim();
					_update_message = new ui_call_back(update_message);
					_update_counter = new ui_call_back(update_record_count);
					ThreadPool.QueueUserWorkItem(new WaitCallback(import_excel_to_sharepoint_list), ui_selections);
				} else {
					_is_running = false;
					cmdImport.Text = "Import";
					numericUpDown1.Value = _count;
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}

		/// <summary>
		/// Simulate the Folder Tree Structure of the SharePoint List.
		/// </summary>
		class Tree {
			public string Name;
			public string URL;
			public Tree Parent;
			public List<Tree> Children;
		}
		Tree _folder_tree = null; // this is a local copy of the SharePoint List folder tree. to improve performance.

		/// <summary>
		/// Create folder for items, if it's not exists.
		/// </summary>
		/// <param name="parent_folder">The parent folder. Tree object.</param>
		/// <param name="target_folder_name">The name of the target folder under current parent folder.</param>
		/// <returns>The reference to the target folder. Tree object.</returns>
		private Tree create_folder_if_not_exists(Tree parent_folder,string target_folder_name) {
			this.Invoke( _update_message,
				new object[]{"Processing: "+target_folder_name+" "+DateTime.Now.ToLongTimeString()}
			);
			string target_folder_url = parent_folder.URL + "/" + target_folder_name;
			foreach(Tree child_folder in parent_folder.Children){
				if (child_folder.URL.ToLower().Equals(target_folder_url.ToLower())) {
					return child_folder;
				}
			}

			try {
				// TODO: I should try to catch exceptions here.
				ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
				itemCreateInfo.UnderlyingObjectType = FileSystemObjectType.Folder;
				itemCreateInfo.FolderUrl = parent_folder.URL;
				Microsoft.SharePoint.Client.ListItem olistItem = _list.AddItem(itemCreateInfo);
				olistItem["Title"] = target_folder_name;
				olistItem.Update();
				_clientContext.ExecuteQuery();
			}
			catch (Exception ex) {
				if(ex.Message.Contains("exist")){
					//nothing to do with it.
				}else{
					throw ex;
				}
			}

			Tree target_folder = new Tree();
			target_folder.Name = target_folder_name;
			target_folder.URL = target_folder_url;
			target_folder.Parent = parent_folder;
			target_folder.Children = new List<Tree>();
			parent_folder.Children.Add(target_folder);

			return target_folder;
		}

		private void import_excel_to_sharepoint_list(Object stateInfo) {
			try {
				string[] ui_selections = stateInfo.ToString().Split(';');

				_clientContext.Load(_site);
				_list = _site.Lists.GetByTitle(ui_selections[0]);
				_clientContext.ExecuteQuery();

				string commandStr = "select * from [" + ui_selections[1] + "]";
				OleDbDataAdapter command = new OleDbDataAdapter(commandStr, get_conn_string());
				DataTable data = new DataTable();
				command.Fill(data);
				this.Invoke(
					_update_counter,
					new object[] { _count + " / " + data.Rows.Count }
				);

				// prepar for the loop
				DataTable table = new DataTable();
				ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();

				if (_folder_tree == null) {
					_folder_tree = new Tree();
					_folder_tree.URL = String.Format("{0}/Lists/{1}", _site.ServerRelativeUrl == "/" ? "" : _site.ServerRelativeUrl, ui_selections[0]);
					_folder_tree.Name = "ROOT";
					_folder_tree.Children = new List<Tree>();
				}
				int record_number = 0;
				_count = int.Parse(numericUpDown1.Value.ToString());
				foreach (DataRow row in data.Rows) {
					if (!_is_running) return;
					record_number++;
					if (record_number <= _count) continue;
					// check if need to created Folders for imported items
					string new_folder_relative_url = "";
					Tree new_folder = null;
					Tree parent_folder = _folder_tree;
					foreach (DataGridViewRow mapping_row in dgMapping.Rows) {
						// if the Folder Level column is not null, then, put item into this folder
						// the folder level will depends on the sequence of the folder columns apprea in the Mapping GridView. TODO: maybe improved in the future.
						if (mapping_row.Cells[3].Value != null) {
							string folder_name = format_folder_name(row[mapping_row.Cells[0].Value.ToString()].ToString());
							new_folder = create_folder_if_not_exists(parent_folder, folder_name);
							new_folder_relative_url += folder_name + "/";
							parent_folder = new_folder;
						}
					}
					if (new_folder != null) {
						itemCreateInfo.FolderUrl = new_folder.URL;
					}

					Microsoft.SharePoint.Client.ListItem listItem = _list.AddItem(itemCreateInfo);
					// Item Value
					foreach (DataGridViewRow mapping_row in dgMapping.Rows) {
						if (mapping_row.Cells[1].Value != null) {
							if (mapping_row.Cells[2].Value != null) {
								if (string.IsNullOrEmpty(table.TableName)) {
									table.TableName = mapping_row.Cells[2].Value.ToString();
								} else {
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
								if (row[mapping_row.Cells[0].Value.ToString()] != null) {	// The column must have value, or, it will be ignored.
									if (!string.IsNullOrEmpty(row[mapping_row.Cells[0].Value.ToString()].ToString())) {
										string[] col_values = row[mapping_row.Cells[0].Value.ToString()].ToString().Split(_separator);
										if (table.Rows.Count == 0) {
											for (int i = 0; i < col_values.Length; i++) {
												table.Rows.Add(new object[] { col_values[i] });
											}
										} else {
											int i = 0;
											for (i = 0; i < Math.Min(col_values.Length, table.Rows.Count); i++) {
												table.Rows[i][tcol] = col_values[i];
											}
											if (col_values.Length > table.Rows.Count) {
												for (int j = i; j < col_values.Length; j++) {
													DataRow new_row = table.Rows.Add();
													new_row[tcol] = col_values[j];
												}
											}
										}
									}
								}
							} else {
								string value = row[mapping_row.Cells[0].Value.ToString()].ToString();
								if (!string.IsNullOrEmpty(value)) {
									// Check the Date-Time format, for some date column will have time in it, we must remove the time first before we can put it into SharePoint.
									Regex reg = new Regex(@" [0-9]+:[0-9]+[AP]M$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
									if (reg.IsMatch(value)) {
										value = reg.Replace(value, "");
									}
									listItem[mapping_row.Cells[1].Value.ToString()] = value;
								}
							}
						}
					}
					if (table.Columns.Count > 0) {
						listItem[table.TableName] = this.ToHtmlTable(table);
						table.TableName = null;
						table.Clear();
						table.Columns.Clear();
					}
					listItem.Update();
					_count++;
					this.Invoke(
						_update_counter,
						new object[] { _count + " / " + data.Rows.Count }
					);
					if (_count % 20 == 0) {
						_clientContext.ExecuteQuery(); // TODO: Is there any other way to improve this?
					}
				}
				_clientContext.ExecuteQuery(); // for the last item
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}

		private void cmdCheckExcelFile_Click(object sender, EventArgs e) {
			try {
				update_message("Analyzing the Excel file. "+DateTime.Now.ToLongTimeString());
				OleDbConnection conn = new OleDbConnection(get_conn_string());
				conn.Open();
				DataTable sheet_names = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
				cmbSheets.Items.Clear();
				foreach (DataRow row in sheet_names.Rows) {
					cmbSheets.Items.Add(row[2].ToString().Replace("'", ""));
				}
				if (sheet_names.Rows.Count > 0) {
					cmbSheets.SelectedIndex = 0;
				}
				conn.Close();
				update_message("Excel file is ready. " + DateTime.Now.ToLongTimeString());
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}
		private string get_conn_string() {
			return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+txtExcel.Text+";Extended Properties=Excel 8.0;";
		}
		private string get_site_url() {
			if (chkIsDevelop.Checked)
				return "http://dev:8100";
			else
				return txtSiteUrl.Text;
		}
		private void cmdValidateSharePointSite_Click(object sender, EventArgs e) {
			try {
				update_message("Validating the SharePoint site. " + DateTime.Now.ToLongTimeString());
				_clientContext = new ClientContext(get_site_url());
				if (chkUseClaims.Checked) {
					_clientContext = ClaimClientContext.GetAuthenticatedContext(get_site_url());
				}
				_site = _clientContext.Web;
				ListCollection lists = _site.Lists;
				_clientContext.Load(lists);
				_clientContext.ExecuteQuery();
				cmbLists.Items.Clear();
				List<string> sorted_lists = new List<string>();
				foreach (List list in lists) {
					sorted_lists.Add(list.Title);
				}
				sorted_lists.Sort();
				cmbLists.Items.AddRange(sorted_lists.ToArray<string>());
				if (lists.Count > 0) {
					cmbLists.SelectedIndex = 0;
				}
				update_message("SharePoint site is ready. " + DateTime.Now.ToLongTimeString());
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
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


			List list = _site.Lists.GetByTitle(cmbLists.SelectedItem.ToString());
			FieldCollection fields = list.Fields;
			_clientContext.Load(fields);
			_clientContext.ExecuteQuery();

			foreach (DataGridViewRow row in dgMapping.Rows) {
				if (row.Cells[0].Value == null) continue; //TODO: How can that happen?
				DataGridViewComboBoxCell col = new DataGridViewComboBoxCell();
				List<string> sorted_columns = new List<string>();
				string field_name = row.Cells[0].Value.ToString().ToLower();
				for(int i=0;i<fields.Count;i++){
					if (!fields[i].FromBaseType || 
						fields[i].InternalName == "Title"||
						fields[i].InternalName=="Created"||
						fields[i].InternalName=="Author"||
						fields[i].InternalName=="Modified"||
						fields[i].InternalName=="Editor"
						) {
						string item_string = fields[i].InternalName;
						sorted_columns.Add(item_string);
					}
				}
				sorted_columns.Sort();
				col.Items.AddRange(sorted_columns.ToArray<string>());
				for (int i = 0; i < col.Items.Count; i++) {
					if (field_name.Contains(col.Items[i].ToString().Replace("_x0020_"," ").Replace("_x002f_","/").Replace("_x002e_","#").ToLower())) {
						col.Value = col.Items[i];
						break;
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

		private void cmdSelectFile_Click(object sender, EventArgs e) {
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				txtExcel.Text = openFileDialog1.FileName;
			}
		}

		private string format_folder_name(string name) {
			string folder_name = name.Replace("&", " N ").Replace('\'', ' ').Replace('.', '_').Replace('/', ' ').Replace('\\', ' '); // SharePoint doesn't accept '&', so, replace it with '-'
			folder_name = folder_name.Trim();
			return folder_name;
		}
	}
}
