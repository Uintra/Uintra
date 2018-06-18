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
                    return { value: dataItem.Title ? dataItem.Title : "", additionalInfo: dataItem.AdditionalInfo, url: dataItem.Url, type: dataItem.Type };
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
            var newData;
            if (suggestion.value) {
                newData = suggestion.value.split('(?i)' + currentValue).join('<strong>' + currentValue + '</strong>');
            } else {
                newData = "";
            }

            if (suggestion.type === 'all') {
                return `<span class='title _all'>${newData}</span>`;
            }

            return `${renderIcon(suggestion.additionalInfo)}<span class='title'>${newData}</span>${renderAdditionData(suggestion.additionalInfo)}<span class='type'>${suggestion.type}</span>`;
        },
        onSelect: function (suggestion) {
            window.location = suggestion.url;
        }
    });

    function renderIcon(data) {
        if (!data) return "";

        const filteredData = data.filter((item) => { return item.Name === "Photo" });
        if (!filteredData.length || !filteredData[0].Value) return "";

        return `<img class="icon" src="${filteredData[0].Value}" />`;
    }

    function renderAdditionData(data) {
        if (!data || !data.length) return "";

        const result = data.map((item) => {
            if (item.Name === "Photo" || item.Value === null || item.Value === "undefined") return "";

            return `<li class="autocomplete-suggestion__frame">${item.Value}</li>`;
        }).join('');

        if (!result) return;

        return `<ul class="autocomplete-suggestion__list">${result}</ul>`;
    }

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

    searchBoxIcon.on('click', function () {
        window.location = url + searchBox.val();
    });
}

function isOutsideClick(el, trigger, target, className, callback) {
    if (el && !el.contains(target) && (trigger && !trigger.contains(target)) && body.classList.contains(className)) {
        if (typeof callback === "function") {
            callback();
        }
    }
}

export default function () {
    initSearchBox();
}