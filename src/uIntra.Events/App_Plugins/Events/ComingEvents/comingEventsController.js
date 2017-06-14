(function () {

    var controller = function ($scope) {

        var defaultEventsAmount = 5;
        var defaultDisplayTitle = "Coming Events";
        $scope.overlay = {
            show: false,
            view: "/App_Plugins/Events/ComingEvents/coming-events-overlay.html",
            title: "Coming events",
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
        }

        $scope.open = function () {
            $scope.overlay.show = true;
            $scope.backupModel = angular.copy($scope.control.value);
        }



        $scope.init = function (control) {
            if (!$scope.control.value) {
                $scope.control.value = getDefaultModel();
            }
            $scope.control = control;
        }

        function getDefaultModel() {
            return {
                eventsAmount: defaultEventsAmount,
                displayTitle: defaultDisplayTitle
            }
        }

        function isValidModel(model) {
            if (!model.displayTitle || model.displayTitle.length < 0) {
                $scope.control.validationMessage = "Title is required";
                return false;
            }
            if (model.eventsAmount <= 0) {
                $scope.control.validationMessage = "Amount must be bigger than 0";
                return false;
            }
            return true;
        }
    }
    controller.$inject = ["$scope"];
    angular.module('umbraco').controller('comingEventsController', controller);
})();