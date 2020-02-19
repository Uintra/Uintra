angular.module('umbraco').controller('memberGroups.deleteController', function ($scope, memberGroupsService, $http, $routeParams, $location, navigationService) {


    $scope.performDelete = function () {
        memberGroupsService.delete($scope.dialogOptions.currentNode.id)
            .then(function (response) {
                navigationService.hideDialog();
                navigationService.reloadNode($scope.dialogOptions.currentNode.parent());
                $location.url("/" + $routeParams.section);
            });
    };

    $scope.cancel = function () {
        navigationService.hideDialog();
    };
});