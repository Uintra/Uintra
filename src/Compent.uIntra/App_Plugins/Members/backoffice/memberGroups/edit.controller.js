angular.module('umbraco').controller('memberGroups.editController',
    function ($scope, $routeParams, $http, notificationsService, dialogService, navigationService, treeService) {

        var vm = this;


        vm.memberGroup = null;
        if ($routeParams.create) {
            console.log("create");
            vm.isCreate = true;
            return;
        }
        vm.checked = true;
        var memberGroupId = $routeParams.id;
        vm.buttonState = "init";
        vm.property = {
            label: "Name",
            description: "Member group name"
        };

        $http.get('/umbraco/backoffice/api/MemberGroup/Get?id=' + memberGroupId).success(function (response) {
            console.log(response);
            vm.memberGroup = response;
        });

        vm.toggle = function myfunction() {
            vm.checked = !vm.checked;
        };

        vm.save = function () {
            $http.post('/umbraco/backoffice/api/MemberGroup/Save', { id: memberGroupId, name: vm.memberGroup.name })
                .success(function (response) {
                    //console.log("save works");
                    //console.log(response);
                    $scope.nav.hideMenu();
                    $scope.nav.reloadNode(memberGroupId);

                    //navigationService​.​hideDialog​();
                    //$location​.​url​(​item​.​editPath​);

                    //set the active item on the tree
                    //navigationService.syncTree({ tree: 'workshopTree', path: ["-1", vm.workshop.id], forceReload: false });
                });
        };

        vm.click = function () {
            notificationsService.success("OPAAAAAA!");
        };

    });

//angular.module("umbraco").controller("community.dashboard", function ($scope, contentResource, entityResource) {
//
//
//    entityResource.getByQuery("//community", -1, "Document").then(function (document) {
//
//        contentResource.getChildren(document.id).then(function (response) {
//            var community = response.items;
//            _.each(community, function (member) {
//                var date = moment(member.updateDate);
//                member.outdated = date.diff(Date.now(), "days") <= -6;
//                member.diff = date.fromNow();
//                member.avatar = _.findWhere(member.properties, { 'alias': 'twitterHandle' }).value;
//            })
//
//            $scope.community = community;
//
//        });
//
//    });


    //$scope.generate = function () {
    //    $scope.generating = true;
    //    umbRequestHelper.resourcePromise(
    //        $http.post(umbRequestHelper.getApiUrl("modelsBuilderBaseUrl", "BuildModels")),
    //        'Failed to generate.')
    //        .then(function (result) {
    //            $scope.generating = false;
    //            $scope.dashboard = result;
    //        });
    //};

//});