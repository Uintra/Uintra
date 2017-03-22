import appInitializer from "./../../Core/Content/scripts/AppInitializer";

require("./_usermenu.css");
require("./_topNavigation.css");

var MobileDetect = require('mobile-detect');

var body = document.querySelector('body');

function initMobile() {
    initMobileNav();
};

function initMobileNav() {
    var opener = document.querySelector("#js-menu-opener");
    var container = document.querySelector('#sidebar');

    opener.addEventListener('click', () => {
        body.classList.toggle('_menu-expanded');
        body.classList.remove('_search-expanded');
        body.classList.remove('_notifications-expanded');
        body.classList.remove('_sidebar-expanded');

        $(body).on("click.nav", function(ev) {
            isOutsideClick(container, opener, ev.target, '_menu-expanded', function() {
                body.classList.remove(className);
                $(body).off("click.nav");
            });
        });
    });
};

var initToTop = function () {
    var trigger = document.getElementById('toTop');
    if(trigger){
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
    }
}

var isOutsideClick = function (el, opener, target, className, callback) {
    if (!el.contains(target) && (opener && !opener.contains(target)) && body.classList.contains(className)) {
        if (typeof callback === "function") {
            callback();
        }
    }
};

var controller = {
    init: function () {
        initToTop();

        var md = new MobileDetect(window.navigator.userAgent);

        if (md.mobile()) {
            initMobile();
        }
    }
}

appInitializer.add(controller.init);