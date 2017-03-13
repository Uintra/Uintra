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
        var arrows = Array.from($("img[id *= 'side-nav-arrow_']"));
        arrows.forEach(function (arrow) {
            arrow.addEventListener('click',
                 function () {
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
        });
        showActive();
    });

    function hideChildrenNav(id) {
        var nav = $("#side-nav-children_" + id);
        $(nav).hide();
    }

    function showChildrenNav(id) {
        var nav = $("#side-nav-children_" + id);
        $(nav).show();
    }

    function showActive() {
        var active = $("._active.side-nav__item");
        if (active.length > 0) {
            var parentDiv = $(active).closest("div[id *= 'side-nav-children_']");
            parentDiv.show();
            var id = $(parentDiv).data("id");
            var arrow = $("#side-nav-arrow_" + id);
            arrow[0].src = "/Content/images/arrow_up.svg";
            $(arrow).data("open", true);
        }
    }

    document.body.addEventListener('cfTabChanged', locationChagned);
})();