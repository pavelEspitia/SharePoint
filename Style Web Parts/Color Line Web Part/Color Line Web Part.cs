using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;

namespace Style_Web_Parts.Color_Line_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class Color_Line_Web_Part : WebPart
	{
		protected override void CreateChildControls()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<script>");
			sb.Append("$(document).ready(function(){");
			sb.Append("	$('td[class=\"ms-vb2\"]').each(function(index){");
			sb.Append("		if($(this).html()=='错误'){");
			sb.Append("			$(this).css('color','red');	");
			sb.Append("		}");
			sb.Append("		if($(this).html()=='警告'){");
			sb.Append("			$(this).css('color','orange');");
			sb.Append("		}");
			sb.Append("	});");
			sb.Append("});");
			sb.Append("</script>");
			this.Controls.Add(new LiteralControl(sb.ToString()));
		}
	}
}
