(function (angular) {
    var factory = function () {
        var link = function ($scope) {
            $scope.ngModel = $scope.ngModel || {
                imageTypeAlias: "Image",
                mediaFolderId: null,
                crops: [],
                focalPoint: {
                    left: 0.5,
                    top: 0.5
                }
            };

            $scope.save = function () {
                if ($scope.newItem
                    && angular.isNumber($scope.newItem.width)
                    && angular.isNumber($scope.newItem.height)
                    && $scope.newItem.width > 0
                    && $scope.newItem.height > 0) {
                    $scope.hasError = false;
                    $scope.ngModel.crops.push({
                        width: $scope.newItem.width,
                        height: $scope.newItem.height
                    });
                    $scope.newItem = null;
                } else {
                    $scope.hasError = true;
                }
            };

            $scope.remove = function (index) {
                $scope.ngModel.crops.splice(index, 1);
            };
        };

        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/MultiImageCropper/multi-image-cropper-config.html',
            scope: { ngModel: '=' },
            link: link
        };
    }
    factory.$inject = [];
    angular.module('umbraco').directive('multiImageCropperConfig', factory);
})(angular);
