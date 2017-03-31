'use strict';
var Subscribe = (function () {
    var callbacks = [];

    function initNotificationDisable(holder) {
        var notificationDisableControl = holder.find('.js-subscribe-notification-disable');

        if (notificationDisableControl.length === 0) {
            return;
        }

        var type = holder.find('.js-subscribe-type').val();
        notificationDisableControl.on('change', function () {
            var sender = $(this);
            var id = sender.data("id");
            sender.disabled = true;
            $.post('/umbraco/surface/Subscribe/ChangeNotificationDisabled', { Id: id, NewValue: this.checked, type: type }, function () {

            }).always(function () {
                sender.disabled = false;
            });
        });
    }

    function init(holder) {
        initNotificationDisable(holder);
    }

    var controller = {
        initOnLoad: function () {
            this.init('.js-subscribe');
        },
        init: function (selector) {
            var holders = $(selector);

            if (!holders.length) {
                return;
            }

            holders.each(function () {
                init($(this));
            });
        },
        addOnSubscribe: function (callback) {
            callbacks.push(callback);
        },
        notify: function (data) {
            callbacks.forEach(function (cb) {
                cb(data);
            });
        }
    }

    App.AppInitializer.add(function () {
        controller.initOnLoad();
    });

    return controller;
})();

window.App = window.App || {};
window.App.Subscribe = Subscribe;