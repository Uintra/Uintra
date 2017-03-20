'use strict';
var topMenu = (function () {
    var body = document.querySelector('body');

    function initMobile() {
        initMobileNav();
    };

    function initMobileNav() {
        var opener = document.querySelector("#js-menu-opener");
        var container = document.querySelector('#sidebar');

        opener.addEventListener('click',
            () => {
                body.classList.toggle('_menu-expanded');
                if (body.classList.contains('_search-expanded')) {
                    body.classList.remove('_search-expanded');
                }
                if (body.classList.contains('_notifications-expanded')) {
                    body.classList.remove('_notifications-expanded');
                }
                if (body.classList.contains('_sidebar-expanded')) {
                    body.classList.remove('_sidebar-expanded');
                }

                body.addEventListener('click',
                    function (ev) {
                        isOutsideClick(container, opener, ev.target, '_menu-expanded');
                    });
            });
    };

    var initToTop = function () {
        var trigger = document.getElementById('toTop');
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

    var isOutsideClick = function (el, opener, target, className) {
        if (!el.contains(target) && (opener && !opener.contains(target)) && body.classList.contains(className)) {
            body.classList.remove(className);
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

    App.AppInitializer.add(controller.init);
    return controller;
})();

