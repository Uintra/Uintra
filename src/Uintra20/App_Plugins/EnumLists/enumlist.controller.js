angular.module("umbraco").controller("EnumList",
    function ($scope, enumResource) {

        enumResource.getMediaFolderTypeEnum().then(function (response) {
            $scope.enumItems = response.data;
            if ($scope.model.value === "") {
                var i = parseInt($scope.model.config.defaultValueIndex);
                if (!isNaN(i)) {
                    $scope.model.value = response.data[i];
                }
            }
        });
        
    });
