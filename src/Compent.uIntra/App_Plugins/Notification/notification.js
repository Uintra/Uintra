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
            let count = response.data;
            var countHolder = $('.js-notification__number');
            if (count > 0) {
                countHolder.html(count);
                countHolder.show();
            } else {
                countHolder.hide();
            }
        });
}

function updateNotifications() {
    //ajax.get("/umbraco/surface/Notification/GetNotNotifiedNotifications")
    ajax.get("/umbraco/api/DesktopNotification/Get")
        .then((response) => {
            console.log(response.data);
            if (response.data.count > 0) {
                if (push.Permission.has()) {
                    sentNotification(response.data.notifications);
                } else {
                    console.log("BOOM!!!!");
                }
            }
        });
}

function getPermissions() {
    if (push.Permission.has()) {
        onGranted();
    } else if (push.Permission.get() === push.Permission.DENIED) {
        console.log("You are block permissions!");
        //push.Permission.request(onGranted, onDenied);
        onDenied();

    } else {
        push.Permission.request(onGranted, onDenied);
    }

}

function sentNotification(notifications) {
    console.log(notifications);
    for (var i = 0; i < notifications.length; i++) {
        var n = notifications[i];
        var url = n.url;
        var _window = window;
        push.create("Some title", {
            body: n.message,
            icon: n.notifierPhoto,
            //timeout: 5000,
            requireInteraction: true,
            tag: n.id,
            onClick: function () {
                _window.focus();
                _window.location.href = url;
                _window.location.reload();
                this.close();
            },
            onShow: function () {
                console.log("Notification is shown!");
            }
        });
    }
}

function onGranted() {
    console.log("You granted desktop notifications!");
    updateNotifications();
    setInterval(updateNotifications, 5000);
}

function onDenied() {
    console.log("You denied desktop notifications!");
    updateNotificationsCount();
    setInterval(updateNotificationsCount, 3000);
}

export default function () {
    initPreviewControls();
    getPermissions();
    initCustomControls();
    initInfinityScroll();
}

