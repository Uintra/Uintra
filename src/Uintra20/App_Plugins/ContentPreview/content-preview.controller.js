function contentPreviewController($scope, editorState, contentEditingHelper,
    eventsService, navigationService, umbRequestHelper, $http, $q, contentResource, angularHelper)
{
    var vm = this;
    vm.culture = param("cculture");
    vm.previewUrl = "/umbraco/preview/?id=" + editorState.current.id + "#?culture=" + vm.culture;
    vm.width = 1200;
    vm.height = 750;
    var infiniteMode = $scope.infiniteModel && $scope.infiniteModel.infiniteMode;
    vm.contentForm = angularHelper.getNullForm("contentForm");
    vm.page = {
        isNew: $scope.isNew ? true : false
    };
    $scope.contentForm = $scope.$parent.$parent.$parent.$parent.$parent.$parent.$parent.$parent.$parent.contentForm;

    vm.refresh = function () {
        var selectedVariant = $scope.content.variants[0];
        if (vm.culture) {
            var found = _.find($scope.content.variants, function (v) {
                return (v.language && v.language.culture === vm.culture);
            });

            if (found) {
                selectedVariant = found;
            }
        }

        //ensure the save flag is set
        selectedVariant.save = true;
        performSave({ saveMethod: contentResource.save, action: "save" }).then(function (data) {
            var iframe = document.getElementsByClassName("js-preview-frame")[0];
            iframe.src = iframe.src;
        }, function (err) {
        });
        
    };

    function param(name) {
        var arr = (window.location.href.split(name + '=')[1] || '').split('&');
        return arr && arr.length > 0 ? arr[0] : "";
    }

    function performSave(args) {

        var fieldsToRollback = checkValidility();

        //Used to check validility of nested form - coming from Content Apps mostly
        //Set them all to be invalid
        eventsService.emit("content.saving", { content: $scope.content, action: args.action });
        return contentEditingHelper.contentEditorPerformSave({
            saveMethod: args.saveMethod,
            scope: $scope,
            content: $scope.content,
            create: vm.page.isNew,
            action: args.action,
            showNotifications: args.showNotifications,
            softRedirect: true
        }).then(function (data) {
                //needs to be manually set for infinite editing mode
                vm.page.isNew = false;

                eventsService.emit("content.saved", { content: $scope.content, action: args.action });

                resetNestedFieldValiation(fieldsToRollback);
                ensureDirtyIsSetIfAnyVariantIsDirty();

                return $q.when(data);
            },
            function (err) {
                syncTreeNode($scope.content, $scope.content.path);


                return $q.reject(err);
            });
    }

    function syncTreeNode(content, path, initialLoad) {

        if (infiniteMode || !path) {
            return;
        }

        if (!$scope.content.isChildOfListView) {
            navigationService.syncTree({ tree: $scope.treeAlias, path: path.split(","), forceReload: initialLoad !== true })
                .then(function (syncArgs) {
                    $scope.page.menu.currentNode = syncArgs.node;
                }, function () {
                    //handle the rejection
                    console.log("A problem occurred syncing the tree! A path is probably incorrect.")
                });
        }
        else if (initialLoad === true) {

            //it's a child item, just sync the ui node to the parent
            navigationService.syncTree({ tree: $scope.treeAlias, path: path.substring(0, path.lastIndexOf(",")).split(","), forceReload: initialLoad !== true });

            //if this is a child of a list view and it's the initial load of the editor, we need to get the tree node
            // from the server so that we can load in the actions menu.
            umbRequestHelper.resourcePromise(
                $http.get(content.treeNodeUrl),
                'Failed to retrieve data for child node ' + content.id).then(function (node) {
                $scope.page.menu.currentNode = node;
            });
        }
    }

    function recurseFormControls(controls, array) {

        //Loop over the controls
        for (var i = 0; i < controls.length; i++) {
            var controlItem = controls[i];

            //Check if the controlItem has a property ''
            if (controlItem.hasOwnProperty('$submitted')) {
                //This item is a form - so lets get the child controls of it & recurse again
                var childFormControls = controlItem.$getControls();
                recurseFormControls(childFormControls, array);
            }
            else {
                //We can assume its a field on a form
                if (controlItem.hasOwnProperty('$error')) {
                    //Set the validlity of the error/s to be valid
                    //String of keys of error invalid messages
                    var errorKeys = [];

                    for (var key in controlItem.$error) {
                        errorKeys.push(key);
                        controlItem.$setValidity(key, true);
                    }

                    //Create a basic obj - storing the control item & the error keys
                    var obj = { 'control': controlItem, 'errorKeys': errorKeys };

                    //Push the updated control into the array - so we can set them back
                    array.push(obj);
                }
            }
        }
        return array;
    }

    function resetNestedFieldValiation(array) {
        for (var i = 0; i < array.length; i++) {
            var item = array[i];
            //Item is an object containing two props
            //'control' (obj) & 'errorKeys' (string array)
            var fieldControl = item.control;
            var fieldErrorKeys = item.errorKeys;

            for (var j = 0; j < fieldErrorKeys.length; j++) {
                fieldControl.$setValidity(fieldErrorKeys[j], false);
            }
        }
    }

    function ensureDirtyIsSetIfAnyVariantIsDirty() {

        $scope.contentForm.$dirty = false;

        for (var i = 0; i < $scope.content.variants.length; i++) {
            if ($scope.content.variants[i].isDirty) {
                $scope.contentForm.$dirty = true;
                return;
            }
        }
    }

    function checkValidility() {
        //Get all controls from the 'contentForm'
        var allControls = $scope.contentForm.$getControls();

        //An array to store items in when we find child form fields (no matter how many deep nested forms)
        var childFieldsToMarkAsValid = [];

        //Exclude known formControls 'contentHeaderForm' and 'tabbedContentForm'
        //Check property - $name === "contentHeaderForm"
        allControls = _.filter(allControls, function (obj) {
            return obj.$name !== 'contentHeaderForm' && obj.$name !== 'tabbedContentForm' && obj.hasOwnProperty('$submitted');
        });

        for (var i = 0; i < allControls.length; i++) {
            var nestedForm = allControls[i];

            //Get Nested Controls of this form in the loop
            var nestedFormControls = nestedForm.$getControls();

            //Need to recurse through controls (could be more nested forms)
            childFieldsToMarkAsValid = recurseFormControls(nestedFormControls, childFieldsToMarkAsValid);
        }

        return childFieldsToMarkAsValid;
    }
}
angular.module("umbraco").controller("UBaseline.ContentPreviewController", contentPreviewController);