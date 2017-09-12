import helpers from "./../../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../../Core/Content/scripts/UmbracoAjaxForm";
import ajax from "./../../Core/Content/scripts/Ajax";

require('select2');

const onlyPinnedFilterSelector = 'input[name="onlyPinned"]';

var infinityScroll = helpers.infiniteScrollFactory;
var searchTimeout;
var formController;
var holder;
var select;
let scrollHeight = 0;

var initTypesSelect = function () {
    select.select2({
        placeholder: select.data("placeholder")
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
        function() {
            clearTimeout(searchTimeout);
            var text = searchBox.value;
            if (text.length > 1) {
                searchTimeout = setTimeout(function() {search()}, 250);
            } else {
                document.querySelector(".js-searchResults-listContainer").innerHTML = "";
            }
        });

    select.change(function() {
        search()
    });
}

function initInfinityScroll() {
    formController = umbracoAjaxForm(holder.find("form.js-ajax-form")[0]);

    var state = {
        get page() {
            return holder.find('input[name="page"]').val() || 1;
        },
        set page(val) {
            holder.find('input[name="page"]').val(val);
        },
        get storageName() {
            return "searchResults";
        }
    }

    function saveState() {
        localStorage.setItem(state.storageName, { page: state.page });
    }

    function scrollPrevented() {
        return !!parseInt(holder.find('input[name="preventScrolling"]').val()) | false;
    }

    function showLoadingStatus() {
        $(".js-loading-status").show();
    }

    function hideLoadingStatus() {
        $(".js-loading-status").hide();
    }

    function reload(skipLoadingStatus) {
        !skipLoadingStatus && showLoadingStatus();
        saveState();
        var promise = formController.reload();
        promise.then(hideLoadingStatus);
        promise.catch(hideLoadingStatus);
        return promise;
    }

    function restoreState() {
        var hash = (window.location.hash || "").replace("#", "");
        if (hash) {
            var savedState = localStorage.getItem(state.storageName);

            state.page = (savedState || {}).page || 1;
            reload().then(function () {
                var elem = $('[data-anchor="' + hash + '"]');
                if (!elem) return;
                scrollTo(document.body, elem.offsetTop, 300);
                window.history.pushState("", document.title, window.location.pathname);
            });
        } else {
            localStorage.removeItem(state.storageName);
        }
    }

    function onScroll(done) {
        if (scrollPrevented()) {
            done();
            return;
        }
        state.page++;
        var promise = reload();
        if (isScrollDirectionBottom()) {
            promise.then(done, done);
        }
    }

    restoreState();
    infinityScroll(onScroll)();
}

function isScrollDirectionBottom() {
    const tempScrollHeight = Math.max(scrollHeight, window.pageYOffset);
    scrollHeight = window.pageYOffset;

    return tempScrollHeight < scrollHeight;
}

function search() {
    holder.find('input[name="page"]').val(1);
    formController.reload();
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