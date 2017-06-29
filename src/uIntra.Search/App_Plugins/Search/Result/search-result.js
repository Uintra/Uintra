import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../../Core/Content/scripts/UmbracoAjaxForm";

var infinityScroll = helpers.infiniteScrollFactory;
var searchTimeout;

function initSearchPage() {
    var searchBox = $('.js-search-page-searchbox');
    if (!searchBox.length) {
        return;
    }
    var text = searchBox.val();
    if (text.length > 1) {
        search(text);
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

function initInfinityScroll(holder) {
    var formController = umbracoAjaxForm(holder.find("form.js-ajax-form")[0]);

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
        promise.then(done, done);
    }

    restoreState();
    infinityScroll(onScroll)();
}

function search(query) {
    if (query) {
        var url = $('.js-search-page-searchbox').data('searchUrl') + '?query='+ query;

        $.ajax({
            url: url,
            type:"POST",
            success: function (data) {
                $(".js-searchResults-listContainer").html(data);
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

    initSearchPage(holder);
    initInfinityScroll(holder);
});

