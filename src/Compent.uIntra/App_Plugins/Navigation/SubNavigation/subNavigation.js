require("./subNavigation.css");

const $body = $('body');
const $subMenu = $('.tabset__navigation');
const $title = $('.tabset__title');

if ($subMenu.length > 0) {
    $body.addClass('_with-sub-nav');
}

if ($title.length > 0) {
    $body.addClass('_with-subtitle');
}

const controller = {
    init: function () {
        //To be continued
    }
}

export default controller;