(function () {

    let controller = function ($scope, centralFeedService) {


        $scope.availableActivityTypes = [];

        $scope.init = function () {
            centralFeedService.getActivityTypes().success(function (data) {
                $scope.availableActivityTypes = data;
            });
        };

        $scope.overlay = {
            show: false,
            title: "Latest activities",
            view: "/App_Plugins/LatestActivities/backoffice/latest-activities.overlay.html",
            close: function () {
                $scope.overlay.show = false;
                $scope.control.value = $scope.backupModel;
            },
            submit: function () {
                if (isValidModel($scope.control.value)) {
                    $scope.overlay.show = false;
                    $scope.control.validationMessage = null;
                }
            }
        };

        $scope.open = function () {
            $scope.overlay.show = true;
            $scope.backupModel = angular.copy($scope.control.value);
        };

        function isValidModel(model) {

            if (!model) {
                $scope.control.validationMessage = "Fields can not be empty";
                return false;
            }

            if (!isValidTitle(model)) {
                $scope.control.validationMessage = "Title is required";
                return false;
            }

            if (!isValidNumberOfActivities(model)) {
                $scope.control.validationMessage = "Number of activities is invalid";
                return false;
            }

            return true;

            function isValidNumberOfActivities(model) {
                return !isNaN(model.nubmerOfActivities) && parseInt(model.nubmerOfActivities) > 0; 
            }

            function isValidTitle(model) {
                return model.displayTitle && model.displayTitle.length > 0;
            }
        }

    };
    controller.$inject = ["$scope", "centralFeedService"];
    angular.module('umbraco').controller('latestActivititesController', controller);
})();