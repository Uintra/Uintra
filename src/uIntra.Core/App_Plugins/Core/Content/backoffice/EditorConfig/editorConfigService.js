(function () {
    'use strict';

    var editorConfigService = function ($http) {
        var self = this;
        var baseUrl = "/umbraco/backoffice/api/EditorConfig/Config";

        self.getConfig = function (editorAlias) {
            return $http.get(baseUrl + '?editorAlias=' + editorAlias);
        }
    }

    editorConfigService.$inject = ["$http"];
    angular.module('umbraco').service('editorConfigService', editorConfigService);
})();