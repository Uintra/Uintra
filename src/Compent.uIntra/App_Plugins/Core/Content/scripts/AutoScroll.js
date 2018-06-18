var additionalDistanceToScroll = 30;

var AutoScroll = function() {
    var currentUrl = window.location.href;
    var topBlocksHeight = $('#header').height() + $('.tabset').height() + additionalDistanceToScroll;
    if (currentUrl.indexOf('#') >= 0) {
        var elementId = currentUrl.split('#').pop();
        $('html, body').animate({
            scrollTop: $('#' + elementId).offset().top - topBlocksHeight
        }, 500);

    }
}

export default AutoScroll;