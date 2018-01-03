require("./subNavigation.css");

const mobileMediaQuery = window.matchMedia("(max-width: 899px)");
const $tabset = $('.tabset');
const $subMenu = $('.tabset__navigation');
const $title = $('.tabset__title');
//const $menuHolder = subMenu.closest('.tabset');
const $body = $('body');
const $wrapper = $('#wrapper');
const $header = $('#header');

if ($subMenu.length > 0) {
    $body.addClass('_with-sub-nav');
}

if ($title.length > 0) {
    $body.addClass('_with-subtitle');
}

const initSubMenuPosition = function () {
    if(!$tabset || !$header){
        return;
    }

    let height = $header.outerHeight() + $tabset.outerHeight();

    if($subMenu.length > 1){
        $wrapper.css({
            'padding-top': height + 46 + 'px'
        });
    }
    else{
        $wrapper.css({
            'padding-top': height + 'px'
        });
    }
}
    
// const initMobileMenu = function() {
//     const opener = $subMenu.find('.tabset__navigation-link');

//     if ($subMenu.find('._selected').length <= 0) {
//         $subMenu.find('.tabset__navigation-item:first-child').addClass('_selected');
//     }

//     $subMenu.each(function () {
//         if ($(this).closest('.tabset__inner').length == 0) {
//             $(this).css({
//                 'padding-top': '50px'
//             });
//         }
//     });

//     if ($body.hasClass('_with-sub-nav')) {
//         const height = $tabset.outerHeight() + $header.outerHeight();

//         $wrapper.css({
//             'padding-top': height + 'px'
//         })
//     }

//     if (opener.length > 1) {
//         opener.on("click", function (e) {
//             if ($(this).closest('li').hasClass('_selected')) {
//                 e.preventDefault();
//                 $menuHolder.toggleClass('_expanded');
//                 $body.removeClass('_search-expanded notifications-expanded _menu-expanded _sidebar-expanded');
//             }
//         });
//     }
//     else {
//         opener.closest('.tabset__navigation-item').addClass('_disabled');
//     }
// }

const controller = {
    init: function () {
        initSubMenuPosition();
        $(window).resize(function(){
            initSubMenuPosition();
        });

        if (mobileMediaQuery.matches) {
            //initMobileMenu();
        }
    }
}

export default controller;