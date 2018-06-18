function initAnchorScroll() {
    // navbar height 
    var navHeigth = 170;

    // Scroll to anchor function
    var scrollToAnchor = function (hash) {
        // If got a hash
        if (hash) {
            // Scroll to the top (prevention for Chrome)
            window.scrollTo(0, 0);

            // Hash in IE could contain only hash symbol
            if (hash.substr(1).length === 0) {
                return;
            }

            // Anchor element
            var term = $(hash);
            // If element with hash id is defined
            if (term.length) {

                // Get top offset, including header height
                var scrollto = term.offset().top - navHeigth;

                // Capture id value
                var id = term.attr('id');
                // Capture name value
                var name = term.attr('name');

                // Remove attributes for FF scroll prevention
                term.removeAttr('id').removeAttr('name');

                // Returning id and name after .5sec for the next scroll
                setTimeout(function () {
                    // Scroll to element
                    $('html, body').animate({ scrollTop: scrollto }, 200);
                    term.attr('id', id).attr('name', name);
                }, 500);
            }
        }
    };

    // if we are opening the page by url
    if (location.hash) {
        scrollToAnchor(location.hash);
    }
}

export default initAnchorScroll;