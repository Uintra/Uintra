import helpers from "./../../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../../Core/Content/scripts/UmbracoAjaxForm";

let scrollTo = helpers.scrollTo;
let localStorage = helpers.localStorage;
let infinityScroll = helpers.infiniteScrollFactory;

let formController;
let holder = document.querySelector('.js-groups-overview');

let state = {
    get page() {
        return holder.querySelector('input[name="page"]').value || 1;
    },
    set page(val) {
        holder.querySelector('input[name="page"]').value = val;
    },
    get storageName() {
        return "groups";
    }
}

function reload(skipLoadingStatus) {
    !skipLoadingStatus && showLoadingStatus();
    saveState();
    let promise = formController.reload();
    promise.then(hideLoadingStatus);
    promise.catch(hideLoadingStatus);
    return promise;
}

function saveState() {
    localStorage.setItem(state.storageName, { page: state.page });
}

function scrollPrevented() {
    return !!parseInt(holder.querySelector('input[name="preventScrolling"]').value) | false;
}

function showLoadingStatus() {
    let loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "block");
}

function hideLoadingStatus() {
    let loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "none");
}

// TODO: reimplement this according to our libs
//function onScroll(done) {
//    if (scrollPrevented()) {
//        done();
//        return;
//    }
//    state.page++;
//    let promise = reload();
//    promise.then(done, done);
//}

function restoreState() {
    let hash = (window.location.hash || "").replace("#", "");
    if (hash) {
        let savedState = localStorage.getItem(state.storageName);
        state.page = (savedState || {}).page || 1;
        reload().then(function () {
            let elem = document.querySelector('[data-anchor="' + hash + '"]');
            if (!elem) return;
            scrollTo(document.body, elem.offsetTop, 300);
            window.history.pushState("", document.title, window.location.pathname);
        });
    } else {
        localStorage.removeItem(state.storageName);
    }

}

function initInfinityScroll() {
    formController = umbracoAjaxForm($(holder).find("form.js-ajax-form")[0]);

    infinityScroll({
        storageName: "groupsList",
        loaderSelector: '.js-loading-status',
        $container: $(formController.form),
        reload: formController.reload
    });
}

let controller = {
    init: function() {
        if (!holder) return;
        formController = umbracoAjaxForm(holder.querySelector("form.js-ajax-form"));
        
        restoreState();
        initInfinityScroll();
    }
}

export default controller;