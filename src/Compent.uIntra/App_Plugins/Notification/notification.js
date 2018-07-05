import helpers from "./../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../Core/Content/scripts/UmbracoAjaxForm";
import ajax from "./../Core/Content/scripts/Ajax";

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
    ajax.get("/umbraco/surface/Notification/GetNotNotifiedNotifications")
        .then((response) => {
            console.log(response.data);
            let count = response.data.count;
            var countHolder = $('.js-notification__number');
            if (count > 0) {
                countHolder.html(count);
                countHolder.show();
            } else {
                countHolder.hide();
            }
        });
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

export default function () {
    initPreviewControls();
    updateNotifications();
    setInterval(updateNotifications, 3000);
    initCustomControls();
    initInfinityScroll();
}

