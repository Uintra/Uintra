require('./leftNavigation.css');
import helpers from "./../../Core/Content/scripts/Helpers";

var classExpanded = '_expand';
var navState = helpers.localStorage.getItem("leftNavigation") || {};
var opener = $('.js-side-nav__opener');

function getNavState(){
    var navItems = $('.js-side-nav__item');
    
    if(!jQuery.isEmptyObject(navState)){
        for(var i = 0; i < navItems.length; i++){
            var id = $(navItems[i]).data("id");
            if(id){
                for(var item in navState){
                    if(id == item){
                        $(navItems[i]).toggleClass(classExpanded, navState[item]);
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

    item.toggleClass(classExpanded);
    isExpanded = item.hasClass(classExpanded);

    navState[itemId] = isExpanded;

    helpers.localStorage.setItem("leftNavigation", navState);
}

var controller = {
    init: function () {
        getNavState();

        if(!document.querySelector('.ss-container')){
            helpers.initScrollbar(document.querySelector('.js-sidebar'));
        }

        if(opener.length > 0){
            opener.on('click', function(e){
                toggleLinks(this);
            });
        }
    }
}

export default controller;