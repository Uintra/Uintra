import helpers from "./../Core/Content/scripts/Helpers";
import umbracoAjaxForm from "./../Core/Content/scripts/UmbracoAjaxForm";
import ajax from "./../Core/Content/scripts/Ajax";

require("./List/notificationList.css");

var infinityScroll = helpers.infiniteScrollFactory;
var body = document.querySelector('body');

function initPreviewControls() {
    var notification = document.querySelector(".js-notification");
    var notificationList = document.querySelector(".js-notification-list");
    var notificationBlock = document.querySelector(".notification");

    notification.addEventListener('click', function () {
        if (!body.classList.contains("_notifications-expanded")) {
            ajax.Get("/umbraco/surface/Notification/List")
                .then(function (response) {
                    notificationList.innerHTML = response;
                    notificationList.classList.remove('_loading');
                    initCustomControls();
                });
            body.classList.add("_notifications-expanded");
        } else {
            body.classList.remove("_notifications-expanded");
        }
    });

    body.addEventListener("click", function (ev) {
        isOutsideClick(notificationBlock, notification, ev.target, "_notifications-expanded");
    });
}

function isOutsideClick(el, trigger, target, classname) {
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

    $.ajaxSetup({
        cache: false
    });
}

function initCustomControls() {
    $('.js-notification__list-item').on('click', function () {
        var $this = $(this);
        var delivered = $this.data("viewed");
        var url = $this.data("href");

        if (!delivered) {
            $.ajax({
                type: "POST",
                data: { id: $this.data("id") },
                url: "/umbraco/surface/Notification/View/",
                success: function () {
                    $this.attr("data-viewed", true);
                }
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

export default function () {
    initPreviewControls();
    updateNotificationsCount();
    setInterval(updateNotificationsCount, 3000);
    initCustomControls();
    initInfinityScroll();
}