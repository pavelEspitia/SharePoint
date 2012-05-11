
    $(document).ready(function () {
        function generatePages() {
            var _total, i, _link;

            _total = $("#carousel").rcarousel("getTotalPages");

            for (i = 0; i < _total; i++) {
                _link = $("<a href='#'></a>");
                $(_link).bind("click", { page: i },
                        function (event) {
                            $("#carousel").rcarousel("goToPage", event.data.page);
                            event.preventDefault();
                        }
                    ).addClass("bullet off").appendTo("#pages");
        }

            // mark first page as active
            $("a:eq(0)", "#pages")
						.removeClass("off")
						.addClass("on")
						.css("background-image", "url(/DocLib2/rcarousel/page-on.png)");

        }

        function pageLoaded(event, data) {
            $("a.on", "#pages")
						.removeClass("on")
						.css("background-image", "url(/DocLib2/rcarousel/page-off.png)");

            $("a", "#pages")
						.eq(data.page)
						.addClass("on")
						.css("background-image", "url(/DocLib2/rcarousel/page-on.png)");
        }

        $("#carousel").rcarousel({
            visible: 1,
            step: 1,
            height: 300,
            width: 500,
            auto: { enabled: true },
            start: generatePages,
            pageLoaded: pageLoaded
        });

        $(".bullet")
					.hover(
						function () {
						    $(this).css("opacity", 0.7);
						},
						function () {
						    $(this).css("opacity", 1.0);
						}
					);
        $("#ui-carousel-next")
            .add("#ui-carousel-prev")
            .hover(
                function () {
                    $(this).css("opacity", 0.7);
                },
                function () {
                    $(this).css("opacity", 1.0);
                }
            );
    });