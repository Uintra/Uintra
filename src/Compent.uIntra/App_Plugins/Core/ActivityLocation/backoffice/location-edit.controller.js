(function(angular) {

    var controller = function (assetsService) {

        assetsService.load(['/App_Plugins/Core/ActivityLocation/activityLocationEdit.js'])
            .then(init);

        function init() {
            debugger;
            initActivityLocationEdit();
        }
    }

    controller.$inject = ['assetsService'];
    angular.module('umbraco').controller('locationEditController', controller);
})(angular);
