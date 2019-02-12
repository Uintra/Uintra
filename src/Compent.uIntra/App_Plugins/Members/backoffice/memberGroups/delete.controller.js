angular.module('umbraco').controller('memberGroups.deleteController', function ($scope, $http, $routeParams, $location, navigationService) {

    $scope.performDelete = function () {
        $http.post('/umbraco/backoffice/api/MemberGroup/Delete', { id: parseInt($scope.entityId) })
            .success(function (response) {
                navigationService.hideDialog();
                navigationService.reloadNode($scope.dialogOptions.currentNode.parent());
                $location.url("/" + $routeParams.section);
            });
    };

    $scope.cancel = function () {
        navigationService.hideDialog();
    };
});