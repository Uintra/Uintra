(function () {

    var controller = function ($scope, $http) {

        var defaultDisplayedAmount = 10;
        var defaultTitle = "User list";
        var defaultAmountPerRequest = 10;
        $scope.overlay = {
            show: false,
            view: "/App_Plugins/Users/UserList/backoffice/user-list-overlay.html",
            title: "User list",
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

        $scope.init = function (control) {
            if (!$scope.control.value) {
                $scope.control.value = getDefaultModel();
            }
            $scope.control = control;
        };

        function getDefaultModel() {
            return {
                displayedAmount: defaultDisplayedAmount,
                title: defaultTitle,
                amountPerRequest: defaultAmountPerRequest
            };
        }

        function isValidModel(model) {
            if (!model.title || model.title.length < 0) {
                $scope.control.validationMessage = "Title is required";
                return false;
            }
            if (model.displayedAmount <= 0) {
                $scope.control.validationMessage = "Displayed amount must be bigger than 0";
                return false;
            }
            if (model.amountPerRequest <= 0) {
                $scope.control.validationMessage = "Amount per request must be bigger than 0";
                return false;
            }
            return true;
        }

    };
    controller.$inject = ["$scope", "$http"];
    angular.module('umbraco').controller('userListController', controller);
})();