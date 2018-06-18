var Photoswipe = require('photoswipe');
var photoswipeUiDefault = require('photoswipe/dist/photoswipe-ui-default');

require("./contentPanel.css");

var itemTypes = {
    Image: "Image",
    Video: "Video"
}

var selectors = window.contentPanelSelectors || [];
var body = document.querySelector('body');
var html = document.querySelector('html');

var videoPlay = function (videoElement, isLightBox) {
    if (!isLightBox) {
        videoElement.parentElement.parentElement.classList.add("_active");
    }
    videoElement.setAttribute("src", videoElement.dataset["src"]);
}

var videoStopPlay = function (videoElement, isLightBox) {
    if (!isLightBox) {
        videoElement.parentElement.parentElement.classList.remove("_active");
    }
    videoElement.setAttribute("src", "");
}

var openPhotoSwipe = function (itemToshow, itemType) {
    var photoSwiper = document.querySelector(".pswp");
    var items = [];

    switch (itemType) {
        case itemTypes.Image:
            var imageItem = {
                src: itemToshow,
                w: 800,
                h: 800
            }
            items.push(imageItem);
            break;
        case itemTypes.Video:
            var videoItem = {
                html: itemToshow.outerHTML
            }
            items.push(videoItem);
            break;
        default:
    }

    var options = {
        history: false,
        focus: false,
        shareEl: false,
        fullscreenEl: false,
        bgOpacity: 0.8,
        showHideOpacity: false
    };

    var gallery = new Photoswipe(photoSwiper, photoswipeUiDefault, items, options);
    gallery.init();

    if (itemType == itemTypes.Video) {
        gallery.listen('close',
            () => {
                var photoswipeVideoItem = photoSwiper.querySelector(".js-photoswipe-item");
                videoStopPlay(photoswipeVideoItem, true);
            });
    }
    gallery.listen('close',
        () => {
            photoSwiper.parentElement.classList.remove('js-lightbox-open');
        });
}

var initPanel = function (selector) {
    var panel = document.querySelector(selector);
    var videoPosterBtn = panel.querySelector(".js-videoPoster");
    var showLightboxBtn = panel.querySelector(".js-show-lightbox");
    var elementToshow = panel.querySelector(".js-photoswipe-item");
    var bodyElement = document.querySelector('body');

    if (videoPosterBtn) {
        if (videoPosterBtn.dataset["backgroundimage"]) {
            videoPosterBtn.style.backgroundImage = `url('${videoPosterBtn.dataset["backgroundimage"]}')`;
        }

        if (showLightboxBtn) {
            showLightboxBtn.addEventListener("click",
                () => {
                    var videoElement = elementToshow.cloneNode(true);
                    videoPlay(videoElement, true);
                    openPhotoSwipe(videoElement, itemTypes.Video);
                    bodyElement.classList.add('js-lightbox-open');
                });
            return;
        } else {
            videoPosterBtn.addEventListener("click",
                () => {
                    videoPlay(elementToshow);
                });
            return;
        }

    } else if (showLightboxBtn) {
        showLightboxBtn.addEventListener("click",
            () => {
                var imageSrc = (elementToshow).src;
                openPhotoSwipe(imageSrc, itemTypes.Image);
                bodyElement.classList.add('js-lightbox-open');
            });
    }
}

var initMobileBanners = function () {
    var opener = document.querySelector("#js-aside-opener");
    var aside = document.querySelector('.aside');
    var tabset = document.querySelector('.tabset');
    var header = document.getElementById('header');
    var bulletinBtn = document.querySelector('.bulletin__btn-holder');
    var height = header.clientHeight;

    if (tabset) {
        height += tabset.clientHeight;
    }

    if (bulletinBtn) {
        height += bulletinBtn.clientHeight;
    }
        
    if (aside) {
        body.classList.add('_show-aside-opener');
        opener.addEventListener('click',
            (e) => {
                e.preventDefault();
                html.classList.toggle('_aside-expanded');
                if (html.classList.contains('_search-expanded')) {
                    html.classList.remove('_search-expanded');
                }
                if (html.classList.contains('_menu-expanded')) {
                    html.classList.remove('_menu-expanded');
                }

                html.addEventListener('click',
                    function (ev) {
                        isOutsideClick(aside, opener, ev.target, '_aside-expanded');
                    });
            });
    }
}

var isOutsideClick = function (el, opener, target, className) {
    if (!el.contains(target) && (opener && !opener.contains(target)) && html.classList.contains(className)) {
        html.classList.remove(className);
    }
}

function getClientHeight() { return document.compatMode == 'CSS1Compat' ? document.documentElement.clientHeight : document.body.clientHeight; }

/*function mobileAsideHeight() {
    var mobileAside = document.querySelector(".aside > div");
    var headerHeight = document.getElementById('header').offsetHeight;

    if (mobileAside) {
        mobileAside.style.height = (getClientHeight() - headerHeight) + 'px';
    }
}

function windowResize() {
    window.addEventListener('resize', () => {
        mobileAsideHeight();
    });
}*/

// media query change
function WidthChange(mq) {
    if (!mq.matches) {
        initMobileBanners();
        //mobileAsideHeight();
        //windowResize();
    }
}

var controller = {
    init: function () {
        selectors.forEach(function (selector) {
            initPanel(selector);
        });

        if (matchMedia) {
            var mq = window.matchMedia("(min-width: 900px)");
            mq.addListener(WidthChange);
            WidthChange(mq);
        }
    }
}

export default controller;