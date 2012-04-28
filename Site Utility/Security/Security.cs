using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Security.Principal;
using Microsoft.IdentityModel.Claims;

namespace Site_Utility.Security
{
	[ToolboxItemAttribute(false)]
	public class Security : WebPart
	{
		protected override void CreateChildControls()
		{
			//WindowsIdentity wi = WindowsIdentity.GetCurrent();
			IPrincipal ip = HttpContext.Current.User;
			IClaimsIdentity ci = Page.User.Identity as IClaimsIdentity;
			SPUser user = SPContext.Current.Web.CurrentUser;
			user.Email = user.LoginName.Split('\\')[1]+"@works.com";
		}
	}
}
