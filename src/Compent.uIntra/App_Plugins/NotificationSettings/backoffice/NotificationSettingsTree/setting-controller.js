(function (angular) {
    'use strict';

    var controller = function ($scope, $location, appState, notificationsService, notificationSettingsConfig, notificationSettingsService) {
        var self = this;
        self.settings = {};

        const notifierType = {
            email: 1,
            ui: 2
        };

        let selectedNotifierType = notifierType.email;


        self.isEmailTabSelected = function () {
            return selectedNotifierType === notifierType.email;
        }

        self.isUiTabSelected = function () {
            return selectedNotifierType === notifierType.ui;
        }

        self.selectEmailTab = function () {
            selectedNotifierType = notifierType.email;
        }

        self.selectUiTab = function () {
            selectedNotifierType = notifierType.ui;
        }

        self.save = function () {
            saveSettings(self.settings);
        }

        self.isControlTextHasValidLength = function (control, maxLength) {
            const trimmedText = trimHtml(control.$modelValue);
            const isValidLength = trimmedText.length <= maxLength;

            control.$setValidity("maxLength", isValidLength);

            return isValidLength;
        }

        var getUrlParams = function (url) {
            var params = {};
            (url + '?').split('?')[1].split('&').forEach(function (pair) {
                pair = (pair + '=').split('=').map(decodeURIComponent);
                if (pair[0].length) {
                    params[pair[0]] = pair[1];
                }
            });
            return params;
        };

        function initalize() {
            var params = getUrlParams($location.path());
                notificationSettingsService.getSettings(params.activityType, params.notificationType).then(function (result) {
                    self.settings = result.data;
                }, showGetErrorMessage);

            self.config = notificationSettingsConfig;
        }

        function saveSettings(settings) {
            if (self.isEmailTabSelected()) {
                notificationSettingsService.seveEmailSettings(settings.emailNotifierSetting).then(showSaveSuccessMessage, showSaveErrorMessage);
            }
            else if (self.isUiTabSelected()) {
                notificationSettingsService.seveUiSettings(settings.uiNotifierSetting).then(showSaveSuccessMessage, showSaveErrorMessage);
            }
        }

        function showGetErrorMessage() {
            notificationsService.error("Error", "Notification settings were not loaded, because some error has occurred");
        }

        function showSaveSuccessMessage() {
            notificationsService.success("Success", "Notification settings were updated successfully");
        }

        function showSaveErrorMessage() {
            notificationsService.error("Error", "Notification settings were not updated, because some error has occurred");
        }

        function trimHtml(text) {
            return text ? String(text).replace(/<[^>]+>/gm, '') : '';
        }

        initalize();
    }

    controller.$inject = ['$scope','$location',  'appState', 'notificationsService', 'notificationSettingsConfig', 'notificationSettingsService'];

    angular.module('umbraco').controller('settingController', controller);
})(angular);