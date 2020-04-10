(function () {
    var onError = function(error) { console.error(error); };

    var controller = function (
        $scope, 
        $routeParams, 
        contentResource, 
        usersTagsService) {
        
        const self = this;
        usersTagsService.getAll($routeParams.id).then(function (response) {
            self.usersTags = response.data;
        },onError);
        
    };

    controller.$inject = ["$scope", "$routeParams", "contentResource", "usersTagsService"];
    angular.module('umbraco').controller('usersTagsController', controller);
})();