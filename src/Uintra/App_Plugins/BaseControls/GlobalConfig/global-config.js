(function (angular) {
    var defaults = {
        name: 'Global Config',
        filter: { 'alias': 'custom.GlobalConfig' }
    };

    var localCache = {
        get: function (name, promise) { return this[name] || (this[name] = promise()); }
    };

    var factory = function ($filter, $parse, dataTypeResource) {
        var link = function ($scope, $element, $attrs) {
            var config = angular.extend({}, defaults, $scope.$eval($attrs.gcConfig));
            var getById = function (id) {
                var mapPrevalues = function (data) {
                    var result = {};
                    if (!data) return result;
                    angular.forEach(data.preValues, function (field) {
                        result[field.key] = field.value;
                    });
                    return result;
                };
                return function () {
                    return dataTypeResource.getById(id).then(mapPrevalues);
                };
            };

            var datatypeGetter = function () {
                var mapToObject = function (data) {
                    var result = {};
                    angular.forEach(data.preValues, function (panel) {
                        angular.forEach(panel.value, function (id, name) {
                            this[name] = localCache.get(id, getById(id));
                        }, (result[panel.key] = {}));
                    });
                    return result;
                };

                return dataTypeResource.getByName(config.name).then(mapToObject);
            };

            $parse($attrs.globalConfig).assign($scope, localCache.get(config.name, datatypeGetter));

            if ($attrs.gcAll) {
                var getAllPropertyEditors = function () {
                    var filterPromise = function (data) { return $filter('filter')(data, config.filter); }
                    return dataTypeResource.getAllPropertyEditors().then(filterPromise);
                };
                $parse($attrs.gcAll).assign($scope, localCache.get('getAllPropertyEditors', getAllPropertyEditors));
            }
        };
        return {
            restrict: 'A',
            link: link
        };
    };

    factory.$inject = ['$filter', '$parse', 'dataTypeResource'];

    angular.module('umbraco').directive('globalConfig', factory);
})(angular);
