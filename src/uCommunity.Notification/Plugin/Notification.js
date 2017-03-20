(function () {
    'use strict';

    App.AppInitializer.add(function () {

        function updateNotificationsCount() {
            $.ajax({
                url: "/umbraco/surface/Notification/NotNotifiedCount",
                success: function (count) {
                    var countHolder = $('.notifications__number');
                    if (count > 0) {
                        countHolder.html(count);
                        countHolder.show();
                    } else {
                        countHolder.hide();
                    }

                }
            });
        }

        updateNotificationsCount();

        var holder = $('#header');
        if (!holder.length) {
            return;
        }
        setInterval(updateNotificationsCount, 3000);

        var notifications = Array.from($('.notifications__list-item'));
        notifications.forEach(function (notification) {
            var id = $(notification).data("id");            
            notification.addEventListener('click',
            function () {
                var delivered = $(notification).data("viewed");
                if (!delivered) {
                    $.ajax({
                        url: "/umbraco/surface/Notification/ViewNotification/" + id,
                        success: function () {
                            $(notification).data("viewed", true);
                        }

                    });
                }
            });
        });

    });
})();