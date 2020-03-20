angular
    .module('umbraco')
    .controller('searchController',
        ['$scope', 'searchService',
            function ($scope, searchService) {

                $scope.inProgress = false;
                $scope.showResult = false;
                $scope.class = '';
                $scope.data = '';

                $scope.rebuild = function () {
                    $scope.inProgress = true;
                    searchService.rebuildIndex()
                        .then(
                            function (resolve) {
                                $scope.class = 'alert-success';
                                $scope.data = Object.values(resolve.data.index);
                            },
                            function (reject) {
                                $scope.class = 'alert-dander';
                                $scope.data = Object.values(reject.data);
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