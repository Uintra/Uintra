import appInitializer from "./../../Core/Content/scripts/AppInitializer";

require('devbridge-autocomplete');
require("./_searchResult.css");

var holder;
var searchTimeout;

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

function initSearchBox() {
    var searchBox = $('.js-searchbox');
    var searchBoxIcon = $('.js-searchbox-icon');

    if (!searchBox.length || !searchBoxIcon.length) {
        return;
    }
    var url = searchBox.data('searchResultsUrl') + '?query=';
    var query;
    var minChars = 2;

    var emptyText = searchBox.data('emptyText');
    var autocompleteUrl = searchBox.data('autocompleteUrl');
    var seeAllText = searchBox.data('seeAllText');

    searchBox.autocomplete({
        serviceUrl: autocompleteUrl,
        paramName: 'query',
        minChars: minChars,
        showNoSuggestionNotice: true,
        noSuggestionNotice: emptyText,
        dataType: 'json',
        transformResult: function (response, originalQuery) {
            query = originalQuery;
            var result = {
                suggestions: $.map(response.Documents, function (dataItem) {
                    return { value: dataItem.Title, data: dataItem.Id, url: dataItem.Url, type: dataItem.Type };
                })
            };

            if (response.Documents.length) {
                result.suggestions.push({
                    value: seeAllText,
                    data: -1,
                    url: url + originalQuery,
                    type: 'all'
                });
            }
            return result;
        },
        formatResult: function (suggestion, currentValue) {
            var newData = suggestion.value.split('(?i)' + currentValue).join('<strong>' + currentValue + '</strong>');
            if (suggestion.type === 'all') {
                return "<span class='title _all'>" + newData + "</span>";
            }

            return "<span class='type'>" + suggestion.type + "</span><span class='title'>" + newData + "</span>";
        },
        onSelect: function (suggestion) {
            window.location = suggestion.url;
        }
    });

    searchBox.on('keypress', function (e) {
        if (e.which == 13 || e.keyCode == 13) {
            var query = $(this).val();
            if (query.length >= minChars) {
                window.location = url + query;
            }
            return false;
        }
        return true;
    });

    searchBoxIcon.on('click',function() {
        window.location = url + searchBox.val();
    });
}

function initMobileSearch() {
    var opener = document.querySelector("#js-search-opener");
    if (!opener) {
        return;
    }

    var body = document.querySelector('body');

    opener.addEventListener('click',
        () => {
            body.classList.toggle('_search-expanded');
            if (body.classList.contains('_sidebar-expanded')) {
                body.classList.remove('_sidebar-expanded');
            }
            if (body.classList.contains('_menu-expanded')) {
                body.classList.remove('_menu-expanded');
            }
        });
};

appInitializer.add(function () {
    initSearchBox();
    initMobileSearch();
    holder = $('#js-search-page');
    var query = $('#query').val();
    if (!holder.length) {
        return;
    }
    initSearchPage();
    search(query);
});

