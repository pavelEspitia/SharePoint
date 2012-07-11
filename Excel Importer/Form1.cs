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

namespace Excel_Importer {
	public partial class Form1 : System.Windows.Forms.Form {
		public Form1() {
			InitializeComponent();
		}
		Web site = null;
		ClientContext clientContext = null;
		private void cmdImport_Click(object sender, EventArgs e) {

			//ClientContext clientContext = new ClientContext(get_site_url());
			//if (chkUseClaims.Checked) {
			//    clientContext.ExecutingWebRequest +=
			//        new EventHandler<WebRequestEventArgs>(ctx_MixedAuthRequest);
			//    clientContext.AuthenticationMode = ClientAuthenticationMode.Default;
			//    clientContext.Credentials = System.Net.CredentialCache.DefaultCredentials;
			//}
			List list = site.Lists.GetByTitle(cmbLists.SelectedItem.ToString());
			clientContext.ExecuteQuery();

			lbCounter.Text = "0 / 0";
			string commandStr = "select * from [" + cmbSheets.SelectedItem.ToString() + "]";
			OleDbDataAdapter command = new OleDbDataAdapter(commandStr, get_conn_string());
			DataTable data = new DataTable();
			command.Fill(data);
			lbCounter.Text = "0 / " + data.Rows.Count;

			int count = 0;
			ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
			foreach (DataRow row in data.Rows) {
				ListItem listItem = list.AddItem(itemCreateInfo);

				foreach (DataGridViewRow mapping_row in dgMapping.Rows) {
					if (mapping_row.Cells[1].Value!=null) {
						listItem[mapping_row.Cells[1].Value.ToString()] = row[mapping_row.Cells[0].Value.ToString()].ToString();
					}
				}
				listItem.Update();
				count++;
				lbCounter.Text = count + " / " + data.Rows.Count;
				if (count % 500 == 0) {
					clientContext.ExecuteQuery();
				}
			}
			clientContext.ExecuteQuery();
		}

		private void button2_Click(object sender, EventArgs e) {
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
				return "http://dev:8100";
			else
				return txtSiteUrl.Text;
		}
		private void button1_Click(object sender, EventArgs e) {
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
	}
}
