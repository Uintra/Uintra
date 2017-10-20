import helpers from "./../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../Core/Content/scripts/UmbracoAjaxForm";
import lightbox from "./../Core/Controls/LightboxGallery/LightboxGallery";
import subscribe from "./../Subscribe/subscribe";
import initOpener from "./openCloseCentralFeed";
import readonlyClickWarning from './../Core/Content/scripts/readonlyClickWarning';

require("./centralFeed.css");

const hideClass = "_hide";
let infinityScroll = helpers.infiniteScrollFactory;
let scrollTo = helpers.scrollTo;

uIntra.events.add("cfTabChanged");
uIntra.events.add("cfReloadTab", {
    detail: {
        isReinit: false
    }
});

uIntra.events.add("cfTabReloaded", {
    detail: {
        isReinit: false
    }
});

let holder;
let navigationHolder;
let extendedState;
let formController;
let reloadintervalId;

function initDescription() {
    let container = $('._clamp');
    if (container.length > 0) {
        for (var i = 0; i < container.length; i++) {
            var target = $(container[i]).data('url');
            helpers.clampText(container[i], target);
        }
    }
}

function showLoadingStatus() {
    let loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "block");
}

function hideLoadingStatus() {
    let loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "none");
}

function initCustomControls(data) {
    if (!data) {
        return;
    }

    lightbox.init();
    initDescription();
    subscribe.initOnLoad();
}

function scrollPrevented() {
    return !!parseInt(holder.querySelector('input[name="preventScrolling"]').value) | false;
}

function attachEventFilter() {
    var clearFiltersElem = formController.form.querySelector('input[name="clearFilters"]');

    if (clearFiltersElem) {
        clearFiltersElem.addEventListener('click', function () {
            var showSubscribed = formController.form.querySelector('input[name="filterState.showSubscribed"]');
            var showPinned = formController.form.querySelector('input[name="filterState.showPinned"]');
            var inlcudeBulletin = formController.form.querySelector('input[name="filterState.includeBulletin"]');
            $(showSubscribed).val(false);
            $(showPinned).val(false);
            $(inlcudeBulletin).val(false);
            goToAllTab();
            reload(false, false, false);
        });
    }

    var showSubscribedElem = formController.form.querySelector('input[name="filterState.showSubscribed"]');
    if (showSubscribedElem) {
        showSubscribedElem.addEventListener('change', function () {
            reload(false, false, false);
        });
    }

    var showPinnedElem = formController.form.querySelector('input[name="filterState.showPinned"]');
    if (showPinnedElem) {
        showPinnedElem.addEventListener('change', function () {
            reload(false, false, false);
        });
    }

    var inlcudeBulletinElem = formController.form.querySelector('input[name="filterState.includeBulletin"]');
    if (inlcudeBulletinElem) {
        inlcudeBulletinElem.addEventListener('change', function () {
            reload(false, false, false);
        });
    }
}

function reload(useVersion, skipLoadingStatus, isReinit) {
    if (!useVersion) {
        holder.querySelector('input[name="version"]').value = null;
    }

    !skipLoadingStatus && showLoadingStatus();

    helpers.state.save(extendedState.storageName);

    var promise = formController.reload();
    promise.then(attachEventFilter);
    promise.then(hideLoadingStatus);
    promise.then(initCustomControls);
    promise.then(function () { emitTabReloadedEvent(isReinit); });
    promise.catch(hideLoadingStatus);
    return promise;
}

function scrollReload() {
    holder.querySelector('input[name="version"]').value = null;
    let promise = formController.reload();
    promise.then(attachEventFilter);
    promise.then(attachReadonlyClickWarning);
    promise.then(initCustomControls);
    promise.then(function () { emitTabReloadedEvent(true); });
    return promise;
}

