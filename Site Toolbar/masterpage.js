	function showDialog(pageUrl){
		var options = {
			url: pageUrl,
			width: 700,
			title: '对话框',
			allowMaximize: true,
			showClose: true
		}
		SP.UI.ModalDialog.showModalDialog(options);
	}

	function openAboutDialog(){
		showDialog("/Pages/About.aspx");
	}
	
	function openHelpDialog(){
		showDialog("/Pages/Help.aspx");
	}