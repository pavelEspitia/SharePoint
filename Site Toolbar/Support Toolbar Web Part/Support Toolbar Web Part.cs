using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Site_Toolbar.Support_Toolbar_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class Support_Toolbar_Web_Part : WebPart
	{
		protected override void CreateChildControls()
		{
			SupportToolbarMenu stm = new SupportToolbarMenu();
			this.Controls.Add(stm);
		}
	}

	public class SupportToolbarMenu : ToolBarMenuButton
	{
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			FeatureMenuTemplate ft = new FeatureMenuTemplate();
			ft.ID = "SupportToolbar.FeatureMenuTemplate";
			ft.Location = "Works.SupportToolbar";
			ft.GroupId = "SupportToolbar";
			ft.FeatureScope = "Web";
			base.MenuTemplateControl.Controls.Add(ft);

			base.MenuTemplateControl.LargeIconMode = true;
			base.MenuControl.HoverCellActiveCssClass = "ms-SPLink ms-SpLinkButtonActive ms-welcomeMenu";
			base.MenuControl.HoverCellInActiveCssClass = "ms-SPLink ms-SpLinkButtonInActive ms-welcomeMenu";
			base.MenuControl.MenuFormat = MenuFormat.ArrowAlwaysVisible;
			base.MenuControl.UseDivForMenu = true;
			base.MenuControl.ArrowImageUrl = "/_layouts/images/fgimg.png";
			base.MenuControl.ArrowImageWidth = 5;
			base.MenuControl.ArrowImageHeight = 3;
			base.MenuControl.ArrowImageOffsetX = 0;
			base.MenuControl.ArrowImageOffsetY = 0x1eb;

			base.MenuControl.Text = "技术支持";
			base.MenuControl.Width = new System.Web.UI.WebControls.Unit("100px");

			base.AddMenuItemSeparator();
			base.AddMenuItem("SupportToolbar.About",
				"网站介绍",
				"/_layouts/images/centraladmin_security_informationpolicy_32x32.png",
				"查看关于网站的介绍",
				null,
				"openAboutDialog();");
		}
	}
}
