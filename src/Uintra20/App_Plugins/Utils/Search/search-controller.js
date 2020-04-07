angular
    .module('umbraco')
    .controller('searchController',
        ['$scope','$http',
            function ($scope,$http) {

                $scope.inProgress = false;
                $scope.showResult = false;
                $scope.class = '';
                $scope.data = '';

                $scope.rebuild = function () {
                    $scope.inProgress = true;
                    $http.post('/ubaseline/api/search/rebuildIndex')
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