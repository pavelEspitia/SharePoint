$(document).ready(function () {
	//zTree
	var setting = {
		view: {
			selectedMulti: false
		},
		callback: {
			onExpand: reset_leaf_style
		}
	};
	var zTreeObj = $.fn.zTree.init($("#ul_folder_tree"), setting, zTreeNodes);

	var folder = getQueryString()['RootFolder'];
	if (folder == null || folder == "undefined") { folder = ''; }
	var current_node = zTreeObj.getNodeByParam('url', unescape(location.pathname + '?RootFolder='+folder), null);
	zTreeObj.selectNode(current_node, false);
	reset_leaf_style();
});

function reset_leaf_style() {
	$('.ico_docu').addClass('ico_close');
	$('.ico_docu').removeClass('ico_docu');
}

function getQueryString() {
	var result = {}, queryString = location.search.substring(1),
      re = /([^&=]+)=([^&]*)/g, m;

	while (m = re.exec(queryString)) {
		result[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
	}

	return result;
}
