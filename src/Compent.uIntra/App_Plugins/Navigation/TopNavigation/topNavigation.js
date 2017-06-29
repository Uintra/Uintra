require("./topNavigation.css");
import helpers from "./../../Core/Content/scripts/Helpers";

var mobileMediaQuery = window.matchMedia("(max-width: 899px)");
var body = $('body');
var className = "_menu-expanded";
var logout = $('.js-logout');

function initMobileNav() {
    var opener = document.querySelector(".js-menu-opener");
    var container = document.querySelector('#sidebar');
    var overlay = document.querySelector(".js-side-nav__overlay");

    opener.addEventListener('click', () => {
        toggleMobileMenu(opener, container);
    });
};

function toggleMobileMenu(element, container){
    body.toggleClass(className).removeClass('_search-expanded _notifications-expanded _sidebar-expanded');

    body.on("click.nav", function(ev) {
        isOutsideClick(container, element, ev.target, '_menu-expanded', function() {
            body.removeClass(className).off("click.nav");
        });
    });
}

var isOutsideClick = function (el, opener, target, className, callback) {
    if (el && !el.contains(target) && (opener && !opener.contains(target)) && body.hasClass(className)) {
        if (typeof callback === "function") {
            callback();
        }
    }
};

logout.on('click', function(){
    helpers.localStorage.removeItem("leftNavigation");
    helpers.localStorage.removeItem("myLinks");
    helpers.localStorage.removeItem("systemLinks");
});

var controller = {
    init: function () {
        if (mobileMediaQuery.matches) {
            initMobileNav();
        }
    }
}

export default controller;