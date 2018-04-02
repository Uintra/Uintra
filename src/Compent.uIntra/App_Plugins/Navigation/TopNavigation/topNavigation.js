require("./topNavigation.css");
import helpers from "./../../Core/Content/scripts/Helpers";

var mobileMediaQuery = window.matchMedia("(max-width: 899px)");
var body = $('body');
var html = $('html');
var className = "_menu-expanded";
var logout = $('.js-logout');

function initMobileNav() {
    var opener = document.querySelector(".js-menu-opener");
    var closeBtn = document.querySelector(".js-sidebar-close");
    var container = document.querySelector('#sidebar');

    if(opener){
        opener.addEventListener('click', () => {
            toggleMobileMenu(opener, container);
        });
        closeBtn.addEventListener('click', () => {
            toggleMobileMenu(closeBtn, container);
        });
    }
};

function toggleMobileMenu(element, container){
    html.toggleClass(className).removeClass('_search-expanded _notifications-expanded _sidebar-expanded');

    /*html.on("click.nav", function(ev) {
        isOutsideClick(container, element, ev.target, '_menu-expanded', function() {
            html.removeClass(className).off("click.nav");
        });
    });*/
}

function toggleUserMenu(){
    var userOpener = document.querySelector('.user__opener');
    var userContainer = document.querySelector('.user__menu');
    var userClass = '_usermenu-expanded';

    userOpener.addEventListener('click', () => {
        html.toggleClass(userClass).removeClass('_search-expanded _notifications-expanded _sidebar-expanded');
    });

    body.on("click", function(ev) {
        isOutsideClick(userContainer, userOpener, ev.target, userClass, function() {
            html.removeClass(userClass);
        });
    });
}

var isOutsideClick = function (el, opener, target, className, callback) {
    if (el && !el.contains(target) && (opener && !opener.contains(target)) && html.hasClass(className)) {
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

function getClientHeight() { return document.compatMode == 'CSS1Compat' ? document.documentElement.clientHeight : document.body.clientHeight; }

function mobileSidebarHeight() {
    var mobileSidebar = document.querySelector(".sidebar__holder");
    var searchHeight = document.querySelector("#sidebar .search").offsetHeight;

    if (mobileSidebar) {
        mobileSidebar.style.height = (getClientHeight() - searchHeight) + 'px';
    }
}

function windowResize() {
    window.addEventListener('resize', () => {
        mobileSidebarHeight();
    });
}

var controller = {
    init: function () {
        toggleUserMenu();
        if (mobileMediaQuery.matches) {
            initMobileNav();
            mobileSidebarHeight();
            windowResize();
        }
    }
}

export default controller;