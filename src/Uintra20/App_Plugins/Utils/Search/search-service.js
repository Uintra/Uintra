angular
    .module('umbraco')
    .factory('searchService',
        ['$http',
            function ($http) {
                return {
                    rebuildIndex: function () {
                        return $http.post('/ubaseline/api/search/rebuildIndex');
                    }
                };
            }
        ]
    );