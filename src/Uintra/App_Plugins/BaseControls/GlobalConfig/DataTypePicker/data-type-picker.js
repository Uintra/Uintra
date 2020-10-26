(function (angular) {
    var localCache = {
        get: function (name, promise) { return this[name] || (this[name] = promise()); }
    };
    var factory = function ($filter, dataTypeResource) {
        var getByAlias = function (alias) {
            var filter = { 'alias': alias, trashed: false };
            var filterPromise = function (data) { return $filter('filter')(data, filter); }
            return function () {
                return localCache.get('allDataTypeResource', dataTypeResource.getAll).then(filterPromise);
            };
        };
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/GlobalConfig/DataTypePicker/data-type-picker.html',
            scope: {
                ngModel: '=',
                alias: '@'
            },
            link: function ($scope) {
                $scope.all = localCache.get($scope.alias, getByAlias($scope.alias));
            }
        };
    };

    factory.$inject = ['$filter', 'dataTypeResource'];

    angular.module('umbraco').directive('dataTypePicker', factory);
})(angular);
