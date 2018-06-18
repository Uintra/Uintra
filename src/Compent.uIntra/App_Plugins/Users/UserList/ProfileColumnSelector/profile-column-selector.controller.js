(function () {
    var profileColumnSelectorController = function ($scope, $http) {
        $scope.init = function (control) {
            console.log("Profile column selector initialize!");
        }
    }
    angular.module('umbraco').controller('profileColumnSelectorController', ['$scope', '$http', profileColumnSelectorController]);
})();