require('./leftNavigation.css');

var container = $('#sidebar');
var active = '_expand';

if(localStorage.getItem('sidebar')) {
    container.html(localStorage.getItem('sidebar'));
}

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

function toggleLinks(el, event, key){
    if(localStorage.getItem(key)) {
        localStorage.removeItem(sidebar);
    }
    $(el).closest('.js-side-nav__item').toggleClass(active);
    container = $('#sidebar');
    var content = container.html();
    localStorage.setItem(key, content);
}

var controller = {
    init: function () {
        opener.on('click', function(e){
            toggleLinks(this, e, 'sidebar');
        });
        document.body.addEventListener('cfTabChanged', locationChagned);
    }
}

export default controller;