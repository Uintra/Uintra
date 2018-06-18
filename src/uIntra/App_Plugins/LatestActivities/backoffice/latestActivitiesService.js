(function (angular) {
    'use strict';

    function findIndex(array, predicate) {
        for (var i = 0; i < array.length; i++)
            if (predicate(array[i])) return i;
        return -1;
    };

    function moveElementToTheEnd(arr, i) {
        if (arr.length < 2) return arr;
        var el = arr[i];
        for (var j = i; j < arr.length - 1; j++)
            arr[j] = arr[j + 1];
        arr[j] = el;
        return arr;
    };

    var service = function (centralFeedService, $q) {
        var self = this;

        self.getActivityTypes = function () {
            var deferred = $q.defer();

            centralFeedService.getActivityTypes().then(function (response) {
                var result = moveAllTabTypeToEndIfExists(response.data);
                deferred.resolve(result);
            }, function (err) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        function moveAllTabTypeToEndIfExists(data) {
            var i = findIndex(data, function (el) {
                return el.Name == "All";
            });

            return i > -1 ? moveElementToTheEnd(data, i) : data;
        };
    }

    service.$inject = ["centralFeedService", "$q"];
    angular.module('umbraco').service('latestActivitiesService', service);
})(angular);