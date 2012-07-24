var slider_item_number = 0;
var current_slider_item = 0;
var timer = null;
var interval = 3000;

$(document).ready(function () {
	var slider_id = 0;
	$('li[class*="dfwp-item"]').each(function () {
		$(this).attr('id', slider_id);
		slider_id++;
	});
	slider_item_number = slider_id / 2;

	//set pages style
	for (var i = 0; i < slider_item_number; i++) {
		$('li[id="' + i + '"]').each(function () {
			if (i > 0) {
				$(this).hide();
			}
			//$(this).children('div').children('div').children('a').children('img').attr('height','150');	
			//$(this).children('div').children('div').children('a').children('img').attr('width','250');	
			//$(this).children('div').children('div').children('a').addClass('title_text');	
		});
	}

	//set thumbs' style
	for (var i = slider_item_number; i < slider_id; i++) {
		$('li[id="' + i + '"]').each(function () {
			//$(this).prepend('<img src="https://china.works.com/PublishingImages/up_arrow.png" />');
			//$(this).addClass('thumb_item');					
			//$(this).attr('width','33%');
			var div = $(this).children('div');
			div.addClass('unselected_item');
			div.attr('parent_id', i);
			div.mouseover(function () {
				current_slider_item = $(this).attr('parent_id') - slider_item_number;
				roll_pages();
				window.clearInterval(timer);
			});
			div.mouseout(function () {
				timer = window.setInterval(roll_pages, interval);
			});
			//div.children('div').addClass('selected_item_padding');
			//var img=div.children('div').children('a').children('img');
			//img.attr('height','80');
			//img.attr('width','120');					
		});
	}
	roll_pages();
	timer = window.setInterval(roll_pages, interval);
});


function roll_pages() {
	//alert("current item:"+current_slider_item);
	if (current_slider_item >= slider_item_number) {
		current_slider_item = 0;
	}
	for (var i = 0; i < (slider_item_number * 2); i++) {
		$('li[id="' + i + '"]').each(function () {
			var div = $(this).children('div');
			//alert(div+"\n"+i);
			//for current page
			if (i == current_slider_item) {
				$(this).show();
			} else {
				if (i < slider_item_number) {
					$(this).hide();
				}
			}

			//for thumbs
			if (i == (current_slider_item + slider_item_number)) {
				div.addClass('selected_item');
				div.removeClass('unselected_item');
				div.children('div').addClass('selected_item_background');
			} else {
				if (i >= slider_item_number) {
					div.removeClass('selected_item');
					div.addClass('unselected_item');
					div.children('div').removeClass('selected_item_background');
				}
			}
		});
	}
	current_slider_item++;
}
