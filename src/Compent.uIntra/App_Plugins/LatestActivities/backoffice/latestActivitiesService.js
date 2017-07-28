(function (angular) {
    'use strict';
    var service = function (centralFeedService, $q) {
        var self = this;

        self.getActivityTypes = function () {
            var deferred = $q.defer();

            centralFeedService.getActivityTypes().then(function (data) {
                deferred.resolve([]);
            }, function (err) {
                deferred.reject(err);
            });

            return deferred.promise;
        }
    }

    service.$inject = ["centralFeedService", "$q"];
    angular.module('umbraco').service('latestActivitiesService', service);
})(angular);