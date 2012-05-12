using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;

namespace News_Carousel.News_Carousel_Web_Part
{
    [ToolboxItemAttribute(false)]
    public class News_Carousel_Web_Part : WebPart
    {
        protected override void CreateChildControls()
        {
			// TODO: 这里是不是可以搞个缓存什么的？
			string component_lib = "/DocLib2";
            SPWeb web = SPContext.Current.Web;
            SPList summary_list = web.Lists["新闻摘要"];
            SPQuery query = new SPQuery();
			query.Query = "<OrderBy><FieldRef Name='Created' Ascending='FALSE' /><FieldRef Name='Modified' Ascending='FALSE' /></OrderBy>";
            query.RowLimit = 5;
            SPListItemCollection summary_items = summary_list.GetItems(query);

            StringBuilder sb = new StringBuilder();
            sb.Append("<link type='text/css' rel='stylesheet' href='"+component_lib+"/rcarousel/rcarousel.css' />");
			sb.Append("<script type='text/javascript' src='" + component_lib + "/jquery/jquery.ui.core.min.js'></script>");
			sb.Append("<script type='text/javascript' src='" + component_lib + "/jquery/jquery.ui.widget.min.js'></script>");
			sb.Append("<script type='text/javascript' src='" + component_lib + "/rcarousel/jquery.ui.rcarousel.min.js'></script>");
            sb.Append("<link type='text/css' rel='stylesheet' href='/carousel/carousel.css' />");
            sb.Append("<script type='text/javascript' src='/carousel/init.js'></script>");
            sb.Append("<div id='container'>");
            sb.Append("    <div id='carousel'>");

            foreach (SPListItem summary in summary_items)
            {
                sb.Append("        <div class='news-summary-div'>");
				sb.Append("            <a class='news-summary-link' href='"); sb.Append(summary["新闻链接"]); sb.Append("'><h3>"); sb.Append(summary["Title"]); sb.Append("</h3></a>");
                sb.Append("            <img class='news-summary-image' src='"); sb.Append(summary["图片"].ToString().Split(',')[0]); sb.Append("' />");
                sb.Append("            <p class='news-summary-phase'>"); sb.Append(summary["摘要"]); sb.Append(".</p>");
                sb.Append("        </div>");
            }

            sb.Append("    </div>");
            sb.Append("    <a href='#' id='ui-carousel-next'><span>next</span></a> ");
            sb.Append("    <a href='#' id='ui-carousel-prev'><span>prev</span></a>");
            sb.Append("    <div id='pages'>");
            sb.Append("    </div>");
            sb.Append("</div>");
            sb.Append("");

            this.Controls.Add(new LiteralControl(sb.ToString()));
        }
    }
}
