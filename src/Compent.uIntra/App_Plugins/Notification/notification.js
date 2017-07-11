import helpers from "./../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../Core/Content/scripts/UmbracoAjaxForm";
import ajax from "./../Core/Content/scripts/Ajax";

require("./List/notificationList.css");

var infinityScroll = helpers.infiniteScrollFactory;
var scrollTo = helpers.scrollTo;
var localStorage = helpers.localStorage;
var body = document.querySelector('body');

function initPreviewControls() {
    var notification = document.querySelector(".js-notification");
    var notificationList = document.querySelector(".js-notification-list");
    notification.addEventListener('click', function() {
        if (notificationList.classList.contains("hide")) { 
            ajax.Get("/umbraco/surface/Notification/List")
            .then(function (response) {
                notificationList.innerHTML = response;
                notificationList.classList.remove('_loading');
                initCustomControls();
            });
            notificationList.classList.remove("hide");
        } else {
            notificationList.classList.add("hide");
        }
    });

    body.addEventListener("click", function(ev) {
        isOutsideClick(notificationList, notification, ev.target, "hide");
        
    });
}

function isOutsideClick(el, trigger, target, classname){
    if (el && !el.contains(target) && (trigger && !trigger.contains(target)) && !el.classList.contains(classname)) {
        el.classList.add(classname);
        body.removeEventListener("click", isOutsideClick);
    }
}

function updateNotificationsCount() {
    $.ajax({
        url: "/umbraco/surface/Notification/GetNotNotifiedCount",
        success: function (count) {
            var countHolder = $('.js-notification__number');
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
    $('.js-notification__list-item').on('click', function () {
        var $this = $(this);
        var delivered = $this.data("viewed");

        if (!delivered) {
            $.ajax({
                type: "POST",
                data: {id: $this.data("id")},
                url: "/umbraco/surface/Notification/View/",
                success: function () {
                    $this.attr("data-viewed", true);
                }
            });
        }
    });
}

function initInfinityScroll() {
    var holder = $('.js-notification-overview');
    if (!holder.length) return;
    
    var formController = umbracoAjaxForm(holder.find("form.js-ajax-form")[0]);

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

export default function() {
    initPreviewControls();
    updateNotificationsCount();
    setInterval(updateNotificationsCount, 3000);
    initCustomControls();
    initInfinityScroll();
}