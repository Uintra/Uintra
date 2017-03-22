require("./_leftNavigation.css");

(function () {
    var active = "_active";
    function locationChagned() {
        var path = window.location.pathname;
        var links = document.querySelectorAll('.side-nav .side-nav__link');

        for (var i = 0; i < links.length; i++) {
            var link = links[i];
            if (link.pathname == path) {
                link.parentElement.classList.add(active);
            } else {
                link.parentElement.classList.remove(active);
            }
        }
    }

    $(document).ready(function () {
        /*$("img[id *= 'side-nav-arrow_']").on("click", function () {
            var arrow = this;
            var open = $(arrow).data("open");
            var id = $(arrow).data("id");

            if (!open) {
                arrow.src = "/Content/images/arrow_up.svg";
                $(arrow).data("open", true);
                showChildrenNav(id);
            } else {
                arrow.src = "/Content/images/arrow_down.svg";
                $(arrow).data("open", false);
                hideChildrenNav(id);
            }
        });

        showActive();*/
    });


    function hideChildrenNav(id) {
        //$("#side-nav-children_" + id).hide();
    }

    function showChildrenNav(id) {
        //$("#side-nav-children_" + id).show();
    }

    function showActive() {
        /*var active = $(".side-nav__item._active");
        
        if (active.length > 0) {
            var parentDiv = $(active).closest("div[id *= 'side-nav-children_']");

            parentDiv.show();

            var id = $(parentDiv).data("id");
            var arrow = $("#side-nav-arrow_" + id);

            arrow[0].src = "/Content/images/arrow_up.svg";
            $(arrow).data("open", true);
        }*/
    }

    document.body.addEventListener('cfTabChanged', locationChagned);
})();