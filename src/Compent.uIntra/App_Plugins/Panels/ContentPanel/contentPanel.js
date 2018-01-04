import ajax from "./../../Core/Content/scripts/Ajax";

var Photoswipe = require('photoswipe');
var photoswipeUiDefault = require('photoswipe/dist/photoswipe-ui-default');

require("./contentPanel.css");

var itemTypes = {
    Image: "Image",
    Video: "Video"
}

var youtubeImageSize = {
    Hq: "hq",
    Mq: "mq",
    Sd: "sd",
    Maxres: "maxres"
}

var vimeoVideoInfoLink = "http://vimeo.com/api/v2/video/";
var youtubeImageLink = "https://img.youtube.com/vi/";
var youtubeDefaultImage = "default.jpg";
var selectors = window.contentPanelSelectors || [];
var body = document.querySelector('body');
var mobileMediaQuery = window.matchMedia("(max-width: 899px)");

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

var setVideoThumnailUrl = function (videoId, videoType, btnStyle) {
    switch (videoType) {
        case "Youtube":
            btnStyle
                .backgroundImage = `url('${youtubeImageLink}${videoId}/${youtubeImageSize.Mq}${youtubeDefaultImage}')`;
            return;
        case "Vimeo":
            ajax.Get(`${vimeoVideoInfoLink}${videoId}.json`,
                function (response) {
                    btnStyle.backgroundImage = `url('${response[0].thumbnail_medium}')`;
                });
            return;
    }
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
        } else {
            setVideoThumnailUrl(elementToshow.dataset["videoid"],
                elementToshow.dataset["videotype"],
                videoPosterBtn.style);
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
    var opener = document.querySelector("#js-sidepanel-opener");
    var container = document.querySelector('.sidebar');
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

    if (!opener || !container) {
        body.classList.add('_hide-sidepanel-opener');
        return
    };

    var sideBlock = container.querySelectorAll('.block');
    //container.style.top = height + 'px';

    if (sideBlock.length > 0) {
        opener.addEventListener('click',
            () => {
                body.classList.toggle('_sidebar-expanded');
                if (body.classList.contains('_search-expanded')) {
                    body.classList.remove('_search-expanded');
                }
                if (body.classList.contains('_menu-expanded')) {
                    body.classList.remove('_menu-expanded');
                }

                body.addEventListener('click',
                    function (ev) {
                        isOutsideClick(container, opener, ev.target, '_sidebar-expanded');
                    });
            });
    }
    else {
        body.classList.add('_hide-sidepanel-opener');
    }
}

var isOutsideClick = function (el, opener, target, className) {
    if (!el.contains(target) && (opener && !opener.contains(target)) && body.classList.contains(className)) {
        body.classList.remove(className);
    }
}

var controller = {
    init: function () {
        selectors.forEach(function (selector) {
            initPanel(selector);
        });

        if (mobileMediaQuery.matches) {
            initMobileBanners();
        }
    }
}

export default controller;