function attachReadonlyClickWarning() {
    readonlyClickWarning.init();
}
function tabClickEventHandler(e) {
    if (!$(e.target).hasClass('_active') && !$(e.target).closest('._active').length > 0) {
        e.preventDefault();
        window.history.replaceState({}, "", e.currentTarget.dataset["pageUrl"]);
        extendedState.tab = e.currentTarget.dataset['type'];
        $(e.target).closest('.tabset').removeClass('_expanded');
    } else {
        e.preventDefault();
    }

}

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}

function reloadTabEventHandler(e) {
    clearInterval(reloadintervalId);
    let hash = (window.location.hash || "").replace("#", "");

    reload(true, false, e.detail.isReinit).then(function () {
        if (hash) {
            let elem = document.querySelector('[data-anchor="' + hash + '"]');

            if (elem) {
                scrollTo(document.body, elem.offsetTop, 300);
                window.history.pushState("", document.title, window.location.pathname + window.location.search);
            }
        }
    });

    runReloadInverval();
}

function showBulletinsEventHandler(e) {
    goToTab(4);
}

function goToTab(tabNumber) {
    var tab = document.querySelector("[data-type='" + tabNumber + "']");
    var link = $(tab).find('a');
    $(link)[0].click();
}

function goToAllTab() {
    goToTab(0);
}

function goActivityTab(tabNum) {
    goToTab(tabNum);
}

function runReloadInverval() {
    reloadintervalId = setInterval(function () {
        reload(true, true, false);
    }, 30000);
}

function emitTabReloadedEvent(isReinit) {
    uIntra.events.cfTabReloaded.eventBody.detail.isReinit = isReinit;
    uIntra.events.cfTabReloaded.dispatch();
}

function setBulletinCreateMode(feed) {
    feed.classList.add(hideClass);
}

function removeBulletinCreateMode(feed) {
    feed.classList.remove(hideClass);
}

function init() {
    initOpener();
    holder = document.querySelector('.js-feed-overview');
    navigationHolder = document.querySelector('.js-feed-navigation');
    if (!holder || !navigationHolder) return;
    formController = umbracoAjaxForm(holder.querySelector("form.js-ajax-form"));
    let tabs = navigationHolder.querySelectorAll('.js-feed-links .js-feed-type');
    readonlyClickWarning.init();
    extendedState = {
        get tab() {
            var el = navigationHolder.querySelector('.js-feed-links .js-feed-type._active');
            return el && el.dataset['type'];
        },
        set tab(val) {
            var active = '_active';
            for (var i = 0; i < tabs.length; i++) {
                var tab = tabs[i];
                if (tab.dataset['type'] == val) {
                    tab.classList.add(active);
                    holder.querySelector('form input[name="typeId"]').value = val;
                } else {
                    tab.classList.remove(active);
                }
            }

            var element = (document.documentElement && document.documentElement.scrollTop) ? document.documentElement : document.body;
            scrollTo(element, 0, 200);

            reload(false, false, true);

            uIntra.events.cfTabChanged.dispatch();
        },
        get storageName() {
            return "centrallFeed_" + this.tab;
        }
    }

    for (var i = 0; i < tabs.length; i++) {
        var tab = tabs[i];
        tab.addEventListener('click', tabClickEventHandler);
    }

    initDescription();

    infinityScroll({
        storageName: extendedState.storageName,
        loaderSelector: '.js-loading-status',
        $container: $(formController.form),
        reload: scrollReload
    });

    attachEventFilter();
    runReloadInverval();

    uIntra.events.addListener("cfReloadTab", reloadTabEventHandler);

    const feedCreate = document.querySelector(".js-feed-create");
    if (feedCreate) {
        uIntra.events.addListener("setBulletinCreateMode", () => setBulletinCreateMode(feedCreate));
        uIntra.events.addListener("removeBulletinCreateMode", () => removeBulletinCreateMode(feedCreate));
    }
}

export default {
    init,
    reload,
    goToAllTab,
    goActivityTab
}