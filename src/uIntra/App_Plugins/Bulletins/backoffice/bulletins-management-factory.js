angular.module('umbraco.resources').factory('bulletinsManagementFactory', ['$http', function ($http) {

    var baseUrl = '/Umbraco/backoffice/Api/BulletinsSection/';
    var factory = {};

    factory.getAll = function () {
        return $http.get(baseUrl + 'GetAll');
    }

    factory.create = function (bulletin) {
        return $http.post(baseUrl + 'Create', bulletin);
    }

    factory.save = function (bulletin) {
        return $http.post(baseUrl + 'Save', bulletin);
    }

    factory.delete = function (bulletinId) {
        return $http.delete(baseUrl + 'Delete?id=' + bulletinId);
    }

    return factory;
}
]);