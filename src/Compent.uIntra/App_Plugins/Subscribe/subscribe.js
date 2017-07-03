import subscribeList from"./subscribeList";

require("./../Core/Content/libs/jquery.unobtrusive-ajax.min.js");
require("./subscribe.css");

function initNotificationDisable(holder) {
    var notificationDisableControl = holder.find('.js-subscribe-notification-disable');

    if (notificationDisableControl.length === 0) {
        return;
    }

    var activityId = holder.find('.js-subscribe-activity-id').val();
    notificationDisableControl.on('change', function () {
        var sender = $(this);
        var id = sender.data("id");
        sender.disabled = true;
        $.post('/umbraco/surface/Subscribe/ChangeNotificationDisabled', { Id: id, NewValue: this.checked, activityId: activityId }).always(function () {
            sender.disabled = false;
        });

        changeNotificationLabel(holder, this);
    });

    changeNotificationLabel(holder, notificationDisableControl[0]);
}

function init(holder) {
    initNotificationDisable(holder);
}

function changeNotificationLabel(holder, control) {
    var isEnabled = !control.checked;
    var label = holder.find('.js-subscribe-notification-label');
    var text = isEnabled ? label.data('textEnabled') : label.data('textDisabled');
    label.html(text);
}

var callbacks = [];
var controller = {
    initOnLoad: function () {
        this.init('.js-subscribe');
    },
    init: function (selector) {
        subscribeList.init();

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
};

window.subscribe = controller;

export default controller;