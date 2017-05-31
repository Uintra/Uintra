import appInitializer from "./../Core/Content/scripts/AppInitializer";
import helpers from "./../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../Core/Content/scripts/UmbracoAjaxForm";
import lightbox from "./../Core/Controls/LightboxGallery/LightboxGallery";
import subscribe from "./../Subscribe/subscribe";

var infinityScroll = helpers.infiniteScrollFactory;
var scrollTo = helpers.scrollTo;
var localStorage = helpers.localStorage;

var holder;
var state;
var formController;
var centralFeedTabEvent = new CustomEvent("cfTabChanged");
var centralFeedTabReloadedEvent = new CustomEvent("cfTabReloaded",{
    detail: {
        isReinit: false
    }
});

function showLoadingStatus() {
    var loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "block");
}

function hideLoadingStatus() {
    var loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "none");
}

function displayDescription() {
    var container = $('._clamp');
    if (container.length > 0) {
        for (var i = 0; i < container.length; i++) {
            if (container[i].textContent.trim().length > 300) {
                helpers.clampText(container[i]);
            }
        }
    }
}

function initCustomControls(data) {
    if (!data) {
        return;
    }

    lightbox.init();
    subscribe.initOnLoad();
    displayDescription();
}

function scrollPrevented() {
    return !!parseInt(holder.querySelector('input[name="preventScrolling"]').value) | false;
}

function attachEventFilter() {

    var clearFiltersElem = formController.form.querySelector('input[name="clearFilters"]');
    if (clearFiltersElem) {
        clearFiltersElem.addEventListener('click', function () {
            var showSubscribedFilter = formController.form.querySelector('input[name="showSubscribedFilter"]');
            var showSubscribed=formController.form.querySelector('input[name="showSubscribed"]');
            var showPinnedFilter = formController.form.querySelector('input[name="showPinnedFilter"]');
            var showPinned = formController.form.querySelector('input[name="showPinned"]');
            var inlcudeBulletinFilter = formController.form.querySelector('input[name="includeBulletinFilter"]');
            var inlcudeBulletin = formController.form.querySelector('input[name="includeBulletin"]');
            $(showSubscribedFilter).val(false);
            $(showSubscribed).val(false);
            $(showPinned).val(false);
            $(showPinnedFilter).val(false);
            $(inlcudeBulletinFilter).val(false);
            $(inlcudeBulletin).val(false);
            reload(false, false, false);
        });
    }

    var showSubscribedFilter = formController.form.querySelector('input[name="showSubscribedFilter"]');
    if (showSubscribedFilter) {
        showSubscribedFilter.addEventListener('change', function () {
            var showSubscribed=formController.form.querySelector('input[name="showSubscribed"]');
            if ($(showSubscribedFilter).is(':checked')) {
                $(showSubscribedFilter).val(true);
                $(showSubscribed).val(true);
            } else {
                $(showSubscribedFilter).val(false);
                $(showSubscribed).val(false);
            }
            reload(false, false, false);
        });
    }

    var showPinnedFilter = formController.form.querySelector('input[name="showPinnedFilter"]');
    if (showPinnedFilter) {
        showPinnedFilter.addEventListener('change', function () {
            var showPinned = formController.form.querySelector('input[name="showPinned"]');
            if ($(showPinnedFilter).is(':checked')) {
                $(showPinned).val(true);
                $(showPinnedFilter).val(true);
            } else {
                $(showPinned).val(false);
                $(showPinnedFilter).val(false);
            }
            reload(false, false, false);
        });
    }

    var inlcudeBulletinFilter = formController.form.querySelector('input[name="includeBulletinFilter"]');
    if (inlcudeBulletinFilter) {
        inlcudeBulletinFilter.addEventListener('change', function () {
            var inlcudeBulletin = formController.form.querySelector('input[name="includeBulletin"]');
            if ($(inlcudeBulletinFilter).is(':checked')) {
                $(inlcudeBulletinFilter).val(true);
                $(inlcudeBulletin).val(true);
            } else {
                $(inlcudeBulletinFilter).val(false);
                $(inlcudeBulletin).val(false);
            }
            reload(false, false, false);
        });
    }
}

function reload(useVersion, skipLoadingStatus, isReinit) {
    if (!useVersion) {
        holder.querySelector('input[name="version"]').value = null;
    }

    !skipLoadingStatus && showLoadingStatus();

    saveState();
    var promise = formController.reload();
    promise.then(attachEventFilter);
    promise.then(hideLoadingStatus);
    promise.then(initCustomControls);
    promise.then(function() { emitTabReloadedEvent(isReinit); });
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
        reload(false, false, true).then(function () {
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
    var promise = reload(false, false, true);
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

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}

function reloadTabEventHandler(e) {   
    clearInterval(reloadintervalId);

    reload(true, true, e.detail.isReinit);

    runReloadInverval();
}

function runReloadInverval() {
    reloadintervalId = setInterval(function() {
        reload(true, true, false);
    }, 30000);
}

function emitTabReloadedEvent(isReinit) {
    centralFeedTabReloadedEvent.detail.isReinit = isReinit;
    document.body.dispatchEvent(centralFeedTabReloadedEvent);
}

appInitializer.add(function () {
    holder = document.querySelector('.js-feed-overview');
    var navigationHolder = document.querySelector('.js-feed-navigation');
    if (!holder || !navigationHolder) return;
    formController = umbracoAjaxForm(holder.querySelector("form.js-ajax-form"));
    var tabs = navigationHolder.querySelectorAll('.js-feed-links .js-feed-type');
    
    state = {
        get tab() {
            return navigationHolder.querySelector('.js-feed-links .js-feed-type._active').dataset['type'];
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

            var element = (document.documentElement && document.documentElement.scrollTop) ? document.documentElement : document.body;
            scrollTo(element, 0, 200); 
            
            reload(false, false, true);

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
    runReloadInverval();

    document.body.addEventListener('cfReloadTab', reloadTabEventHandler);
});
