(function (angular) {
    'use strict';

    var controller = function ($scope, $http, appState, notificationSettingsConfig) {
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

                getSettings(activityType, notificationType).then(function (result) {
                    self.settings = result.data;
                });
            }

            self.config = notificationSettingsConfig;
        }

        function getSettings(activityType, notificationType) {
            return $http.get('/umbraco/backoffice/api/NotificationSettingsApi/Get?activityType=' +
                activityType +
                '&notificationType=' +
                notificationType);
        }

        function saveSettings(settings) {
            if (self.isUiTabSelected()) {
                $http.post('/umbraco/backoffice/api/NotificationSettingsApi/SaveUiNotifierSetting', settings.uiNotifierSetting);
                return;
            }

            $http.post('/umbraco/backoffice/api/NotificationSettingsApi/SaveEmailNotifierSetting', settings.emailNotifierSetting);
        }

        initalize();
    }

    controller.$inject = ["$scope", "$http", "appState", "notificationSettingsConfig", "$routeParams"];

    angular.module('umbraco').controller('settingController', controller);
})(angular);