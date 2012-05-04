using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Style_Web_Parts.Lightbox_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class Lightbox_Web_Part : WebPart
	{
		[WebBrowsable(true), Category("Parameter"),
		 WebDisplayName("jQuery 1.7.x Url. (Required)"),
		 WebDescription("Please fillin the jQuery 1.7.x JS file's URL."),
		 Personalizable(PersonalizationScope.Shared),
		 DefaultValue("")]
		public string jQueryUrl { get; set; }
		[WebBrowsable(true), Category("Parameter"),
		 WebDisplayName("Lightbox2 JS Url. (Required)"),
		 WebDescription("Please fillin the Lightbox2 JS file's URL."),
		 Personalizable(PersonalizationScope.Shared),
		 DefaultValue("")]
		public string lightboxJSUrl { get; set; }
		[WebBrowsable(true), Category("Parameter"),
		 WebDisplayName("Lightbox2 CSS Url. (Required)"),
		 WebDescription("Please fillin the Lightbox2 CSS file's URL."),
		 Personalizable(PersonalizationScope.Shared),
		 DefaultValue("")]
		public string lightboxCSSUrl { get; set; }

		protected override void CreateChildControls()
		{
			string lightboxCSSLink= "<link rel='stylesheet' type='text/css' href='" + lightboxCSSUrl + "' />";
			string lightboxJSLink = "<script src='" + lightboxJSUrl + "'></script>";
			string jQueryJSLink = "<script type='text/javascript' src='" + jQueryUrl + "'></script>";
			this.Controls.Add(new LiteralControl(jQueryJSLink+lightboxJSLink+lightboxCSSLink));
		}
	}
}
