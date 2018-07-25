import helpers from "./../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../Core/Content/scripts/UmbracoAjaxForm";
import ajax from "./../Core/Content/scripts/Ajax";
import push from 'push.js';

require("./List/notificationList.css");

var infinityScroll = helpers.infiniteScrollFactory;
var body = document.querySelector('body');
var html = document.querySelector('html');

function initPreviewControls() {
    var notification = document.querySelector(".js-notification");
    var notificationList = document.querySelector(".js-notification-list");
    var notificationBlock = document.querySelector(".notification");

    notification.addEventListener('click', function (e) {
        e.preventDefault();
        if (!html.classList.contains("_notifications-expanded")) {

            ajax.get("/umbraco/surface/Notification/List")
                .then(function (response) {
                    notificationList.innerHTML = response.data;
                    notificationList.classList.remove('_loading');
                    initCustomControls();
                });
            html.classList.add("_notifications-expanded");
        } else {
            html.classList.remove("_notifications-expanded");
        }
    });

    body.addEventListener("click", function (ev) {
        isOutsideClick(notificationBlock, notification, ev.target, "_notifications-expanded");
    });
}

function isOutsideClick(el, trigger, target, classname) {
    if (el && !el.contains(target) && (trigger && !trigger.contains(target)) && html.classList.contains(classname)) {
        html.classList.remove(classname);
        html.removeEventListener("click", isOutsideClick);
    }
}

function initCustomControls() {
    $('.js-notification__list-item').on('click', function () {
        var $this = $(this);
        var delivered = $this.data("viewed");
        var url = $this.data("href");

        if (!delivered) {
            let data = { id: $this.data("id") };
            ajax.post('/umbraco/surface/Notification/View/', data)
                .then(function () {
                    $this.attr("data-viewed", true);
                });
        }

        window.location.href = url;
    });
}

function initInfinityScroll() {
    var holder = $('.js-notification-overview');
    if (!holder.length) return;

    var formController = umbracoAjaxForm(holder.find("form.js-ajax-form")[0]);

    function reload() {
        var promise = formController.reload();
        promise.then(initCustomControls);
        return promise;
    }

    infinityScroll({
        storageName: "notifications",
        loaderSelector: '.js-loading-status',
        $container: $(formController.form),
        reload: reload
    });
}

function getClientHeight() { return document.compatMode == 'CSS1Compat' ? document.documentElement.clientHeight : document.body.clientHeight; }

function updateNotificationsCount() {
    ajax.get("/umbraco/surface/Notification/GetNotNotifiedCount")
        .then((response) => {
            updateCounter(response.data);
        });
}

function updateNotifications() {
    ajax.get("/umbraco/api/DesktopNotification/Get")
        .then((response) => {
            if (response.data.count > 0) {
                var pushedNotificationsCount = 0;
                for (var i = 0; i < response.data.notifications.length; i++) {
                    var notification = response.data.notifications[i];
                    if (push.Permission.has() && notification.isDesktopNotificationEnabled) {
                        sentNotification(notification);
                        pushedNotificationsCount++;
                    }
                }
                updateCounter(response.data.count - temp);
            }
        });
}

function updateCounter(count) {
    var countHolder = $('.js-notification__number');
    if (count > 0) {
        countHolder.html(count);
        countHolder.show();
    } else {
        countHolder.hide();
    }
}

function getPermissions() {
    if (push.Permission.has()) {
        onGranted();
    } else if (push.Permission.get() === push.Permission.DENIED) {
        onDenied();
    } else {
        push.Permission.request(onGranted, onDenied);
    }
}

function sentNotification(notification) {
    push.create(notification.desktopTitle, {
        body: notification.desktopMessage,
        icon: notification.notifierPhoto,
        requireInteraction: true,
        //timeout: 5000,
        onClick: function () {
            var pushWindow = this;
            var url = "/umbraco/api/DesktopNotification/Viewed?id=" + notification.id;
            ajax.post(url).then(result => {
                var destUrl = window.location.origin + notification.url;
                window.focus();
                window.location.assign(destUrl);
                if (equals(destUrl, window.location.href))
                    window.location.reload();
                pushWindow.close();
            });
        },
        onShow: function () { }
    });
}

function equals(destUrl, currentUrl) {
    var destIndex = destUrl.indexOf("#");
    var destPath = destIndex !== -1 ? destUrl.substring(0, destIndex) : destUrl;
    var currentIndex = currentUrl.indexOf("#");
    var currentPath = currentIndex !== -1 ? currentUrl.substring(0, currentIndex) : currentUrl;
    return destPath === currentPath;
}

function onGranted() {
    updateNotifications();
    setInterval(updateNotifications, 3000);
}

function onDenied() {
    updateNotificationsCount();
    setInterval(updateNotificationsCount, 3000);
}

export default function () {
    initPreviewControls();
    getPermissions();
    initCustomControls();
    initInfinityScroll();
}