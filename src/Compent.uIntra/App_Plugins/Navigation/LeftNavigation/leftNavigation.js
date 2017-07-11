require('./leftNavigation.css');
import helpers from "./../../Core/Content/scripts/Helpers";

//var container = $('.js-sidebar-inner');
var active = '_expand';
var mobileMediaQuery = window.matchMedia("(max-width: 899px)");
var navState = helpers.localStorage.getItem("leftNavigation") || {};
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

function getNavState(){
    var navItems = $('.js-side-nav__item');
    
    if(!jQuery.isEmptyObject(navState)){
        for(var i = 0; i < navItems.length; i++){
            var id = $(navItems[i]).data("id");
            if(id){
                for(var item in navState){
                    if(id == item){
                        $(navItems[i]).toggleClass('_expand', navState[item]);
                    }
                }
            }
        }
    }
}

function toggleLinks(el){
    var item = $(el).closest('.js-side-nav__item');
    var itemId = item.data("id");
    var isExpanded;

    item.toggleClass(active);
    isExpanded = item.hasClass('_expand');

    navState[itemId] = isExpanded;

    helpers.localStorage.setItem("leftNavigation", navState);
}

var controller = {
    init: function () {
        getNavState();
        opener.on('click', function(e){
            toggleLinks(this);
        });
        if(!document.querySelector('.ss-container') && !mobileMediaQuery.matches){
            helpers.initScrollbar(document.querySelector('.js-sidebar'));
        }

        uIntra.events.cfTabChanged.addListener(locationChagned);
    }
}

export default controller;