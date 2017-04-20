import appInitializer from "./../Core/Content/scripts/AppInitializer";
import helpers from "./../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../Core/Content/scripts/UmbracoAjaxForm";
import lightbox from "./../Core/Controls/LightboxGallery/LightboxGallery";
import subscribe from "./../Subscribe/Subscribe";

var infinityScroll = helpers.infiniteScrollFactory;
var scrollTo = helpers.scrollTo;
var localStorage = helpers.localStorage;
var centralFeedTabEvent = new CustomEvent("cfTabChanged");

var holder;
var state;
var formController;

function showLoadingStatus() {
    var loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "block");
}

function hideLoadingStatus() {
    var loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "none");
}

function initCustomControls(data) {
    if (!data) {
        return;
    }

    lightbox.init();
    subscribe.initOnLoad();
}

function scrollPrevented() {
    return !!parseInt(holder.querySelector('input[name="preventScrolling"]').value) | false;
}

function attachEventFilter() {
    var showSubscribedElem = formController.form.querySelector('input[name="showSubscribed"]');
    if (showSubscribedElem) {
        showSubscribedElem.addEventListener('change', function () {
            reload();
        });
    }
}

function reload(useVersion, skipLoadingStatus) {
    if (!useVersion) {
        holder.querySelector('input[name="version"]').value = null;
    }

    !skipLoadingStatus && showLoadingStatus();

    saveState();
    var promise = formController.reload();
    promise.then(attachEventFilter);
    promise.then(hideLoadingStatus);
    promise.then(initCustomControls);
    promise.catch(hideLoadingStatus);
    return promise;
}

function saveState() {
    localStorage.setItem(state.storageName, { page: state.page });
}

function restoreState() {
    var hash = (window.location.hash || "").replace("#", "");
    if (hash) {
        var savedState = localStorage.getItem(state.storageName);
        state.page = (savedState || {}).page || 1;
        reload().then(function () {
            var elem = document.querySelector('[data-anchor="' + hash + '"]');

            if(elem){
                scrollTo(document.body, elem.offsetTop, 300);
                window.history.pushState("", document.title, window.location.pathname);
            }
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

function tabClickEventHandler(e) {
    if (!$(e.target).hasClass('_active') && !$(e.target).closest('._active').length > 0) {
        e.preventDefault();
        window.history.replaceState({}, "", e.currentTarget.dataset["pageUrl"]);
        state.tab = e.currentTarget.dataset['type'];

        $(e.target).closest('.tabset').removeClass('_expanded');
    }
}

appInitializer.add(function () {
    holder = document.querySelector('.js-feed-overview');
    if (!holder) return;

    formController = umbracoAjaxForm()(holder.querySelector("form.js-ajax-form"));
    var tabs = holder.querySelectorAll('.js-feed-links .js-feed-type');
    
    state = {
        get tab() {
            return holder.querySelector('.js-feed-links .js-feed-type._active').dataset['type'];
        },
        set tab(val) {
            var active = '_active';
            for (var i = 0; i < tabs.length; i++) {
                var tab = tabs[i];
                if (tab.dataset['type'] == val) {
                    tab.classList.add(active);
                    holder.querySelector('form input[name="type"]').value = val;
                } else {
                    tab.classList.remove(active);
                }
            }
            scrollTo(document.body, 0, 200);
            reload();
            document.body.dispatchEvent(centralFeedTabEvent);
        },
        get page() {
            return holder.querySelector('input[name="page"]').value || 1;
        },
        set page(val) {
            holder.querySelector('input[name="page"]').value = val;
        },
        get storageName() {
            return "centrallFeed_" + this.tab;
        }
    }

    for (var i = 0; i < tabs.length; i++) {
        var tab = tabs[i];
        tab.addEventListener('click', tabClickEventHandler);
    }

    restoreState();
    infinityScroll(onScroll)();
    attachEventFilter();
    setInterval(function () { reload(true, true) }, 30000);
});
