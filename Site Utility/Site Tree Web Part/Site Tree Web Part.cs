using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using Microsoft.SharePoint.Administration;

namespace Site_Utility.Site_Tree_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class Site_Tree_Web_Part : WebPart
	{
		protected override void CreateChildControls()
		{
			// Get Application
			SPSite site = SPContext.Current.Site;
			Uri app_uri = new Uri(site.Protocol+"//"+site.HostName+":"+site.Port+"/");
			SPWebApplication app = SPWebApplication.Lookup(app_uri);

			// Loop for all site collections
			SPSiteCollection sites = app.Sites;
			StringBuilder sb = new StringBuilder();
			foreach (SPSite s in sites)
			{
				sb.Append("<div class='div-site-collection'>");
				sb.Append("<ul class='ul-site-collection'>");
				build_webs(s.RootWeb, sb);
				sb.Append("</ul>");
				sb.Append("</div>");
			}

			this.Controls.Add(new LiteralControl(sb.ToString()));
		}

		protected void build_webs(SPWeb parent_web, StringBuilder sb)
		{
			// Show Parent Web
			sb.Append("<li class='li-web-site'>");
			build_web_link(parent_web, sb);

			// Show Sub-webs
			if (parent_web.Webs.Count > 0)
			{
				#region Sub Webs
				sb.Append("<ul class='ul-web-site'>");
				foreach (SPWeb web in parent_web.Webs)
				{
					if (web.Webs.Count > 0)
					{
						build_webs(web, sb);
					}
					else
					{
						sb.Append("<li>");
						build_web_link(web, sb);
						sb.Append("</li>");
					}
				}
				sb.Append("</ul>");
				#endregion
			}
			// Finish tag
			sb.Append("</li>");
		}

		protected void build_web_link(SPWeb web, StringBuilder sb){
			sb.Append("<img class='img-web-site' src='");
			if (string.IsNullOrEmpty(web.SiteLogoUrl)) { sb.Append("/_layouts/images/siteIcon.png"); }
			else { sb.Append(web.SiteLogoUrl); }
			sb.Append("'></img>");
			sb.Append("<a class='a-web-site' title='");
			sb.Append(web.Description);
			sb.Append("' href='");
			sb.Append(web.Url);
			sb.Append("'>");
			sb.Append(web.Title);
			sb.Append("</a>");
		}

		}
	}
}
