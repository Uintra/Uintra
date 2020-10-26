(function () {
    'use strict';

    var replaceFilterFactory = function () {
        return function (str, what, to) {
            return str ? str.replace(what, to) : str;
        }
    }

    var interpolationFilterFactory = function () {
        return function (str, model) {
            if (!angular.isString(str)) {
                throw new Error('Interpolation Filter: First parameter should be string');
            }
            if (!angular.isDefined(model)) {
                throw new Error('Interpolation Filter: Second parameter should be object');
            }
            var result = str.replace(/{([^{}]*)}/g, function (a, b) {
                var r = model[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            });
            return result;
        }
    }

    var stripHtmlFilterFactory = function () {
        return function (str) {
            var el = angular.element('<div>' + str + '</div>');
            return el.text();
        }
    }

    var orderByObjectFilterFactory = function () {
        return function (items, field, reverse) {
            var filtered = [];
            angular.forEach(items, function (item, key) {
                item.$key = key;
                filtered.push(item);
            });
            filtered.sort(function (a, b) {
                return (a[field] > b[field] ? 1 : -1);
            });
            if (reverse) filtered.reverse();
            return filtered;
        };
    }

    angular.module('umbraco').filter('interpolate', interpolationFilterFactory);
    angular.module('umbraco').filter('stipHtml', stripHtmlFilterFactory);
    angular.module('umbraco').filter('orderByObject', orderByObjectFilterFactory);
    angular.module('umbraco').filter('replace', replaceFilterFactory);
})();