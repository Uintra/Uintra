(function () {
    'use strict';

    function dashboardController($scope, $timeout, navigationService, notificationsService) {

        var vm = this;

        vm.page = {
            title: 'Umbraco Structure Management',
            description: '',
            navigation: [
                {
                    'name': 'Manage',
                    'alias': 'manage',
                    'icon': 'icon-handtool',
                    'view': Umbraco.Sys.ServerVariables.umbracoSettings.appPluginsPath + '/UmbracoStructureManagement/settings/default.html',
                    'active': true
                },
                //{
                //    'name': 'Settings',
                //    'alias': 'settings',
                //    'icon': 'icon-settings',
                //    'view': Umbraco.Sys.ServerVariables.umbracoSettings.appPluginsPath + '/uSync8/settings/settings.html'
                //},
                //{
                //    'name': 'Add ons',
                //    'alias': 'expansion',
                //    'icon': 'icon-box',
                //    'view': Umbraco.Sys.ServerVariables.umbracoSettings.appPluginsPath + '/usync8/settings/expansion.html'
                //} 
            ]
        };

        $timeout(function () {
            navigationService.syncTree({ tree: "umbracoStructureManagement", path: "-1" });
        });
    }

    angular.module('umbraco')
        .controller('umbracoStructureManagementDashboardController', dashboardController);
})();