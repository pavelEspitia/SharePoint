using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Diagnostics;
using System.Text;
using Microsoft.SharePoint.Publishing.Navigation;

namespace Site_Utility.MapProvider_Test_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class MapProvider_Test_Web_Part : WebPart
	{
		protected override void CreateChildControls()
		{
			return;
			SPQuery query = new SPQuery();
			// supports less than 130,000 records
			// query.Query = "<Where><Eq><FieldRef Name='_x804c__x4f4d_'/><Value Type='Text'>J</Value></Eq></Where>";
			query.Query = "<Where><And><Eq><FieldRef Name='_x6027__x522b_'/><Value Type='Text'>男</Value></Eq><Eq><FieldRef Name='_x804c__x4f4d_'/><Value Type='Text'>J</Value></Eq></And></Where>";
			SPWeb web = SPContext.Current.Web;
			PortalSiteMapProvider ps = PortalSiteMapProvider.WebSiteMapProvider;
			PortalWebSiteMapNode psNode = (PortalWebSiteMapNode)ps.FindSiteMapNode(web.ServerRelativeUrl);

			if (psNode != null)
			{
				Stopwatch timer = new Stopwatch();
				timer.Start();
				SiteMapNodeCollection items = ps.GetCachedListItemsByQuery(psNode, "人物列表", query, web);

				StringBuilder sb = new StringBuilder();
				foreach (PortalListItemSiteMapNode item in items)
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
}
