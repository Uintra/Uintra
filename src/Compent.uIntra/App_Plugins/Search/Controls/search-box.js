require('devbridge-autocomplete');

var body = document.querySelector('body');

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
        noCache: true,
        transformResult: function (response, originalQuery) {
            query = originalQuery;
            var result = {
                suggestions: $.map(response.Documents, function (dataItem) {
                    return { value: dataItem.Title, url: dataItem.Url, type: dataItem.Type };
                })
            };

            if (response.Documents.length) {
                result.suggestions.push({
                    value: seeAllText,
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
    
    var searchContainer = document.querySelector('.search');

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

    body.addEventListener("click", function(ev) {
        isOutsideClick(searchContainer, opener, ev.target, "_search-expanded", function() {
            body.classList.remove("_search-expanded");
        });
    });
};

function isOutsideClick(el, trigger, target, className, callback){
    if (el && !el.contains(target) && (trigger && !trigger.contains(target)) && body.classList.contains(className)) {
        if (typeof callback === "function") {
            callback();
        }
    }
}

export default function () {
    initSearchBox();
    initMobileSearch();
}