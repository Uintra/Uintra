'use strict';
var SubscribeList = (function () {

    var holder;
    var versionControl;
    var listContainer;
    var activityId;
    var titleContainer;

    function setTitle() {
        var count = listContainer.find('li').length;
        var text = titleContainer.data('text').replace('{count}', count);
        titleContainer.text(text);
    }

    function processSubscribers() {
        $.get("/umbraco/surface/Subscribe/Version?activityId=" + activityId, function (data) {
            if (data.Result !== versionControl.value) {
                versionControl.value = data.Result;
                loadSubscribers(activityId);
            }
        });
    }

    function loadSubscribers() {
        $.get("/umbraco/surface/Subscribe/List?activityId=" + activityId, function (data) {
            listContainer.html(data);
            if (versionControl.value > 0) {
                holder.show();
            } else {
                holder.hide();
            }
            setTitle();
        });
    }

    function init(control) {
        holder = control;
        var interval = holder.data('interval');
        if (!interval) {
            interval = 3000;
        }
        
        versionControl = holder.find('.js-subscribe-version');
        listContainer = holder.find('.js-subscribe-list');
        titleContainer = holder.find('.js-subscribe-title');
        activityId = holder.data('activityId');
        processSubscribers();
        setInterval(processSubscribers, interval);
    }

    var controller = {
        init: function (selector) {
            var holders = $(selector);

            if (!holders.length) {
                return;
            }

            holders.each(function () {
                init($(this));
            });
        }
    }

    App.AppInitializer.add(function() {
        controller.init('.js-subscribe-overview');
    });
})();