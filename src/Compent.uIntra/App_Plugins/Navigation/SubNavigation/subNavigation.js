require("./subNavigation.css");

var mobileMediaQuery = window.matchMedia("(max-width: 899px)");
var tabset = $('.tabset');
var menu = $('.tabset__navigation');
var title = $('.tabset__title');
var menuHolder = menu.closest('.tabset');
var body = $('body');
var wrapper = $('#wrapper');
var header = $('#header');

if (menu.length > 0) {
    body.addClass('_with-sub-nav');
}

if (title.length > 0) {
    body.addClass('_with-subtitle');
}

var initSubMenuPosition = function () {
    if(!tabset || !header){
        return;
    }

    var height = header.outerHeight() + tabset.outerHeight();

    if(menu.length > 1){
        wrapper.css({
            'padding-top': height + 46 + 'px'
        });
    }

    else{
        wrapper.css({
            'padding-top': height + 'px'
        });
    }
}
    
var initMobileMenu = function() {
    var opener = menu.find('.tabset__navigation-link');
    var itemHeigth = 0;

    if (menu.find('._selected').length <= 0) {
        menu.find('.tabset__navigation-item:first-child').addClass('_selected');
    }

    menu.each(function () {
        if ($(this).closest('.tabset__inner').length == 0) {
            $(this).css({
                'padding-top': '50px'
            });
        }
    });

    if (body.hasClass('_with-sub-nav')) {
        var height = tabset.outerHeight() + header.outerHeight();
        wrapper.css({
            'padding-top': height + 'px'
        })
    }

    if (opener.length > 1) {
        opener.on("click", function (e) {
            if ($(this).closest('li').hasClass('_selected')) {
                e.preventDefault();
                menuHolder.toggleClass('_expanded');
                body.removeClass('_search-expanded notifications-expanded _menu-expanded _sidebar-expanded');
            }
        });
    }
    else {
        opener.closest('.tabset__navigation-item').addClass('_disabled');
    }
}

var controller = {
    init: function () {
        initSubMenuPosition();
        $(window).resize(function(){
            initSubMenuPosition();
        });
        if (mobileMediaQuery.matches) {
            initMobileMenu();
        }
    }
}

export default controller;