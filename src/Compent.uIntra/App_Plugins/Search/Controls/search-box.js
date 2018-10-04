require('devbridge-autocomplete');

function initSearchBox(holder) {
    var searchBox = holder.find('.js-searchbox');
    const searchBoxIcon = holder.find('.js-searchbox-icon');

    if (!searchBox.length || !searchBoxIcon.length) {
        return;
    }
    var url = searchBox.data('searchResultsUrl') + '?query=';
    var minChars = 2;

    const emptyText = searchBox.data('emptyText');
    const autocompleteUrl = searchBox.data('autocompleteUrl');

    searchBox.autocomplete({
        serviceUrl: autocompleteUrl,
        paramName: 'query',
        minChars: minChars,
        showNoSuggestionNotice: true,
        noSuggestionNotice: emptyText,
        dataType: 'json',
        noCache: true,
        transformResult: function (response, originalQuery) {
            return {
                suggestions: $.map(response.Documents, function (dataItem) {
                    return { value: dataItem.Title, url: dataItem.Url, html: dataItem.Html };
                })
            };
        },
        formatResult: function (suggestion, currentValue) {
            return suggestion.html;
        },
        onSelect: function (suggestion) {
            window.location = suggestion.url;
        }
    });

    searchBox.on('keypress', function (e) {
        if (e.which === 13 || e.keyCode === 13) {
            var query = $(this).val();
            if (query.length >= minChars) {
                window.location = url + escape(query);
            }
            return false;
        }
        return true;
    });

    searchBoxIcon.on('click', function () {
        var query = searchBox.val();
        if (query.length >= minChars) {
            window.location = url + escape(query);
        }
    });
}

export default function () {
    const holders = $(".js-searchbox-holder");
    if (!holders.length) {
        return;
    }

    holders.each((index, element) => {
        initSearchBox($(element));
    });
}