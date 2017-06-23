require("./topNavigation.css");

var mobileMediaQuery = window.matchMedia("(max-width: 899px)");
var body = $('body');
var className = "_menu-expanded";

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

function resetStorage(){
    var trigger = $('.js-logout');
    trigger.on('click', function(){
        if(localStorage.getItem('sidebar')) {
            localStorage.removeItem('sidebar');
        }
        if(localStorage.getItem('myLinks')){
            localStorage.removeItem('myLinks');
        }
    });
}

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
        resetStorage();
    }
}

export default controller;