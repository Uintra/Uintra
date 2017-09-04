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
    var notificationBlock = document.querySelector(".notification");
    notification.addEventListener('click', function() {
        if (!body.classList.contains("_notifications-expanded")) { 
            ajax.Get("/umbraco/surface/Notification/List")
            .then(function (response) {
                notificationList.innerHTML = response;
                notificationList.classList.remove('_loading');
                initDesrcription();
                initCustomControls();
            });
            body.classList.add("_notifications-expanded");
        } else {
            body.classList.remove("_notifications-expanded");
        }
    });

    body.addEventListener("click", function(ev) {
        isOutsideClick(notificationBlock, notification, ev.target, "_notifications-expanded");
    });
}

function isOutsideClick(el, trigger, target, classname){
    if (el && !el.contains(target) && (trigger && !trigger.contains(target)) && body.classList.contains(classname)) {
        body.classList.remove(classname);
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

    $.ajaxSetup ({
        cache: false
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

function initDesrcription(){
    var item = $(".js-notification__list-item");
    item.each(function(i){
        var title = $(item[i]).find(".js-notification__list-heading");
        var url = title.data("url");
        title.contents().wrap("<a href='" + url +"' class='notification__list-heading-link'></a>")
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