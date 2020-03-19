angular
    .module('umbraco')
    .controller('searchController', 
        ['$scope', 
            function ($scope) {
                $scope.rebuild = function() {
                    console.log(321);
                };
            }
        ]
    );