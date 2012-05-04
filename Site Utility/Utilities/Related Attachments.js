<!--Jony-Script-Begin-->
	<SharePoint:ScriptLink Name="SP.js" runat="server" OnDemand="true" Localizable="false"/>
	<script type="text/javascript" src="https://china.works.com/Component/jquery/jquery-1.6.2.min.js"></script>
	<script type="text/javascript" src="https://china.works.com/Component/spservices/jquery.SPServices-0.7.0.min.js"></script>
	<script type="text/javascript">
		var attachmentFiles;
		var task_attachmentFiles;
		var list_name= '';		
		var attachment_title = '';
		var attachment_place_holder_text = '';
		
		function getWebProperties() {
			var itemId = get_related_content_id(list_name.replace(" ","%20"));
			if(itemId==0)return;
			attachment_title='Request attachments:';
			attachment_place_holder_text = 'Attachments.';
			var ctx = new SP.ClientContext.get_current();
			var web = ctx.get_web();
			var attachmentFolder=web.getFolderByServerRelativeUrl('Lists/'+list_name+"/Attachments/"+itemId);
			attachmentFiles= attachmentFolder.get_files();
			ctx.load(attachmentFiles);
			ctx.executeQueryAsync(Function.createDelegate(this,this.onSuccess),Function.createDelegate(this,this.onFailed));
			//-------------------------------------------------------
			var queryStringVals = $().SPServices.SPGetQueryString();
			var task_id = queryStringVals["ID"];

			attachmentFolder=web.getFolderByServerRelativeUrl('Lists/Tasks/Attachments/'+task_id);
			task_attachmentFiles= attachmentFolder.get_files();
			ctx.load(task_attachmentFiles);
			ctx.executeQueryAsync(Function.createDelegate(this,this.onSuccessTask),Function.createDelegate(this,this.onFailed));
		}
		
		function onSuccess(sender, args) {                                     
			var i=0;
			var attachments_list = '<strong>Request attachments:</strong><ul>';
			for(var file in attachmentFiles)
			{
				if(attachmentFiles.itemAt(i)!=null){					
					var current_attachment_url = attachmentFiles.itemAt(i).get_serverRelativeUrl();
					attachments_list = attachments_list + "<li><a href='"+current_attachment_url +"'>"+current_attachment_url.substring(current_attachment_url.lastIndexOf("/")+1)+"</a></li>";
				}
				i++;
			}
			if(i>0){
				$('span').each(function(){
					if($(this).text()=="Attachments."){
						$(this).html(attachments_list+"</ul>");
					}
				});	
			}					
		}
		function onSuccessTask(sender, args) {                                     
			var i=0;
			var attachments_list = '<strong>Task attachments:</strong><ul>';
			for(var file in task_attachmentFiles)
			{
				if(task_attachmentFiles.itemAt(i)!=null){					
					var current_attachment_url = task_attachmentFiles.itemAt(i).get_serverRelativeUrl();
					attachments_list = attachments_list + "<li><a href='"+current_attachment_url +"'>"+current_attachment_url.substring(current_attachment_url.lastIndexOf("/")+1)+"</a></li>";
				}
				i++;
			}
			if(i>0){
				$('span').each(function(){
					if($(this).text()=="Task attachments."){
						$(this).html(attachments_list+"</ul>");
					}
				});					
			}
		}
		function onFailed(sender, args) {
			//alert("sorry!");
			$('input[value="Approve"]').each(function(){
				$(this).attr('disabled','disabled');
			});
			$('input[value="Reject"]').each(function(){
				$(this).attr('disabled','disabled');
			});
			$('span').each(function(){
				if($(this).text()=="Task attachments."){
					$(this).html("<strong>Task attachments:</strong><p style='color:red;'>You need to attach your Risk Assignment File before approve/reject this request.</p>");
				}
			});
		}
		
		/*
		Get almost any attributes for a given list item, except for its attachments.
		*/
		function get_list_item_attributes_by_id(list_name,id){
			var attachments_number = 0;
			$().SPServices({
			    operation: "GetListItems",
			    async: false,
			    listName: list_name,
			    CAMLQuery: "<Query><Where><Eq><FieldRef Name='ID'/><Value Type='Integer'>" + id + "</Value></Eq></Where></Query>",
			    completefunc: function (xData, Status) {
			    	$(xData.responseXML).find("[nodeName='z:row']").each(function(i) {
			    	    attachments_number = $(this).attr('ows_Attachments');
			    	});
			    }
			});
			return attachments_number;
		}
		
		/*
		The list_name parameter must be URL encoded.
		*/
		function get_related_content_id(list_name){
			var id = 0;
			$('a').each(function(){
				var url = $(this).attr('href')+"";
				if(url.indexOf(list_name+"/DispForm.aspx")>0){
					id = url.split("=")[1];
					if(id==null||(id+"")=="undefined"){
						id = 0;
					}
				}								
			});
			return id;
		}
		function show_user_picture(){
			/*
			Title
			EMail
			Notes
			Picture
			Department
			JobTitle
			SipAddress
			FirstName
			LastName
			WorkPhone
			Office
			UserName
			WebSite
			SPResponsibility
			*/
			var current_user_picture = $().SPServices.SPGetCurrentUser({
				fieldName: "Picture",
				debug: false
			});
			var current_img = $('img[src="/_layouts/images/siteIcon.png"]');
			current_img.attr('height','40px');
			current_img.attr('src',current_user_picture);
		}
		
		$(document).ready(function(){
			show_user_picture();
			list_name = "Access Requests";
		});
		ExecuteOrDelayUntilScriptLoaded(getWebProperties, "sp.js");

	</script>	
	<!--Jony-Script-End-->

