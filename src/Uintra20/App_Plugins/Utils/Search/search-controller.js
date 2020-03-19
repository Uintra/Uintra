angular
    .module('umbraco')
    .controller('searchController',
        ['$scope', 'searchService',
            function ($scope, searchService) {

                $scope.inProgress = false;

                $scope.rebuild = function () {
                    $scope.inProgress = true;
                    searchService.rebuildIndex()
                        .then(
                            function (resolve) {
                                alert('Ok');
                            },
                            function (reject) {
                                alert('No Ok');
                            })
                        .finally(
                            function () {
                                $scope.inProgress = false;
                            });
                };
            }
        ]
    );