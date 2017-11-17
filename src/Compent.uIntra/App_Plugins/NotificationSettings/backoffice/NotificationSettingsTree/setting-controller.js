(function (angular) {
    'use strict';

    var controller = function ($scope, $http, appState, notificationSettingsConfig, notificationSettingsService) {
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

        function initalize() {
            var selectedNode = appState.getTreeState("selectedNode");

            if (selectedNode) {
                var notificationType = selectedNode.id;
                var activityType = selectedNode.parentId;

                notificationSettingsService.getSettings(activityType, notificationType).then(function (result) {
                    self.settings = result.data;
                });
            }

            self.config = notificationSettingsConfig;
        }

        function saveSettings(settings) {
            if (self.isEmailTabSelected()) {
                notificationSettingsService.seveEmailSettings(settings.emailNotifierSetting);
            }
            else if (self.isUiTabSelected()) {
                notificationSettingsService.seveEmailSettings(settings.uiNotifierSetting);
            }
        }

        initalize();
    }

    controller.$inject = ["$scope", "$http", "appState", "notificationSettingsConfig", "notificationSettingsService"];

    angular.module('umbraco').controller('settingController', controller);
})(angular);