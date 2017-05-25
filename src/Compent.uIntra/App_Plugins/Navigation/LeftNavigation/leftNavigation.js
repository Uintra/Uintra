import appInitializer from "./../../Core/Content/scripts/AppInitializer";

require("./leftNavigation.css");

var active = "_expand";

function locationChagned() {
    var path = window.location.pathname;
    var links = document.querySelectorAll('.js-side-nav__link');

    for (var i = 0; i < links.length; i++) {
        var link = links[i];
        if (link.pathname == path) {
            link.parentElement.classList.add(active);
        } else {
            link.parentElement.classList.remove(active);
        }
    }
}

appInitializer.add(function() {
    $(".js-side-nav__opener").on("click", function() {
        $(this).closest(".js-side-nav__item").toggleClass(active);
    });

    document.body.addEventListener('cfTabChanged', locationChagned);
});