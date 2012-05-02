using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace List_Require_Attachment.List_Require_Attachment_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class List_Require_Attachment_Web_Part : WebPart
	{
		protected override void CreateChildControls()
		{
			LiteralControl script = new LiteralControl();
			script.Text =
				@"<script>
					function PreSaveAction() {
						var file_count = 0;
						$(""input[name^='fileupload']"").each(function() {
							if ($(this).val() != """") {
								//alert($(this).val());
								file_count = file_count + 1;
							}
						});
						if(file_count>0){
							return true;
						}else{
							var options={url:'" + SPContext.Current.Web.Url + @"/Require Attachment Module/message.html'};
							SP.UI.ModalDialog.showModalDialog(options);
							return false;
						}
					}
				</script>";
			this.Controls.Add(script);
		}
	}
}
