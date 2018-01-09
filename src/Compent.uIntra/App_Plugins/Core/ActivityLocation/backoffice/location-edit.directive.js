(function () {
    'use strict';


    var factory = function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/Core/ActivityLocation/backoffice/location-edit.html',
            scope: {
                model: '=',
                configModel: '=config',
                mode: '@'
            },
            link: function ($scope) {
                //$scope.config = angular.extend({}, defaultConfig, $scope.configModel);
                //$scope.isSingleMode = isSingleMode($scope.mode);

                //var internalPicker = internalPickerFactory($q, dialogService, contentResource, entityResource, $scope.config.internalPicker);
                //var mediaPicker = mediaPickerFactory($q, dialogService);
                //init($scope, internalPicker, mediaPicker);
            }
        };
    }

    //factory.$inject = ['$q', 'dialogService', 'contentResource', 'entityResource'];

    angular.module('umbraco').directive('locationEdit', factory);
})();


