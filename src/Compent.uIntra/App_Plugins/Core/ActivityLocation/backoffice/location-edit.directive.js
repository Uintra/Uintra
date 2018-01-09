(function () {
    'use strict';

    var factory = function (assetsService) {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/Core/ActivityLocation/backoffice/location-edit.html',
            scope: {
                model: '=',
                configModel: '=config',
                mode: '@'
            },
            link: function () {}
        };
    }

    //factory.$inject = ['assetsService'];

    angular.module('umbraco').directive('locationEdit', factory);
})();


