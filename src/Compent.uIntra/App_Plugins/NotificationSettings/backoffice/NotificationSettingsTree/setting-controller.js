(function (angular) {
    'use strict';

    var controller = function ($scope, appState, notificationsService, notificationSettingsConfig, notificationSettingsService) {
        var self = this;
        self.settings = {};
        self.selectedNotifierSettings = {};

        const notifierType = {
            email: 1,
            ui: 2
        };

        let selectedNotifierType;


        self.isEmailTabSelected = function () {
            return selectedNotifierType === notifierType.email;
        }

        self.isUiTabSelected = function () {
            return selectedNotifierType === notifierType.ui;
        }

        self.selectEmailTab = function () {
            selectedNotifierType = notifierType.email;
            self.selectedNotifierSettings = self.settings.emailNotifierSetting;
        }

        self.selectUiTab = function () {
            selectedNotifierType = notifierType.ui;
            self.selectedNotifierSettings = self.settings.uiNotifierSetting;
        }

        self.save = function () {
            if ($scope.settingsForm.$pristine || $scope.settingsForm.$invalid) {
                return;
            }

            saveSettings(self.settings);
        }

        self.isControlTextHasValidLength = function (control, maxLength) {
            const trimmedText = trimHtml(control.$modelValue);
            const isValidLength = trimmedText.length <= maxLength;

            control.$setValidity("maxLength", isValidLength);

            return isValidLength;
        }

        function initalize() {
            var selectedNode = appState.getTreeState('selectedNode');

            if (selectedNode) {
                var notificationType = selectedNode.id;
                var activityType = selectedNode.parentId;

                notificationSettingsService.getSettings(activityType, notificationType).then(function (result) {
                    self.settings = result.data;
                    self.selectEmailTab();
                }, showGetErrorMessage);
            }

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

    controller.$inject = ['$scope', 'appState', 'notificationsService', 'notificationSettingsConfig', 'notificationSettingsService'];

    angular.module('umbraco').controller('settingController', controller);
})(angular);