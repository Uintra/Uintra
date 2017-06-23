import appInitializer from "./../../Core/Content/scripts/AppInitializer";

var searchTimeout;

function initSearchPage() {
    var searchBox = $('.js-search-page-searchbox');
    if (!searchBox.length) {
        return;
    }

    searchBox.on('input', function () {
        clearTimeout(searchTimeout);
        var text = searchBox.val();
        if (text.length > 1) {
            searchTimeout = setTimeout(function() {search(text)}, 250);
        } else {
            $(".js-search-page-search-result").html("");
        }
    });
}

function search(query) {
    if (query) {
        var url = $('.js-search-page-searchbox').data('searchUrl') + '?query='+ query;

        $.ajax({
            url: url,
            success: function (data) {
                $(".js-search-page-search-result").html(data);
            }
        });
    }
}

appInitializer.add(function () {
    var holder = $('.js-search-page');
    var searchbox = $('.js-search-page-searchbox');
    if (!holder.length || !searchbox.length) {
        return;
    }
    initSearchPage();
});

