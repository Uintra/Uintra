angular.module('umbraco').controller('memberGroups.deleteController', function ($scope, memberGroupsService, $http, $routeParams, $location, navigationService, treeService) {


    $scope.performDelete = function () {
        memberGroupsService.delete($routeParams.id)
            .then(function (response) {
                navigationService.hideDialog();
                treeService.removeNode($scope.currentNode)
                navigationService.syncTree({
                    tree: $routeParams.tree, path: ['-1'],
                    forceReload: true,
                    activate: true
                });
                $location.url("/" + $routeParams.section);
            });
    };

    $scope.cancel = function () {
        navigationService.hideDialog();
    };
});