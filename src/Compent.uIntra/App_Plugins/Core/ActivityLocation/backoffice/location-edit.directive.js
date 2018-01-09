(function () {
    'use strict';

    var factory = function (assetsService) {
        function init(scope, elem, attrs) {
            var find = function (query) { return getRawDomElement(elem, query) };
            var findModelHolder = function (name) { return find('[ng-model="' + name + '"]') }

            var addressHolder = findModelHolder('address');
            var shortAddressHolder = findModelHolder('shortAddress');
            var mapContainer = find('#js-map-container');

            initActivityLocationEdit(addressHolder, shortAddressHolder, mapContainer);
        }

        function getRawDomElement(elem, query) { return elem.find(query)[0]; }

        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/Core/ActivityLocation/backoffice/location-edit.html',
            scope: {
                model: '=',
                configModel: '=config',
                mode: '@'
            },
            link: function(scope, elem, attrs) {
                assetsService.load(['/App_Plugins/Core/ActivityLocation/activityLocationEdit.js'])
                    .then(function () {init(scope, elem, attrs)});
            }
        };
    }
   
    factory.$inject = ['assetsService'];

    angular.module('umbraco').directive('locationEdit', factory);
})();


