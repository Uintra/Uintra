(function () {

    var defaultActivityAmount = 5;
    var defaultTitle = "Latest activities";

    let controller = function ($scope, latestActivitiesService) {
        
        $scope.availableActivityTypes = [];

        $scope.init = function () {
            latestActivitiesService.getActivityTypes().then(function (data) {
                $scope.availableActivityTypes = data;
            });

            if (!$scope.control.value) {
                $scope.control.value = getDefaultModel();
            };
        };

        function getDefaultModel() {
            return {
                activityAmount: defaultActivityAmount,
                title: defaultTitle
            }
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

            if (!isValidActivityAmount(model)) {
                $scope.control.validationMessage = "Number of activities is invalid";
                return false;
            }

            return true;

            function isValidActivityAmount(model) {
                return !isNaN(model.activityAmount) && parseInt(model.activityAmount) > 0;
            }

            function isValidTitle(model) {
                return model.title && model.title.length > 0;
            }
        };

    };
    controller.$inject = ["$scope", "latestActivitiesService"];
    angular.module('umbraco').controller('latestActivititesController', controller);
})();