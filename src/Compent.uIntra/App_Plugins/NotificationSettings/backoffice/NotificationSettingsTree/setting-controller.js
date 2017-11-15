(function(angular) {
    'use strict';

    var controller = function($scope, $http, appState) {
        var self = this;
        $scope.content = {
            tabs:
            [
                { id: 1, label: "Mail" },
                { id: 2, label: "Ui" }
            ]
        };

        $scope.Save = function() {
            saveSettings($scope.settings);
        }

        function initalize()
        {
            var selectedNode = appState.getTreeState("selectedNode");

            if (selectedNode) {
                var notificationType = selectedNode.id;
                var activityType = selectedNode.parentId;

                getSettings(activityType, notificationType).then(function (result) {
                    $scope.settings = result.data;
                });
            }

        }

        function getSettings(activityType, notificationType) {
            return $http.get('/umbraco/backoffice/api/NotificationSettingsApi/Get?activityType=' +
                activityType +
                '&notificationType=' +
                notificationType);
        }

        function saveSettings(settings) {
            $http.post('/umbraco/backoffice/api/NotificationSettingsApi/Save', settings);
        }

        initalize();
    }

    controller.$inject = ["$scope", "$http", "appState"];

    angular.module('umbraco').controller('settingController', controller);
})(angular);