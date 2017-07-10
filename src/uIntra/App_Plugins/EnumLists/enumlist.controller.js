angular.module("umbraco").controller("EnumList",
    function ($scope, enumResource) {

        enumResource.getAll($scope.model.config.assembly, $scope.model.config.enum).then(function (response) {
            $scope.enumItems = response.data;
        });
        
    });
