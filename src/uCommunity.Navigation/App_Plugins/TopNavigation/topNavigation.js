import appInitializer from "./../../Core/Content/scripts/AppInitializer";

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

/*var initToTop = function () {
    var trigger = document.getElementById('toTop');

    window.addEventListener('scroll', function (e) {
        if (window.scrollY > 100 && !trigger.classList.contains('_visible')) {
            trigger.classList.add('_visible');
        }
        else if (window.scrollY <= 100 && trigger.classList.contains('_visible')) {
            trigger.classList.remove('_visible');
        }
    });

    trigger.addEventListener('click', function () {
        $('html, body').stop().animate({
            scrollTop: 0
        }, 500);
    });
}*/

var isOutsideClick = function (el, opener, target, className, callback) {
    if (el && !el.contains(target) && (opener && !opener.contains(target)) && body.hasClass(className)) {
        if (typeof callback === "function") {
            callback();
        }
    }
};

var controller = {
    init: function () {
        //initToTop();

        if (mobileMediaQuery.matches) {
            initMobileNav();
        }
    }
}

appInitializer.add(controller.init);