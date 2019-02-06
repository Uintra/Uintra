angular.module('umbraco').controller('memberGroups.deleteController', function ($scope, $http, notificationsService, dialogService, navigationService) {
    //console.log($scope.dialogOptions);
    $scope.entityId = $scope.dialogOptions.currentNode.id;
    $scope.performDelete = function () {
        //console.log("Delete");
        //$scope.nav.hideMenu();
        //console.log($scope.dialogOptions);
        //console.log($scope.nav);
        //console.log(dialogService);
            console.log($scope.dialogOptions.currentNode.parent());
        //$scope.nav.reloadNode($scope.dialogOptions.currentNode.parent());
    };
    $scope.cancel = function () {
        //console.log("cancel");
        //$scope.nav.hideDialog();
        navigationService.hideDialog();
    };

});