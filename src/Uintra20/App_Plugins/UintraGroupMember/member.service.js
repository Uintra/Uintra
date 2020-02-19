(function (angular) {
    'use strict';

    var service = function ($http) {
        var self = this;
        var baseUrl = "/umbraco/backoffice/api/MemberApi/";

        self.memberChanged = function(memberId) {
            return $http.post(baseUrl + "MemberChanged?memberId=" + memberId);
        }
    }

    service.$inject = ["$http"];
    angular.module('umbraco').service('memberService', service);
})(angular);