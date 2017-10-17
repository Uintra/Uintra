import helpers from "./../../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../../Core/Content/scripts/UmbracoAjaxForm";
import ajax from "./../../Core/Content/scripts/Ajax";

require('select2');

const onlyPinnedFilterSelector = 'input[name="onlyPinned"]';

let infinityScroll = helpers.infiniteScrollFactory;
let searchTimeout;
let formController;
let holder;
let select;
let searchInProgress = false;

let initTypesSelect = function () {
    select.select2({
        placeholder: select.data("placeholder-text")
    });
}

function initOnlyPinnedFilter() {
    $(onlyPinnedFilterSelector).on('change', onlyPinnedFilterChangeHandler);
}

function onlyPinnedFilterChangeHandler(event) {
    search();
}

function initSearchPage() {
    var searchBox = document.querySelector(".js-search-page-searchbox");
    if (!searchBox) {
        return;
    }
    searchBox.addEventListener('input',
        function () {
            clearTimeout(searchTimeout);
            var text = searchBox.value;
            if (text.length > 1 || searchInProgress) {
                searchInProgress = true;
                searchTimeout = setTimeout(function () { search() }, 250);
            } else {
                document.querySelector(".js-searchResults-listContainer").innerHTML = "";
            }
        });

    select.change(function () {
        search()
    });
}

function initInfinityScroll() {
    formController = umbracoAjaxForm(holder.find("form.js-ajax-form")[0]);

    infinityScroll({
        storageName: "searchResults",
        loaderSelector: '.js-loading-status',
        $container: $(formController.form),
        reload: formController.reload
    });
}

//function isScrollDirectionBottom() {
//    const tempScrollHeight = Math.max(scrollHeight, window.pageYOffset);
//    scrollHeight = window.pageYOffset;

//    return tempScrollHeight < scrollHeight;
//}

function search() {
    holder.find('input[name="page"]').val(1);

    formController.reload().then(function() {
        var typeSearchCounts = holder.find(".js-type-search-counts");
        var allTypesPlaceholder = typeSearchCounts.data("all-types-placeholder");
        var types = typeSearchCounts.data("types");

        for (var i = 0; i < types.length; i++) {
            select.find("option[value=" + types[i].id + "]").html(types[i].name);
        }

        select.select2({
            placeholder: allTypesPlaceholder
        });
    });

    initInfinityScroll();
}

export default function () {
    holder = $(".js-search-page-holder");
    select = holder.find('.js-types-select');
    if (!holder.length) {
        return;
    }

    initSearchPage();
    initInfinityScroll();
    initTypesSelect();
    initOnlyPinnedFilter();
    search();
};