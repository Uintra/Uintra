(function (angular) {
    'use strict';

    var service = function($http) {
        var self = this;

        const baseUrl = '/umbraco/backoffice/api/NotificationSettingsApi/';

        self.getSettings = function(activityType, notificationType) {
            return $http.get(baseUrl + 'Get?activityType=' + activityType + '&notificationType=' + notificationType);
        };

        self.saveEmailSettings = function(emailSettings) {
            return $http.post(baseUrl + 'SaveEmailNotifierSetting', emailSettings);
        };

        self.savePopupSettings = function(popupSettings) {
            return $http.post(baseUrl + 'SavePopupNotifierSetting', popupSettings);
        };

        self.saveUiSettings = function(uiSettings) {
            return $http.post(baseUrl + 'SaveUiNotifierSetting', uiSettings);
        };

        self.saveDesktopSettings = function(desktopSettings) {
            return $http.post(baseUrl + 'SaveDesktopNotifierSetting', desktopSettings);
        };
    };

    service.$inject = ['$http'];
    angular.module('umbraco').service('notificationSettingsService', service);
})(angular);