﻿(function (angular) {
    'use strict';

    var service = function ($http) {
        var self = this;

        const baseUrl = '/umbraco/backoffice/api/MemberGroup/';
        const permissionBaseUrl = '/umbraco/backoffice/api/permissions/';

        //self.get = function (memberGroupId) {
        //    return $http.get(baseUrl + 'Get?id=' + memberGroupId);
        //};

        self.create = function (memberGroupName) {
            return $http.post(permissionBaseUrl + 'Create', { name: memberGroupName });
        };

        self.save = function (memberGroupId, memberGroupName) {
            return $http.post(baseUrl + 'Save', { id: memberGroupId, name: memberGroupName });
        };

        self.delete = function (memberGroupId) {
            return $http.delete(permissionBaseUrl + 'Delete?groupId=' + memberGroupId);
        };

        self.getPermissions = function(memberGroupId) {
            return $http.get(permissionBaseUrl + 'get?memberGroupId=' + memberGroupId);
        };

        self.toggle = function (permission) {
            return $http.post(permissionBaseUrl + 'Save', permission);
        };

        //self.isSuperUser = function () {
        //    return $http.get(baseUrl + 'IsSuperUser');
        //};

        //self.toggleEnabled = function (permission) {
        //
        //};
        
        //self.toggleAllowed = function (permission) {
        //
        //};
    };

    service.$inject = ['$http'];
    angular.module('umbraco').service('memberGroupsService', service);
})(angular);
