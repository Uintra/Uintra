import helpers from "./../../Core/Content/scripts/Helpers";

require('./leftNavigation.css');
require('./leftNavigation.css');

var container = $('.js-sidebar-inner');
var active = '_expand';
var mobileMediaQuery = window.matchMedia("(max-width: 899px)");

if(localStorage.getItem('sidebar')) {
    container.html(localStorage.getItem('sidebar'));
}

var opener = $('.js-side-nav__opener');

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

function toggleLinks(el, event, key){
    if(localStorage.getItem(key)) {
        localStorage.removeItem(sidebar);
    }
    $(el).closest('.js-side-nav__item').toggleClass(active);
    container = $('.js-sidebar-inner');
    var content = container.html();
    localStorage.setItem(key, content);
}

appInitializer.add(function() {
    opener.on('click', function(e){
        toggleLinks(this, e, 'sidebar');
    });
    if(!document.querySelector('.ss-container') && !mobileMediaQuery.matches){
        helpers.initScrollbar(document.querySelector('.js-sidebar'));
    }
    document.body.addEventListener('cfTabChanged', locationChagned);
});