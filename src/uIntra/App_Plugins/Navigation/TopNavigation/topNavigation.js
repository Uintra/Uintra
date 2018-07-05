require("./topNavigation.css");
import helpers from "./../../Core/Content/scripts/Helpers";

var body = $('body');
var html = $('html');
var className = "_menu-expanded";
var logout = $('.js-logout');

function toggleUserMenu() {
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

logout.on('click', function () {
    helpers.localStorage.removeItem("leftNavigation");
    helpers.localStorage.removeItem("myLinks");
    helpers.localStorage.removeItem("systemLinks");
});

// mobile menu init
function initMobileNav() {
    html.mobileNav({
        menuActiveClass: '_menu-expanded',
        menuOpener: '.js-menu-opener, .js-sidebar-close'
    });
}

/*
 * Simple Mobile Navigation
 */
; (function ($) {
    function MobileNav(options) {
        this.options = $.extend({
            container: null,
            hideOnClickOutside: false,
            menuActiveClass: 'nav-active',
            menuOpener: '.nav-opener',
            menuDrop: '.nav-drop',
            toggleEvent: 'click',
            outsideClickEvent: 'click touchstart pointerdown MSPointerDown'
        }, options);
        this.initStructure();
        this.attachEvents();
    }
    MobileNav.prototype = {
        initStructure: function () {
            this.page = $('html');
            this.container = $(this.options.container);
            this.opener = this.container.find(this.options.menuOpener);
            this.drop = this.container.find(this.options.menuDrop);
        },
        attachEvents: function () {
            var self = this;

            if (activateResizeHandler) {
                activateResizeHandler();
                activateResizeHandler = null;
            }

            this.outsideClickHandler = function (e) {
                if (self.isOpened()) {
                    var target = $(e.target);
                    if (!target.closest(self.opener).length && !target.closest(self.drop).length) {
                        self.hide();
                    }
                }
            };

            this.openerClickHandler = function (e) {
                e.preventDefault();
                self.toggle();
            };

            this.opener.on(this.options.toggleEvent, this.openerClickHandler);
        },
        isOpened: function () {
            return this.container.hasClass(this.options.menuActiveClass);
        },
        show: function () {
            this.container.addClass(this.options.menuActiveClass);
            if (this.options.hideOnClickOutside) {
                this.page.on(this.options.outsideClickEvent, this.outsideClickHandler);
            }
        },
        hide: function () {
            this.container.removeClass(this.options.menuActiveClass);
            if (this.options.hideOnClickOutside) {
                this.page.off(this.options.outsideClickEvent, this.outsideClickHandler);
            }
        },
        toggle: function () {
            if (this.isOpened()) {
                this.hide();
            } else {
                this.show();
            }
        },
        destroy: function () {
            this.container.removeClass(this.options.menuActiveClass);
            this.opener.off(this.options.toggleEvent, this.clickHandler);
            this.page.off(this.options.outsideClickEvent, this.outsideClickHandler);
        }
    };

    var activateResizeHandler = function () {
        var win = $(window),
            doc = $('html'),
            resizeClass = 'resize-active',
            flag, timer;
        var removeClassHandler = function () {
            flag = false;
            doc.removeClass(resizeClass);
        };
        var resizeHandler = function () {
            if (!flag) {
                flag = true;
                doc.addClass(resizeClass);
            }
            clearTimeout(timer);
            timer = setTimeout(removeClassHandler, 500);
        };
        win.on('resize orientationchange', resizeHandler);
    };

    $.fn.mobileNav = function (options) {
        return this.each(function () {
            var params = $.extend({}, options, { container: this }),
                instance = new MobileNav(params);
            $.data(this, 'MobileNav', instance);
        });
    };
}(jQuery));

var controller = {
    init: function () {
        toggleUserMenu();
        initMobileNav();
    }
}

export default controller;