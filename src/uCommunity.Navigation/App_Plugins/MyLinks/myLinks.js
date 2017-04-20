import appInitializer from "./../../Core/Content/scripts/AppInitializer";

require("./_myLinks.css");

var mobileMediaQuery = window.matchMedia("(max-width: 899px)");
var body = $('body');

function initMobileNav() {
    var opener = document.querySelector("#js-menu-opener");
    var container = document.querySelector('#sidebar');

    opener.addEventListener('click', () => {
        body.toggleClass('_menu-expanded').removeClass('_search-expanded _notifications-expanded _sidebar-expanded');

        body.on("click.nav", function(ev) {
            isOutsideClick(container, opener, ev.target, '_menu-expanded', function() {
                body.removeClass(className).off("click.nav");
            });
        });
    });
};

var isOutsideClick = function (el, opener, target, className, callback) {
    if (el && !el.contains(target) && (opener && !opener.contains(target)) && body.hasClass(className)) {
        if (typeof callback === "function") {
            callback();
        }
    }
};

var controller = {
    init: function () {
        if (mobileMediaQuery.matches) {
            initMobileNav();
        }
    }
}

appInitializer.add(controller.init);