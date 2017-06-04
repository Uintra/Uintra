import appInitializer from "./AppInitializer";

appInitializer.add(function () {
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
});