using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;

namespace Style_Web_Parts.List_Image_Web_Part
{
    [ToolboxItemAttribute(false)]
    public class List_Image_Web_Part : WebPart
    {
        protected override void CreateChildControls()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Style>");
            sb.Append("    td.ms-vb2 img{");
            sb.Append("    Height:50px;");
            sb.Append("    }");
            sb.Append("</Style>");
            this.Controls.Add(new LiteralControl(sb.ToString()));
        }
    }
}
