import appInitializer from "./../Core/Content/scripts/AppInitializer";
import helpers from "./../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../Core/Content/scripts/UmbracoAjaxForm";

var infinityScroll = helpers.infiniteScrollFactory;
var scrollTo = helpers.scrollTo;
var localStorage = helpers.localStorage;

function updateNotificationsCount() {
    $.ajax({
        url: "/umbraco/surface/Notification/GetNotNotifiedCount",
        success: function (count) {
            var countHolder = $('.notifications__number');
            if (count > 0) {
                countHolder.html(count);
                countHolder.show();
            } else {
                countHolder.hide();
            }
        }
    });
}

function initCustomControls() {
    $('.notifications__list-item').on('click', function () {
        var $this = $(this);
        var delivered = $this.data("viewed");

        if (!delivered) {
            $.ajax({
                url: "/umbraco/surface/Notification/View/" + $this.data("id"),
                success: function () {
                    $this.data("viewed", 'true');
                }
            });
        }
    });
}

function initInfinityScroll() {
    var holder = $('.js-notifications-overview');
    if (!holder.length) return;
    
    var formController = umbracoAjaxForm()(holder.find("form.js-ajax-form")[0]);

    var state = {
        get page() {
            return holder.find('input[name="page"]').val() || 1;
        },
        set page(val) {
            holder.find('input[name="page"]').val(val);
        },
        get storageName() {
            return "notifications";
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
        promise.then(initCustomControls);
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

appInitializer.add(function () {
    updateNotificationsCount();
    setInterval(updateNotificationsCount, 3000);
    initCustomControls();
    initInfinityScroll();
});