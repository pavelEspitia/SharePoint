using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace List_Attachment_Dialog.Document_Dialog_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class Document_Dialog_Web_Part : WebPart
	{
		protected override void CreateChildControls()
		{
			LiteralControl script = new LiteralControl();
			script.Text =
				@"<script type=""text/jscript"">
					$(document).ready(function() {
					    $(""a[onclick*='DispEx']"").each( function(data){
					        var href = this.href;
					        this.href = '" + SPContext.Current.Web.Url + @"/_layouts/download.aspx?SourceURL=' + this.href;
					    });
					    $(""a[onclick*='DispEx']"").removeAttr('onclick');
					});
				</script>";
			this.Controls.Add(script);
		}
	}
}
