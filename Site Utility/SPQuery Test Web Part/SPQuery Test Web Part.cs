using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.Diagnostics;

namespace Site_Utility.SPQuery_Test_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class SPQuery_Test_Web_Part : WebPart
	{
		protected override void CreateChildControls()
		{
			SPWeb web = SPContext.Current.Web;
			SPList list = web.Lists["人物列表"];
			SPQuery query = new SPQuery();
			// query.Query = "<Where><Eq><FieldRef Name='_x804c__x4f4d_'/><Value Type='Text'>J</Value></Eq></Where>";
			query.Query = "<Where><And><Eq><FieldRef Name='_x6027__x522b_'/><Value Type='Text'>男</Value></Eq><Eq><FieldRef Name='_x804c__x4f4d_'/><Value Type='Text'>J</Value></Eq></And></Where>";

			Stopwatch timer = new Stopwatch();
			timer.Start();
			SPListItemCollection items = list.GetItems(query);
			StringBuilder sb = new StringBuilder();
			foreach (SPListItem item in items)
			{
				sb.Append(item["性别"]);
				sb.Append("，");
			}
			timer.Stop();
			sb.Append("<hr>");
			sb.Append(string.Format("{0:N}",timer.Elapsed.Ticks / 10m));
			LiteralControl lc = new LiteralControl();
			lc.Text = sb.ToString();
			this.Controls.Add(lc);
		}
	}
}
