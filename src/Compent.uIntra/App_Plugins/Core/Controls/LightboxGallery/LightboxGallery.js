require('./_lightbox.css');
require('./_lightboxGallery.css');

var Photoswipe = require('photoswipe');
var photoswipeUiDefault = require('photoswipe/dist/photoswipe-ui-default');

import helpers from "./../../Content/scripts/Helpers";

var photoswipeElement = document.querySelector('.pswp');
var galleries = [];
var defaultOptions = {
    escKey: true,
    arrowKeys: true,
    bgOpacity: 0.8,
    history: false,
    showHideOpacity: false
};

var createGallery = function (gallery) {
    var bodyElement = document.querySelector('body');

    gallery.instance = new Photoswipe(photoswipeElement,
        photoswipeUiDefault,
        gallery.items,
        helpers.deepClone(gallery.options));

    gallery.instance.init();
    bodyElement.classList.add('js-lightbox-open');

    const stopVideo = function () {
        Array.from(gallery.instance.container.querySelectorAll('.pswp__video')).forEach(function (item) {
            item.setAttribute('src', item.getAttribute('src'));
        });
    }
    
    gallery.instance.listen('beforeChange', stopVideo);

    gallery.instance.listen('close', function () {
        stopVideo();
        gallery.instance = null;
        bodyElement.classList.remove('js-lightbox-open');
    });
}

var buildPhotoswipeItems = function (imagesItems) {
    var result = [];

    for (var i = 0; i < imagesItems.length; i++) {
        if (!imagesItems[i].dataset.fullUrl || !imagesItems[i].dataset.fullSize) {
            continue;   
        }
        var item = imagesItems[i];
        var paprentItem = item.parentNode;
        var size = item.getAttribute('data-full-size').split('x');
        var width;
        var height;
        var newItem;

        if (size && size.length == 2) {
            width = parseInt(size[0]);
            height = parseInt(size[1]);
        } else {
            width = window.screen.availWidth;
            height = window.screen.availHeight;
        }

        // create slide object
        if (paprentItem.dataset.type == 'video') {
            newItem = {
                html: paprentItem.dataset.video
            };
        } else {
            newItem = {
                src: item.dataset['fullUrl'],
                w: width,
                h: height
            };
        }
        
        result.push(newItem);
    }
    return result;
}

var initGallery = function (container) {
    var holder = container;
    if (!holder) {
        console.warn("Can't find lightbox gallery holder with selector: " + container);
        return;
    }

    var photoSwipeItems = Array.from(holder.querySelectorAll('.gallery__item-opener'));
    var images = holder.querySelectorAll('img') || [];
    if (!images.length) {
        return;
    }

    var galleryModel = {
        uId: Math.random(),
        holder: holder,
        items: buildPhotoswipeItems(images),
        options: helpers.deepClone(defaultOptions)
    }
    galleryModel.options.galleryUID = galleryModel.uId;

    var imageArray = Array.from(images);

    photoSwipeItems.forEach(function (item) {
        item.addEventListener("click", () => {
            galleryModel.options.index = photoSwipeItems.indexOf(item);
            createGallery(galleryModel);
        });
    });

    galleries.push(galleryModel);
}

var controller = {
    init: function () {
        $('.js-lightbox-gallery').each(function (i, el) {
            initGallery(el);
        });
    }
}

export default controller;