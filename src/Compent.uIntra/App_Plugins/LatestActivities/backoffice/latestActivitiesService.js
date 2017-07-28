(function (angular) {
    'use strict';

     function findIndex (array, predicate) {
         for (var i = 0; i < array.length; i++)
             if (predicate(array[i])) return i;
        return -1;
    };

    function moveElementToTheEnd(data, i) {
        var el = data[i];
        data.splice(i, 1);
        data.push(el);
        return data;
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