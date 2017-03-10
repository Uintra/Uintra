'use strict';
var subNavigationMenu = (function () {

    var menu = $('.tabset__navigation');
    var title = $('.tabset__title');
    var menuHolder = menu.closest('.tabset');
    var body = $('body');

    if (menu.length > 0) {
        body.addClass('_with-sub-nav');
    }

    if (title.length > 0) {
        body.addClass('_with-subtitle');
    }

    var initSubMenuPosition = function () {
        var submenu = $('div.tabset .tabset__navigation');
        var holder = title.closest('.tabset__holder');
        if (title.length > 0 && submenu !== null) {
            submenu.closest('.tabset').remove();
            submenu.appendTo(holder);
        }
    }
    
    var initMobileMenu = function() {
        var opener = menu.find('.tabset__navigation-link');

        if (menu.find('._active').length <= 0) {
            menu.find('.tabset__navigation-item:first-child').addClass('_active');
        }

        if (opener.length > 1) {
            opener.each(function () {
                var $this = $(this);
                $this.click(function (e) {
                    if ($this.closest('._active').length > 0) {
                        e.preventDefault();
                        menuHolder.toggleClass('_expanded');
                        if (body.hasClass('_search-expanded')) {
                            body.removeClass('_search-expanded');
                        }
                        if (body.hasClass('_notifications-expanded')) {
                            body.removeClass('_notifications-expanded');
                        }
                        if (body.hasClass('_menu-expanded')) {
                            body.removeClass('_menu-expanded');
                        }
                        if (body.hasClass('_sidebar-expanded')) {
                            body.removeClass('_sidebar-expanded');
                        }
                    }
                });
            });
        }

        else {
            opener.closest('.tabset__navigation-item').addClass('_disabled');
        }
    }

    var controller = {
        init: function () {
            var md = new MobileDetect(window.navigator.userAgent);

            if (md.mobile()) {
                initMobileMenu();
            }

            initSubMenuPosition();
        }
    }

    App.AppInitializer.add(controller.init);
    return controller;
})();

