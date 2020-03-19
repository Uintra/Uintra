angular
    .module('umbraco')
    .controller('searchController',
        ['$scope', 'searchService',
            function ($scope, searchService) {

                $scope.rebuild = function () {
                    searchService.rebuildIndex()
                        .then(
                            function (resolve) {
                                alert('Ok');
                            },
                            function (reject) {
                                alert('No Ok');
                            });
                };
            }
        ]
    );