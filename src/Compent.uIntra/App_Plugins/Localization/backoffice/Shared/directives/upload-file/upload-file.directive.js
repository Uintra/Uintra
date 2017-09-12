(function (angular) {
    'use strict';

    var uploadFileDirective = function ($timeout, notificationsService) {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/Localization/backoffice/Shared/directives/upload-file/upload-file.directive.html',
            scope: {
                uploadButtonText: "@",
                onLoaded: "="
            },
            link: function (scope, element, attrs) {
                var reader = new FileReader();
                reader.onload = function (loadEvent) {
                    var data = JSON.parse(loadEvent.target.result);

                    $timeout(function () {
                        scope.onLoaded(data);
                    }, 0);
                };

                reader.onerror = function (errorEvent) {
                    notificationsService.error("Error", "Some unhandled error has occurred");
                };

                scope.upload = function () {
                    var file = element[0].querySelector(".js-import-file");
                    if (file.files.length > 0) {
                        var singleFile = file.files[0];

                        if (singleFile.name.indexOf(".json") < 0) {
                            alert("Supported only json files");
                            return;
                        }
                        reader.readAsText(singleFile, "UTF-16");
                    }
                }
            }
        };
    }

    uploadFileDirective.$inject = ["$timeout", "notificationsService"];

    angular.module("umbraco").directive('uploadFile', uploadFileDirective);
})(angular);