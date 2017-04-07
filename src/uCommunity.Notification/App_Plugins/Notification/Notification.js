(function () {
    'use strict';

    App.AppInitializer.add(function () {

        function updateNotificationsCount() {
            $.ajax({
                url: "/umbraco/surface/Notification/GetNotNotifiedCount",
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

        function initCustomControls() {
            var notifications = Array.from($('.notifications__list-item'));
            notifications.forEach(function (notification) {
                var elem = $(notification);
                var id = elem.data("id");
                notification.addEventListener('click',
                function () {
                    var delivered = elem.data("viewed");
                    if (!delivered) {
                        $.ajax({
                            url: "/umbraco/surface/Notification/View/" + id,
                            success: function () {
                                elem.attr("data-viewed", 'true');
                            }

                        });
                    }
                });
            });
        }
        updateNotificationsCount();

        var holder = $('#header');
        if (!holder.length) {
            return;
        }
        setInterval(updateNotificationsCount, 3000);

        initCustomControls();
    });
})();