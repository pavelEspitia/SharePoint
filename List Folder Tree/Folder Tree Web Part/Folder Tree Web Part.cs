using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;

namespace List_Folder_Tree.Folder_Tree_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class Folder_Tree_Web_Part : WebPart
	{
		/// <summary>
		/// The Document Library's Name property parameter.
		/// Users can config this from the web page where this webpart putted.
		/// </summary>
		[WebBrowsable(true), Category("Parameter"),
		 WebDisplayName("Target Document Library's Name. (Required)"),
		 WebDescription("The Document Library's Name, for which you would like to show in the tree view."),
		 Personalizable(PersonalizationScope.Shared),
		 DefaultValue("")]
		public string DocumentLibraryName { get; set; }

		[WebBrowsable(true), Category("Parameter"),
		 WebDisplayName("zTree style file URL. (Required)"),
		 WebDescription("Please fillin the zTree style file (zTreeStyle.css)'s URL."),
		 Personalizable(PersonalizationScope.Shared),
		 DefaultValue("")]
		public string zTreeStyleURL { get; set; }

		[WebBrowsable(true), Category("Parameter"),
		 WebDisplayName("zTree script file URL. (Required)"),
		 WebDescription("Please fillin the zTree script file (jquery.ztree.core-x.x.min.js)'s URL."),
		 Personalizable(PersonalizationScope.Shared),
		 DefaultValue("")]
		public string zTreeScriptURL { get; set; }

		[WebBrowsable(true), Category("Parameter"),
		 WebDisplayName("Initiation script file URL. (Required)"),
		 WebDescription("Please fillin the initiation script file (init.js)'s URL."),
		 Personalizable(PersonalizationScope.Shared),
		 DefaultValue("")]
		public string InitScriptURL { get; set; }

		[WebBrowsable(true), Category("Parameter"),
		 WebDisplayName("View Page URL. (Optional)"),
		 WebDescription("Please fillin the View Page's URL on which you placed this webpart. Default value is the document library's default view page's URL."),
		 Personalizable(PersonalizationScope.Shared),
		 DefaultValue("")]
		public string ViewURL { get; set; }

		/*
		public enum SortFieldEnum
		{
			NameASC = 0,
			NameDESC
		};

		[WebBrowsable(true), Category("Parameter"),
		 DefaultValue(SortFieldEnum.NameASC),
		 WebDisplayName("Sort field. (Optional)"),
		 WebDescription("Sort field.")]
		public SortFieldEnum SortField { get; set; }
		 */

		private StringBuilder _nodes_string = new StringBuilder();
		private string _root_folder = null;	// The query string parameter's value.
		private string _base_url = null; // Current document library's relative URL.
		private string _website_relative_url = null; // Current website's relative URL.
		private string _sort_field = null;
		private string _sort_dir = null;
		/// <summary>
		/// Retieve folders and subfolders for the given document library.
		/// Then, put the result in a ECMAScript (JavaScript) variable, named "zTreeNodes".
		/// </summary>
		protected override void CreateChildControls()
		{
			// Get query string parameters
			_root_folder = this.Context.Request.QueryString["RootFolder"];
			if (_root_folder == null) _root_folder = "";
			_sort_field = this.Context.Request.QueryString["SortField"];
			_sort_dir = this.Context.Request.QueryString["SortDir"];

			// Prepare website's relative url.
			SPWeb web = SPContext.Current.Web;
			_website_relative_url = web.ServerRelativeUrl;
			if (_website_relative_url != "/") { _website_relative_url += "/"; }

			// Begin to generate the JSON string for the zTree.
			try
			{
				SPList lib = web.Lists[DocumentLibraryName];
				int subfolders_count = lib.RootFolder.SubFolders.Count;
				if (string.IsNullOrEmpty(ViewURL)) { _base_url = lib.DefaultViewUrl; } else { _base_url = ViewURL; }
				_nodes_string.Append("[{target:'_self',url:'" + _base_url + "',name:'");
				_nodes_string.Append(DocumentLibraryName);
				_nodes_string.Append("'");
				if (_root_folder != "") { _nodes_string.Append(",open:true"); };
				if (subfolders_count > 0) { _nodes_string.Append(",children:["); parse_subfolders(ref _nodes_string, lib.RootFolder); _nodes_string.Append("]"); }
				_nodes_string.Append("}]");

				string style_link = "<link rel='stylesheet' type='text/css' href='" + zTreeStyleURL + "'>";
				string ztree_script = "<script src='" + zTreeScriptURL + "'></script>";
				string init_script = "<script type='text/javascript' src='" + InitScriptURL + "'></script>";
				this.Controls.Add(new LiteralControl("<script>var zTreeNodes=" + _nodes_string.ToString() + ";</script>" + style_link + ztree_script + init_script + "<ul id=\"ul_folder_tree\" class=\"ztree\"></ul>"));
			}
			catch (NullReferenceException nullex) { this.Controls.Add(new LiteralControl("Please make sure you've choosen the corrent document library's name." + nullex.Message)); }
			catch (Exception ex) { this.Controls.Add(new LiteralControl(ex.Message)); }
		}

		/// <summary>
		/// Get child folders for current parent folder.
		/// </summary>
		/// <param name="sb">The StringBuilder object which will hold the child folders' tree nodes JSON string.</param>
		/// <param name="parent_folder">The Parent Folder.</param>
		private void parse_subfolders(ref StringBuilder sb, SPFolder parent_folder)
		{
			SPFolderCollection folders = parent_folder.SubFolders;

			// first, fill these folders into arrays.
			string[] folder_name_array = new string[folders.Count];
			SPFolder[] folder_array = new SPFolder[folders.Count];
			for (int i = 0; i < folders.Count; i++)
			{
				folder_name_array[i] = folders[i].Name;
				folder_array[i] = folders[i];
			}

			// sort the folders by their name, in ascent order.
			Array.Sort(folder_name_array, folder_array);
			// if current view is manually sorted by name in descent order, 
			// then reverse the folder array.
			if (!string.IsNullOrEmpty(_sort_field) && !string.IsNullOrEmpty(_sort_dir) &&
				_sort_field.ToLower().Contains("name") &&
				_sort_dir.ToLower() == "desc")
			{
				Array.Reverse(folder_array);
			}

			// format the JSON string for each folder
			foreach (SPFolder child_folder in folder_array)
			{
				string child_folder_name = child_folder.Name.ToLower();
				if (child_folder_name == "forms"||
					child_folder_name=="attachments"||
					child_folder_name=="item") continue; // exclude the built-in Forms folder.
				format_json(ref sb, child_folder);
			}

			if (folders.Count > 0)
			{
				// We must remove the last comma.
				sb.Remove(sb.Length - 1, 1);
			}
		}

		/// <summary>
		/// Formate tree node JSON string for current folder, and its subfolders, if has any.
		/// </summary>
		/// <param name="sb">The StringBuilder object which will hold the child folders' tree nodes JSON string.</param>
		/// <param name="folder">Current Folder.</param>
		private void format_json(ref StringBuilder sb, SPFolder folder)
		{
			sb.Append("{name:'");
			sb.Append(folder.Name);
			sb.Append("'");

			sb.Append(",target:'_self',url:'");
			sb.Append(_base_url);
			sb.Append("?RootFolder=");
			sb.Append(_website_relative_url);
			sb.Append(folder.Url);
			sb.Append("'");


			if (folder.SubFolders.Count > 0)
			{
				// Open current folder's parent folders, but not itself.
				// This will provide a nice view for folder tree.
				if (_root_folder.Contains(folder.Url) && _root_folder != (_website_relative_url + folder.Url))
				{
					sb.Append(",open:true");
				}
				sb.Append(",children:[");
				parse_subfolders(ref sb, folder);
				sb.Append("]");
			}
			sb.Append("},");
		}

	}
}
