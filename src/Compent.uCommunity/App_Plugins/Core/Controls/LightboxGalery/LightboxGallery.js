var Lightbox = (function () {
    'use strict';
    var helpers = window.Helpers;
    var photoswipeElement = document.querySelector('.pswp');
    var galleries = [];
    var defaultOptions = {
        escKey: true,
        arrowKeys: true,
        bgOpacity: 0.8,
        showHideOpacity: false
    };

    var createGallery = function (gallery) {
        var bodyElement = document.querySelector('body');
        gallery.instance = new Photoswipe(photoswipeElement,
            photoswipeUiDefault,
            gallery.items,
            App.Helpers.deepClone(gallery.options));
        gallery.instance.init();
        bodyElement.classList.add('js-lightbox-open');
        gallery.instance.listen('close',
            function () {
                gallery.instance = null;
                bodyElement.classList.remove('js-lightbox-open');
            });
    }

    var buildPhotoswipeItems = function (imagesItems) {
        var result = [];

        for (var i = 0; i < imagesItems.length; i++) {
            var item = imagesItems[i];
            var size = item.getAttribute('data-full-size').split('x');
            var width;
            var height;

            if (size && size.length == 2) {
                width = parseInt(size[0]);
                height = parseInt(size[1]);
            } else {
                width = window.screen.availWidth;
                height = window.screen.availHeight;
            }

            var newItem = {
                src: item.dataset['fullUrl'],
                w: width,
                h: height
            }
            result.push(newItem);
        }
        return result;
    }

    var initGalley = function (container) {

        var holder = container;
        if (!holder) {
            console.warn("Can't find lightbox galery holder with selector: " + selector);
            return;
        }

        var images = holder.querySelectorAll('img') || [];
        if (!images.length) {
            return;
        }

        var galleryModel = {
            uId: Math.random(),
            holder: holder,
            items: buildPhotoswipeItems(images),
            options: App.Helpers.deepClone(defaultOptions)
        }
        galleryModel.options.galleryUID = galleryModel.uId;

        var imageArray = Array.from(images);

        imageArray.forEach(function (image) {
            image.addEventListener("click",
                () => {
                    galleryModel.options.index = imageArray.indexOf(image);
                    createGallery(galleryModel);
                });
        });

        galleries.push(galleryModel);
    }

    var controller = {
        init: function () {
            $('.js-lightbox-galley').each(function () {
                initGalley(this);
            });
        }
    }
    App.AppInitializer.add(controller.init);

    return controller;
})();

window.App = window.App || {};
window.App.Lightbox = Lightbox;