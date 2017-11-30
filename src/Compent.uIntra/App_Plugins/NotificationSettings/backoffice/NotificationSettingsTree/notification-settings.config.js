(function (angular) {
    'use strict';

    angular.module('umbraco')
        .constant("notificationSettingsConfig", {
            uiMessageTinyMceOptions: {
                toolbar: "bold | italic | removeformat"
            },
            emailMessageTinyMceOptions: {
                toolbar: "formatselect | bold | italic | alignleft | aligncenter | alignright | removeformat"
            }
        });

})(angular);