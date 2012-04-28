using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;

namespace Style_Web_Parts.Child_List_Style
{
	[ToolboxItemAttribute(false)]
	public class Child_List_Style : WebPart
	{
		protected override void CreateChildControls()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<style>");
			sb.Append("	tr.ms-vhltr th{");
			sb.Append("		border-bottom:2px solid silver !important;");
			sb.Append("		background-color:#cccccc;");
			sb.Append("	}");
			sb.Append("	td.ms-vb-title div.ms-vb {");
			sb.Append("		font-size:9pt;");
			sb.Append("	}");
			sb.Append("	tr.ms-itmhover td{");
			sb.Append("		border-bottom:1px solid silver !important;");
			sb.Append("	}");
			sb.Append("</style>");
			this.Controls.Add(new LiteralControl(sb.ToString()));
		}
	}
}
