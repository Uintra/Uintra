angular
    .module('umbraco')
    .controller('searchController',
        ['$scope', 'searchService',
            function ($scope, searchService) {

                $scope.inProgress = false;
                $scope.showResult = false;
                $scope.class = '';

                $scope.rebuild = function () {
                    $scope.inProgress = true;
                    searchService.rebuildIndex()
                        .then(
                            function (resolve) {
                                $scope.class = 'alert-success';
                            },
                            function (reject) {
                                $scope.class = 'alert-dander';
                            })
                        .finally(
                            function () {
                                $scope.showResult = true;
                                $scope.inProgress = false;
                            });
                };

                $scope.hideAlert = function () {
                    $scope.showResult = false;
                };
            }
        ]
    );