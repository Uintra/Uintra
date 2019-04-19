import helpers from "./../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../Core/Content/scripts/UmbracoAjaxForm";
import ajax from "./../Core/Content/scripts/Ajax";
import push from 'push.js';
import { debug } from "util";

require("./List/notificationList.css");

var infinityScroll = helpers.infiniteScrollFactory;
var body = document.querySelector('body');
var html = document.querySelector('html');
var mobileMediaQuery = window.matchMedia("(max-width: 900px)");

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
        //debugger;
        if (!delivered) {
            let data = { id: $this.data("id") };
            ajax.post('/umbraco/surface/Notification/View/', data)
                .then(function () {
                    $this.attr("data-viewed", true);
                });
        } else location.reload(true); 

        if (window.location.href != url) {
            window.location.href = url;
        }
        
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
    if (push.Permission.has())
        updateNotifications();
    else
        ajax.get("/umbraco/surface/Notification/GetNotNotifiedCount")
            .then((response) => {
                updateCounter(response.data);
            });
}

function updateNotifications() {
    ajax.get("/umbraco/surface/Notification/GetNotNotifiedNotifications")
        .then((response) => {
            if (response.data.count > 0) {
                var i = 0;
                var interval = setInterval(function (notifications) {
                    var notification = notifications[i];
                    if (push.Permission.has() && notification.isDesktopNotificationEnabled) {
                        sentNotification(notification);
                    }
                    if (++i >= response.data.count) {
                        clearInterval(interval);
                        updateCounter(response.data.count);
                    }
                }, 200, response.data.notifications);
            }
            else {
                updateCounter(0);
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
    
    if (mobileMediaQuery.matches) {
        onDenied();
        return;
    }
    if (push.Permission.has()) {
        onGranted();
    } else if (push.Permission.get() === push.Permission.DENIED) {
        onDenied();
    } else {
        push.Permission.request(onGranted, onDenied);
    }
}

function sentNotification(notification) {
    ajax.post("/umbraco/surface/Notification/Notified?id=" + notification.id);

    const defaulAvatar = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MDFBMDYyNzUxMzZCMTFFOTk5NDJENkNBN0M1NDVGQTAiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6MDFBMDYyNzYxMzZCMTFFOTk5NDJENkNBN0M1NDVGQTAiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDowMUEwNjI3MzEzNkIxMUU5OTk0MkQ2Q0E3QzU0NUZBMCIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDowMUEwNjI3NDEzNkIxMUU5OTk0MkQ2Q0E3QzU0NUZBMCIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pk3TRCkAAAFHSURBVHja7JfZaoRAEEXLBUSRuIAbPor//zs++WKQEXFJK6jgkpQwZMGgaeI0DBaIIjT30H1raW5ZlgQAXoBNEO4DYAGGwSMFQ33CA+O4AC4AkXZhXdfrM00TOI4Dqqo+DiBJEsjzHO4lhBACtm2D7/vnH0Hbtt/EMXAXbrcbDMPwGICfxZPjuPXdNA1bE87zzBbgvhOnAvzWu1AcvXA6gCRJm/9RXNO08wFM09wUMgwDZFk+HwCNZlkW8PznUkEQwHVdKhPiQPJ2ZCKqqgrKslxTDYW2UhFBFEUBz/OOVkayC4BnG8fxKnxkeEIQBNR1HYIg2MsMIu45Pooi6Pv+T1mCokVRrN9hGNJ7AMtr13V0TUYU1x6RZRk9QJqmVMXla2DfoO6G2OHGcaSGQC+gKf8lC66p+AJ4WgDml1OsA68s0/BdgAEAmjaocUhguUgAAAAASUVORK5CYII=';

    var objParam = {
        body: notification.desktopMessage,
        icon: notification.notifierPhoto || defaulAvatar,
        requireInteraction: true,
        timeout: 10000,
        onClick: function () {
            var pushWindow = this;
            var url = "/umbraco/surface/Notification/Viewed?id=" + notification.id;
            ajax.post(url).then(function (result) {
                var destUrl = window.location.origin + notification.url;
                window.focus();
                window.location.assign(destUrl);
                if (equals(destUrl, window.location.href))
                    window.location.reload();
                pushWindow.close();
            });
        },
        onShow: function () { }
    };

    push.create(notification.desktopTitle, objParam);
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