﻿require('./leftNavigation.css');
import helpers from "./../../Core/Content/scripts/Helpers";

var container = $('.js-sidebar-inner');
var active = '_expand';
var mobileMediaQuery = window.matchMedia("(max-width: 899px)");

var opener = $('.js-side-nav__opener');

function locationChagned() {
    var path = window.location.pathname;
    var links = document.querySelectorAll('.js-side-nav__link');

    for (var i = 0; i < links.length; i++) {
        var link = links[i];
        if (link.pathname == path) {
            link.parentElement.classList.add(active);
        } else {
            link.parentElement.classList.remove(active);
        }
    }
}

function toggleLinks(el){
    $(el).closest('.js-side-nav__item').toggleClass(active);
}

var controller = {
    init: function () {
        opener.on('click', function(e){
            toggleLinks(this);
        });
        if(!document.querySelector('.ss-container') && !mobileMediaQuery.matches){
            helpers.initScrollbar(document.querySelector('.js-sidebar'));
        }
        document.body.addEventListener('cfTabChanged', locationChagned);
    }
}

export default controller;