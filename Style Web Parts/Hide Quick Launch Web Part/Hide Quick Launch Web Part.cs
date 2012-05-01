using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;

namespace Style_Web_Parts.Hide_Quick_Launch_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class Hide_Quick_Launch_Web_Part : WebPart
	{
		protected override void CreateChildControls()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<style>");
			sb.Append("#s4-leftpanel {");
			sb.Append("	display:none;");
			sb.Append("}");
			sb.Append("#MSO_ContentTable {");
			sb.Append("	margin-left:0px;");
			sb.Append("}");
			sb.Append("</style>");
			this.Controls.Add(new LiteralControl(sb.ToString()));
		}
	}
}
