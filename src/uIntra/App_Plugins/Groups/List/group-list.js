import helpers from "./../../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../../Core/Content/scripts/UmbracoAjaxForm";

var scrollTo = helpers.scrollTo;
var localStorage = helpers.localStorage;

var formController;
var holder = document.querySelector('.js-groups-overview');

var state = {
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
    var promise = formController.reload();
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
    var loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "block");
}

function hideLoadingStatus() {
    var loadingElem = document.querySelector(".js-loading-status");
    loadingElem && (loadingElem.style.display = "none");
}

// TODO: reimplement this according to our libs
//function onScroll(done) {
//    if (scrollPrevented()) {
//        done();
//        return;
//    }
//    state.page++;
//    var promise = reload();
//    promise.then(done, done);
//}

function restoreState() {
    var hash = (window.location.hash || "").replace("#", "");
    if (hash) {
        var savedState = localStorage.getItem(state.storageName);
        state.page = (savedState || {}).page || 1;
        reload().then(function () {
            var elem = document.querySelector('[data-anchor="' + hash + '"]');
            if (!elem) return;
            scrollTo(document.body, elem.offsetTop, 300);
            window.history.pushState("", document.title, window.location.pathname);
        });
    } else {
        localStorage.removeItem(state.storageName);
    }

}      

var controller = {
    init: function() {
        if (!holder) return;
        formController = umbracoAjaxForm(holder.querySelector("form.js-ajax-form"));
        restoreState();
    }
}

export default controller;