(function (angular) {
    'use strict';

    angular.module('umbraco')
        .constant("notificationSettingsConfig", {
            uiMessageTinyMceOptions: {
                toolbar: "bold | italic | removeformat"
            }
        });

})(angular);