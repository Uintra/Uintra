(function (angular) {
    'use strict';

    var service = function ($http) {
        var self = this;

        const baseUrl = '/umbraco/backoffice/api/NotificationSettingsApi/';

        self.getSettings = function (activityType, notificationType) {
            return $http.get(baseUrl + 'Get?activityType=' + activityType + '&notificationType=' + notificationType);
        }

        self.seveEmailSettings = function (emailSettings) {
            return $http.post(baseUrl + 'SaveEmailNotifierSetting', emailSettings);
        }

        self.sevePopupSettings = function (popupSettings) {
            return $http.post(baseUrl + 'SavePopupNotifierSetting', popupSettings);
        }

        self.seveUiSettings = function (uiSettings) {
            return $http.post(baseUrl + 'SaveUiNotifierSetting', uiSettings);
        }
    }

    service.$inject = ['$http'];
    angular.module('umbraco').service('notificationSettingsService', service);
})(angular);