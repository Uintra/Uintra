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
                refresh();
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
            getAllowedProperties().then(function (result) {
                $scope.control.value.properties = result.data;
            });
        }

        $scope.sortableOptions = {
            axis: 'y',
            cursor: "move",
            handle: ".handle",
            update: function (ev, ui) { },
            stop: function (ev, ui) { }
        };

        $scope.handleClick = function (property) {
            property.selected = !property.selected;
            refresh();
        };

        $scope.isSelected = function (property) {
            var result = $scope.control.value.selectedProperties.find(i => i.id === property.id);
            if (result) property.selected = true;
            return result;
        };

        function refresh() {
            $scope.control.value.selectedProperties =
                $scope.control.value.properties.filter(i => i.selected);
        }

        function getDefaultModel() {
            return {
                displayedAmount: defaultDisplayedAmount,
                title: defaultTitle,
                amountPerRequest: defaultAmountPerRequest,
                properties: [],
                selectedProperties: [],
                orderBy: null
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
            if (model.orderBy == null) {
                $scope.control.validationMessage = "Order by column must be selected.";
                return false;
            }
            return true;
        }

        function getAllowedProperties() {
            return $http.get("/umbraco/backoffice/api/UserApi/ProfileProperties");
        }
    }
    controller.$inject = ["$scope", "$http"];
    angular.module('umbraco').controller('userListController', controller);
})();