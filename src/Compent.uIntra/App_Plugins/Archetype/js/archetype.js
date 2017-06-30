angular.module("umbraco").controller("Imulus.ArchetypeController", function ($scope, $http, assetsService, angularHelper, notificationsService, $timeout, fileManager, entityResource, archetypeService, archetypeLabelService, archetypeCacheService, archetypePropertyEditorResource) {

    //$scope.model.value = "";
    $scope.model.hideLabel = $scope.model.config.hideLabel == 1;

    //get a reference to the current form
    $scope.form = $scope.form || angularHelper.getCurrentForm($scope);

    //set the config equal to our prevalue config
    $scope.model.config = $scope.model.config.archetypeConfig;

    //ini the model
    $scope.model.value = $scope.model.value || getDefaultModel($scope.model.config);

    // store the umbraco property alias to help generate unique IDs.  Hopefully there's a better way to get this in the future :)
    $scope.umbracoHostPropertyAlias = $scope.$parent.$parent.model.alias;

    $scope.overlayMenu = {
        show: false,
        style: {}
    };

    init();

    //hold references to helper resources 
    $scope.resources = {
        entityResource: entityResource,
        archetypePropertyEditorResource: archetypePropertyEditorResource
    }

    //hold references to helper services 
    $scope.services = {
        archetypeService: archetypeService,
        archetypeLabelService: archetypeLabelService,
        archetypeCacheService: archetypeCacheService
    }

    //helper to get $eval the labelTemplate
    $scope.getFieldsetTitle = function (fieldsetConfigModel, fieldsetIndex) {
        return archetypeLabelService.getFieldsetTitle($scope, fieldsetConfigModel, fieldsetIndex);
    }

    var draggedRteSettings;
    var rteClass = ".mce-tinymce";

    //sort config
    $scope.sortableOptions = {
        axis: 'y',
        cursor: "move",
        handle: ".handle",
        start: function(ev, ui) {
            draggedRteSettings = {};
            ui.item.parent().find(rteClass).each(function () {
                // remove all RTEs in the dragged row and save their settings
                var $element = $(this);
                var wrapperId = $element.attr('id');
                var $textarea = $element.siblings('textarea');
                var textareaId = $textarea.attr('id');

                draggedRteSettings[textareaId] = _.findWhere(tinyMCE.editors, { id: textareaId }).settings;
                tinyMCE.execCommand('mceRemoveEditor', false, wrapperId);
            });
        },
        update: function (ev, ui) {
            $scope.setDirty();
        },
        stop: function (ev, ui) {
            ui.item.parent().find(rteClass).each(function () {
                var $element = $(this);
                var wrapperId = $element.attr('id');
                var $textarea = $element.siblings('textarea');
                var textareaId = $textarea.attr('id');

                draggedRteSettings[textareaId] = draggedRteSettings[textareaId] || _.findWhere(tinyMCE.editors, { id: textareaId }).settings;
                tinyMCE.execCommand('mceRemoveEditor', false, wrapperId);
                tinyMCE.init(draggedRteSettings[textareaId]);
            });
        }
    };

    //handles a fieldset add
    $scope.openFieldsetPicker = function ($index, event) {
        if ($scope.canAdd() == false) {
            return;
        }

        $scope.overlayMenu.fieldsets = [];
        _.each($scope.model.config.fieldsets, function (fieldset) {
            var icon = fieldset.icon;
            $scope.overlayMenu.fieldsets.push({
                alias: fieldset.alias,
                label: fieldset.label,
                icon: (fieldset.icon || "icon-document-dashed-line") // default icon if none is chosen
            });
            $scope.overlayMenu.index = $index;
        });

        // sanity check
        if ($scope.overlayMenu.fieldsets.length == 0) {
            return;
        }
        if ($scope.overlayMenu.fieldsets.length == 1) {
            // only one fieldset type - no need to display the picker
            $scope.addRow($scope.overlayMenu.fieldsets[0].alias, $index);
            return;
        }

        // calculate overlay position
        // - yeah... it's jQuery (ungh!) but that's how the Grid does it.
        var offset = $(event.target).offset();
        var scrollTop = $(event.target).closest(".umb-panel-body").scrollTop();
        if (offset.top < 400) {
            $scope.overlayMenu.style.top = 300 + scrollTop;
        }
        else {
            $scope.overlayMenu.style.top = offset.top - 150 + scrollTop;
        }
        $scope.overlayMenu.show = true;
    };

    $scope.closeFieldsetPicker = function () {
        $scope.overlayMenu.show = false;
    };
    
    $scope.pickFieldset = function (fieldsetAlias, $index) {
        $scope.closeFieldsetPicker();
        $scope.addRow(fieldsetAlias, $index);
    };    
    
    $scope.addRow = function (fieldsetAlias, $index) {
        if ($scope.canAdd()) {
            if ($scope.model.config.fieldsets) {
                var newFieldset = getEmptyRenderFieldset($scope.getConfigFieldsetByAlias(fieldsetAlias));

                if (typeof $index != 'undefined')
                {
                    $scope.model.value.fieldsets.splice($index + 1, 0, newFieldset);
                }
                else
                {
                    $scope.model.value.fieldsets.push(newFieldset);
                }
            }

            $scope.setDirty();

            $scope.$broadcast("archetypeAddFieldset", {index: $index, visible: countVisible()});

            newFieldset.collapse = $scope.model.config.enableCollapsing ? true : false;
            
            $scope.focusFieldset(newFieldset);
        }
    }

    $scope.removeRow = function ($index) {
        if ($scope.canRemove()) {
            if (confirm('Are you sure you want to remove this?')) {
                $scope.setDirty();
                $scope.model.value.fieldsets.splice($index, 1);
                $scope.$broadcast("archetypeRemoveFieldset", {index: $index});
            }
        }
    }

    $scope.cloneRow = function ($index) {
        if ($scope.canClone() && typeof $index != 'undefined') {
            var newFieldset = angular.copy($scope.model.value.fieldsets[$index]);

            if(newFieldset) {

                $scope.model.value.fieldsets.splice($index + 1, 0, newFieldset);

                $scope.setDirty();

                newFieldset.collapse = $scope.model.config.enableCollapsing ? true : false;
                $scope.focusFieldset(newFieldset);
            }
        }
    }

    $scope.enableDisable = function (fieldset) {
        fieldset.disabled = !fieldset.disabled;
        // explicitly set the form as dirty when manipulating the enabled/disabled state of a fieldset
        $scope.setDirty();
    }

    //helpers for determining if a user can do something
    $scope.canAdd = function () {
        if ($scope.model.config.maxFieldsets)
        {
            return countVisible() < $scope.model.config.maxFieldsets;
        }

        return true;
    }

    //helper that returns if an item can be removed
    $scope.canRemove = function () {
        return countVisible() > 1 
            || ($scope.model.config.maxFieldsets == 1 && $scope.model.config.fieldsets.length > 1)
            || $scope.model.config.startWithAddButton;
    }

    $scope.canClone = function () {

        if (!$scope.model.config.enableCloning) {
            return false;
        }

        if ($scope.model.config.maxFieldsets)
        {
            return countVisible() < $scope.model.config.maxFieldsets;
        }

        return true;
    }

    //helper that returns if an item can be sorted
    $scope.canSort = function ()
    {
        return countVisible() > 1;
    }

    //helper that returns if an item can be disabled
    $scope.canDisable = function () {
        return $scope.model.config.enableDisabling;
    }

    //helpers for determining if the add button should be shown
    $scope.showAddButton = function () {
        return $scope.model.config.startWithAddButton
            && countVisible() === 0;
            ///&& $scope.model.config.fieldsets.length == 1;
    }

    //helper, ini the render model from the server (model.value)
    function init() {
        $scope.model.value = removeNulls($scope.model.value);
        addDefaultProperties($scope.model.value.fieldsets);
    }

    function addDefaultProperties(fieldsets)
    {
        _.each(fieldsets, function (fieldset)
        {
            fieldset.collapse = false;
            fieldset.isValid = true;
        });
    }

    //helper to get the correct fieldset from config
    $scope.getConfigFieldsetByAlias = function(alias) {
        return _.find($scope.model.config.fieldsets, function(fieldset){
            return fieldset.alias == alias;
        });
    }

    //helper to get a property by alias from a fieldset
    $scope.getPropertyValueByAlias = function(fieldset, propertyAlias) {
        var property = _.find(fieldset.properties, function(p) {
            return p.alias == propertyAlias;
        });
        return (typeof property !== 'undefined') ? property.value : '';
    };

    $scope.isCollapsed = function(fieldset)
    {
        if(typeof fieldset.collapse === "undefined")
        {
            fieldset.collapse = true;
        }
        return fieldset.collapse;
    }

    //helper for expanding/collapsing fieldsets
    $scope.focusFieldset = function(fieldset){
        fixDisableSelection();

        if (!$scope.model.config.enableCollapsing) {
            return;
        }

        var iniState;

        if(fieldset)
        {
            iniState = fieldset.collapse;
        }

        _.each($scope.model.value.fieldsets, function(fieldset){
            fieldset.collapse = true;
        });

        if(!fieldset && $scope.model.value.fieldsets.length == 1)
        {
            $scope.model.value.fieldsets[0].collapse = false;
            return;
        }

        if(iniState && fieldset)
        {
            fieldset.collapse = !iniState;
        }
    }

    //ini the fieldset expand/collapse
    $scope.focusFieldset();

    //developerMode helpers
    $scope.model.value.toString = stringify;

    // issue 114: register handler for file selection
    $scope.model.value.setFiles = setFiles;

    //encapsulate stringify (should be built into browsers, not sure of IE support)
    function stringify() {
        return JSON.stringify(this);
    }

    // issue 114: handler for file selection
    function setFiles(files) {
        // get all currently selected files from file manager
        var currentFiles = fileManager.getFiles();
        
        // get the files already selected for this archetype (by alias)
        var archetypeFiles = [];
        _.each(currentFiles, function (item) {
            if (item.alias === $scope.model.alias) {
                archetypeFiles.push(item.file);
            }
        });

        // add the newly selected files
        _.each(files, function (file) {
            archetypeFiles.push(file);
        });

        // update the selected files for this archetype (by alias)
        fileManager.setFiles($scope.model.alias, archetypeFiles);
    }

    //watch for changes
    $scope.$watch('model.value', function (v) {
        if ($scope.model.config.developerMode) {
            console.log(v);
            if (typeof v === 'string') {
                $scope.model.value = JSON.parse(v);
                $scope.model.value.toString = stringify;
            }
        }

        // issue 114: re-register handler for files selection and reset the currently selected files on the file manager
        $scope.model.value.setFiles = setFiles;
        fileManager.setFiles($scope.model.alias, []);

        // reset submit watcher counter on save
        $scope.activeSubmitWatcher = 0;
    });

    //helper to count what is visible
    function countVisible()
    {
        return $scope.model.value.fieldsets.length;
    }

    // helper to get initial model if none was provided
    function getDefaultModel(config) {
        if (config.startWithAddButton)
            return { fieldsets: [] };

        return { fieldsets: [getEmptyRenderFieldset(config.fieldsets[0])] };
    }

    //helper to add an empty fieldset to the render model
    function getEmptyRenderFieldset (fieldsetModel) {
        return {alias: fieldsetModel.alias, collapse: false, isValid: true, properties: []};
    }

    //helper to ensure no nulls make it into the model
    function removeNulls(model){
        if(model.fieldsets){
            _.each(model.fieldsets, function(fieldset, index){
                if(!fieldset){
                    model.fieldsets.splice(index, 1);
                    removeNulls(model);
                }
            });

            return model;
        }
    }

    // Hack for U4-4281 / #61
    function fixDisableSelection() {
        $timeout(function() {
            $('.archetypeEditor .controls')
                .bind('mousedown.ui-disableSelection selectstart.ui-disableSelection', function(e) {
                    e.stopImmediatePropagation();
                });
        }, 1000);
    }

    //helper to lookup validity when given a fieldsetIndex and property alias
    $scope.getPropertyValidity = function(fieldsetIndex, alias)
    {
        if($scope.model.value.fieldsets[fieldsetIndex])
        {
            var property = _.find($scope.model.value.fieldsets[fieldsetIndex].properties, function(property){
                return property.alias == alias;
            });
        }

        return (typeof property == 'undefined') ? true : property.isValid;
    }

    //helper to lookup validity when given a fieldset
    $scope.getFieldsetValidity = function (fieldset) {
        if (fieldset.isValid == false) {
            return false;
        }

        // recursive validation of nested fieldsets
        var nestedFieldsetsValid = true;
        _.each(fieldset.properties, function (property) {
            if (property != null && property.value != null && property.propertyEditorAlias == "Imulus.Archetype") {
                _.each(property.value.fieldsets, function (inner) {
                    if ($scope.getFieldsetValidity(inner) == false) {
                        nestedFieldsetsValid = false;
                    }
                });
            }
        });

        return nestedFieldsetsValid;
    }

    // helper to force the current form into the dirty state
    $scope.setDirty = function () {
        if($scope.form) {
            $scope.form.$setDirty();
        }
    }

    //custom js
    if ($scope.model.config.customJsPath) {
        assetsService.loadJs($scope.model.config.customJsPath);
    }

    //archetype css
    assetsService.loadCss("../App_Plugins/Archetype/css/archetype.css");

    //custom css
    if($scope.model.config.customCssPath)
    {
        assetsService.loadCss($scope.model.config.customCssPath);
    }

    // submit watcher handling:
    // because some property editors use the "formSubmitting" event to set/clean up their model.value,
    // we need to monitor the "formSubmitting" event from a custom property and broadcast our own event
    // to forcefully update the appropriate model.value's
    $scope.activeSubmitWatcher = 0;
    $scope.submitWatcherOnLoad = function () {
        $scope.activeSubmitWatcher++;
        return $scope.activeSubmitWatcher;
    }
    $scope.submitWatcherOnSubmit = function () {
        $scope.$broadcast("archetypeFormSubmitting");
    }
});

angular.module("umbraco").controller("Imulus.ArchetypeConfigController", function ($scope, $http, assetsService, dialogService, archetypePropertyEditorResource) {

    //$scope.model.value = "";
    //console.log($scope.model.value);

    //define empty items
    var newPropertyModel = '{"alias": "", "remove": false, "collapse": false, "label": "", "helpText": "", "dataTypeGuid": "0cc0eba1-9960-42c9-bf9b-60e150b429ae", "value": ""}';
    var newFieldsetModel = '{"alias": "", "remove": false, "collapse": false, "labelTemplate": "", "icon": "", "label": "", "properties": [' + newPropertyModel + ']}';
    var defaultFieldsetConfigModel = JSON.parse('{"showAdvancedOptions": false, "startWithAddButton": false, "hideFieldsetToolbar": false, "enableMultipleFieldsets": false, "hideFieldsetControls": false, "hidePropertyLabel": false, "maxFieldsets": null, "enableCollapsing": true, "enableCloning": false, "enableDisabling": true, "enableDeepDatatypeRequests": false, "fieldsets": [' + newFieldsetModel + ']}');

    //ini the model
    $scope.model.value = $scope.model.value || defaultFieldsetConfigModel;

    //ini the render model
    initConfigRenderModel();

    //get the available datatypes
    archetypePropertyEditorResource.getAllDataTypes().then(function(data) {
        $scope.availableDataTypes = data;
    });

    //iconPicker
    $scope.selectIcon = function(fieldset){
        var dialog = dialogService.iconPicker({
            callback: function(data){
                fieldset.icon = data;
            }
        });
    }

    //config for the sorting
    $scope.sortableOptions = {
        axis: 'y',
        cursor: "move",
        handle: ".handle",
        update: function (ev, ui) {

        },
        stop: function (ev, ui) {

        }
    };

    //function that determines how to manage expanding/collapsing fieldsets
    $scope.focusFieldset = function(fieldset){
        var iniState;

        if(fieldset)
        {
            iniState = fieldset.collapse;
        }

        _.each($scope.archetypeConfigRenderModel.fieldsets, function(fieldset){
            if($scope.archetypeConfigRenderModel.fieldsets.length == 1 && fieldset.remove == false)
            {
                fieldset.collapse = false;
                return;
            }

            if(fieldset.label)
            {
                fieldset.collapse = true;
            }
            else
            {
                fieldset.collapse = false;
            }
        });

        if(iniState)
        {
            fieldset.collapse = !iniState;
        }
    }

    //ini the fieldsets
    $scope.focusFieldset();

    //function that determines how to manage expanding/collapsing properties
    $scope.focusProperty = function(properties, property){
        var iniState;

        if(property)
        {
            iniState = property.collapse;
        }

        _.each(properties, function(property){
            if(property.label)
            {
                property.collapse = true;
            }
            else
            {
                property.collapse = false;
            }
        });

        if(iniState)
        {
            property.collapse = !iniState;
        }
    }

    //ini the properties
    _.each($scope.archetypeConfigRenderModel.fieldsets, function(fieldset){
        $scope.focusProperty(fieldset.properties);
    });

    //setup JSON.stringify helpers
    $scope.archetypeConfigRenderModel.toString = stringify;

    //encapsulate stringify (should be built into browsers, not sure of IE support)
    function stringify() {
        return JSON.stringify(this);
    }

    //watch for changes
    $scope.$watch('archetypeConfigRenderModel', function (v) {
        //console.log(v);
        if (typeof v === 'string') {
            $scope.archetypeConfigRenderModel = JSON.parse(v);
            $scope.archetypeConfigRenderModel.toString = stringify;
        }
    });

    $scope.autoPopulateAlias = function(s) {
        var modelType = s.hasOwnProperty('fieldset') ? 'fieldset' : 'property';
        var modelProperty = s[modelType];

        if (!modelProperty.aliasIsDirty) {
            modelProperty.alias = modelProperty.label.toUmbracoAlias();
        }
    }

    $scope.markAliasDirty = function(s) {
        var modelType = s.hasOwnProperty('fieldset') ? 'fieldset' : 'property';
        var modelProperty = s[modelType];

        if (!modelProperty.aliasIsDirty) {
            modelProperty.aliasIsDirty = true;;
        }
    }

    //helper that returns if an item can be removed
    $scope.canRemoveFieldset = function ()
    {
        return countVisibleFieldset() > 1;
    }

    //helper that returns if an item can be sorted
    $scope.canSortFieldset = function ()
    {
        return countVisibleFieldset() > 1;
    }

    //helper that returns if an item can be removed
    $scope.canRemoveProperty = function (fieldset)
    {
        return countVisibleProperty(fieldset) > 1;
    }

    //helper that returns if an item can be sorted
    $scope.canSortProperty = function (fieldset)
    {
        return countVisibleProperty(fieldset) > 1;
    }

    $scope.getDataTypeNameByGuid = function (guid) {
        if ($scope.availableDataTypes == null) // Might not be initialized yet?
            return "";
        
        var dataType = _.find($scope.availableDataTypes, function(d) {
            return d.guid == guid;
        });

        return dataType == null ? "" : dataType.name;
    }

    //helper to count what is visible
    function countVisibleFieldset()
    {
        var count = 0;

        _.each($scope.archetypeConfigRenderModel.fieldsets, function(fieldset){
            if (fieldset.remove == false) {
                count++;
            }
        });

        return count;
    }

    //determines how many properties are visible
    function countVisibleProperty(fieldset)
    {
        var count = 0;

        for (var i in fieldset.properties) {
            if (fieldset.properties[i].remove == false) {
                count++;
            }
        }

        return count;
    }

    //handles a fieldset add
    $scope.addFieldsetRow = function ($index, $event) {
        $scope.archetypeConfigRenderModel.fieldsets.splice($index + 1, 0, JSON.parse(newFieldsetModel));
        $scope.focusFieldset();
    }

    //rather than splice the archetypeConfigRenderModel, we're hiding this and cleaning onFormSubmitting
    $scope.removeFieldsetRow = function ($index) {
        if ($scope.canRemoveFieldset()) {
            if (confirm('Are you sure you want to remove this?')) {
                $scope.archetypeConfigRenderModel.fieldsets[$index].remove = true;
            }
        }
    }

    //handles a property add
    $scope.addPropertyRow = function (fieldset, $index) {
        fieldset.properties.splice($index + 1, 0, JSON.parse(newPropertyModel));
    }

    //rather than splice the archetypeConfigRenderModel, we're hiding this and cleaning onFormSubmitting
    $scope.removePropertyRow = function (fieldset, $index) {
        if ($scope.canRemoveProperty(fieldset)) {
            if (confirm('Are you sure you want to remove this?')) {
                fieldset.properties[$index].remove = true;
            }
        }
    }

    //helper to ini the render model
    function initConfigRenderModel()
    {
        $scope.archetypeConfigRenderModel = $scope.model.value;

        _.each($scope.archetypeConfigRenderModel.fieldsets, function(fieldset){

            fieldset.remove = false;
            if (fieldset.alias.length > 0)
                fieldset.aliasIsDirty = true;

            if(fieldset.label)
            {
                fieldset.collapse = true;
            }

            _.each(fieldset.properties, function(fieldset){
                fieldset.remove = false;
                if (fieldset.alias.length > 0)
                    fieldset.aliasIsDirty = true;
            });
        });
    }

    //sync things up on save
    $scope.$on("formSubmitting", function (ev, args) {
        syncModelToRenderModel();
    });

    //helper to sync the model to the renderModel
    function syncModelToRenderModel()
    {
        $scope.model.value = $scope.archetypeConfigRenderModel;
        var fieldsets = [];

        _.each($scope.archetypeConfigRenderModel.fieldsets, function(fieldset){
            //check fieldsets
            if (!fieldset.remove) {
                fieldsets.push(fieldset);

                var properties = [];

                _.each(fieldset.properties, function(property){
                   if (!property.remove) {
                        properties.push(property);
                    }
                });

                fieldset.properties = properties;
            }
        });

        $scope.model.value.fieldsets = fieldsets;
    }

    //archetype css
    assetsService.loadCss("../App_Plugins/Archetype/css/archetype.css");
});

angular.module("umbraco.directives").directive('archetypeProperty', function ($compile, $http, archetypePropertyEditorResource, umbPropEditorHelper, $timeout, $rootScope, $q, fileManager, editorState, archetypeService, archetypeCacheService) {

    var linker = function (scope, element, attrs, ngModelCtrl) {
        var configFieldsetModel = archetypeService.getFieldsetByAlias(scope.archetypeConfig.fieldsets, scope.fieldset.alias);
        var view = "";
        var label = configFieldsetModel.properties[scope.propertyConfigIndex].label;
        var dataTypeGuid = configFieldsetModel.properties[scope.propertyConfigIndex].dataTypeGuid;
        var config = null;
        var alias = configFieldsetModel.properties[scope.propertyConfigIndex].alias;
        var defaultValue = configFieldsetModel.properties[scope.propertyConfigIndex].value;
        var propertyAliasParts = [];
        var propertyAlias = archetypeService.getUniquePropertyAlias(scope, propertyAliasParts);
        
        //try to convert the defaultValue to a JS object
        defaultValue = archetypeService.jsonOrString(defaultValue, scope.archetypeConfig.developerMode, "defaultValue");

        //grab info for the selected datatype, prepare for view
        archetypePropertyEditorResource.getDataType(dataTypeGuid, scope.archetypeConfig.enableDeepDatatypeRequests, editorState.current.contentTypeAlias, scope.propertyEditorAlias, alias, editorState.current.id).then(function (data) {
            //transform preValues array into object expected by propertyeditor views
            var configObj = {};

            _.each(data.preValues, function(p) {
                configObj[p.key] = p.value;
            });
            
            config = configObj;

            //caching for use by label templates later
            archetypeCacheService.addDatatypeToCache(data, dataTypeGuid);

            //determine the view to use [...] and load it
            archetypePropertyEditorResource.getPropertyEditorMapping(data.selectedEditor).then(function(propertyEditor) {
                var pathToView = umbPropEditorHelper.getViewPath(propertyEditor.view);

                //load in the DefaultPreValues for the PropertyEditor, if any
                var defaultConfigObj =  {};
                if (propertyEditor.hasOwnProperty('defaultPreValues') && propertyEditor.defaultPreValues != null) {
                    _.extend(defaultConfigObj, propertyEditor.defaultPreValues);
                }

                var mergedConfig = _.extend(defaultConfigObj, config);

                loadView(pathToView, mergedConfig, defaultValue, alias, propertyAlias, scope, element, ngModelCtrl, configFieldsetModel);
            });
        });
    }

    function loadView(view, config, defaultValue, alias, propertyAlias, scope, element, ngModelCtrl, configFieldsetModel) {
        if (view)
        {
            $http.get(view, { cache: true }).success(function (data) {
                if (data) {
                    if (scope.archetypeConfig.developerMode == '1')
                    {
                        console.log(scope);
                    }

                    //define the initial model and config
                    scope.form = scope.umbracoForm;
                    scope.model = {};
                    scope.model.config = {};

                    //ini the property value after test to make sure a prop exists in the renderModel
                    scope.renderModelPropertyIndex = archetypeService.getPropertyIndexByAlias(archetypeService.getFieldset(scope).properties, alias);

                    if (!scope.renderModelPropertyIndex)
                    {
                        archetypeService.getFieldset(scope).properties.push(JSON.parse('{"alias": "' + alias + '", "value": "' + defaultValue + '"}'));
                        scope.renderModelPropertyIndex = archetypeService.getPropertyIndexByAlias(archetypeService.getFieldset(scope).properties, alias);
                    }

                    scope.renderModel = {};
                    scope.model.value = archetypeService.getFieldsetProperty(scope).value;

                    //init the property editor state
                    archetypeService.getFieldsetProperty(scope).editorState = {};

                    //set the config from the prevalues
                    scope.model.config = config;

                    /*
                        Property Specific Hacks

                        Preference is to not do these, but unless we figure out core issues, this is quickest fix.
                    */

                    //MNTP *single* hack
                    if(scope.model.config.maxNumber && scope.model.config.multiPicker)
                    {
                        if(scope.model.config.maxNumber == "1")
                        {
                            scope.model.config.multiPicker = "0";
                        }
                    }

                    //upload datatype hack
                    if(view.indexOf('fileupload.html') != -1) {
                        scope.propertyForm = scope.form;
                        scope.model.validation = {};
                        scope.model.validation.mandatory = 0;
                    }

                    //some items need an alias
                    scope.model.alias = "archetype-property-" + propertyAlias;
                    //some items also need an id (file upload for example)
                    scope.model.id = propertyAlias;

                    //watch for changes since there is no two-way binding with the local model.value
                    scope.$watch('model.value', function (newValue, oldValue) {
                        
                        archetypeService.getFieldsetProperty(scope).value = newValue;

                        // notify the linker that the property value changed
                        archetypeService.propertyValueChanged(archetypeService.getFieldset(scope), archetypeService.getFieldsetProperty(scope));
                    });

                    scope.$on('archetypeFormSubmitting', function (ev, args) {
                        // validate all fieldset properties
                        _.each(scope.fieldset.properties, function (property) {
                            archetypeService.validateProperty(scope.fieldset, property, configFieldsetModel);
                        });

                        var validationKey = "validation-f" + scope.fieldsetIndex;

                        ngModelCtrl.$setValidity(validationKey, scope.fieldset.isValid);

                        // did the value change (if it did, it most likely did so during the "formSubmitting" event)
                        var property = archetypeService.getFieldsetProperty(scope);

                        var currentValue = property.value;

                        if (currentValue != scope.model.value) {
                            archetypeService.getFieldsetProperty(scope).value = scope.model.value;

                            // notify the linker that the property value changed
                            archetypeService.propertyValueChanged(archetypeService.getFieldset(scope), archetypeService.getFieldsetProperty(scope));
                        }
                    });

                    // issue 114: handle file selection on property editors
                    scope.$on("filesSelected", function (event, args) {
                        // populate the fileNames collection on the property editor state
                        var property = archetypeService.getFieldsetProperty(scope);

                        property.editorState.fileNames = [];

                        _.each(args.files, function (item) {
                            property.editorState.fileNames.push(item.name);
                        });

                        // remove the files set for this property
                        // NOTE: we can't use property.alias because the file manager registers the selected files on the assigned Archetype property alias (e.g. "archetype-property-archetype-property-archetype-property-content-0-2-0-1-0-0")
                        fileManager.setFiles(scope.model.alias, []);

                        // now tell the containing Archetype to pick up the selected files
                        scope.archetypeRenderModel.setFiles(args.files);
                    });

                    scope.$on("archetypeRemoveFieldset", function (ev, args) {
                        var validationKey = "validation-f" + args.index;
                        ngModelCtrl.$setValidity(validationKey, true);
                    });

                    element.html(data).show();
                    $compile(element.contents())(scope);

                    $timeout(function() {
                        var def = $q.defer();
                        def.resolve(true);
                        $rootScope.$apply();
                    }, 500);
                }
            });
        }
    }

    return {
        require: "^ngModel",
        restrict: "E",
        replace: true,
        link: linker,
        scope: {
            property: '=',
            propertyConfigIndex: '=',
            propertyEditorAlias: '=',
            archetypeConfig: '=',
            fieldset: '=',
            fieldsetIndex: '=',
            archetypeRenderModel: '=',
            umbracoPropertyAlias: '=',
            umbracoForm: '='
        }
    }
});
angular.module("umbraco.directives").directive('archetypeSubmitWatcher', function ($rootScope) {
    var linker = function (scope, element, attrs, ngModelCtrl) {
        // call the load callback on scope to obtain the ID of this submit watcher
        var id = scope.loadCallback();
        scope.$on("formSubmitting", function (ev, args) {
            // on the "formSubmitting" event, call the submit callback on scope to notify the Archetype controller to do it's magic
            if (id == scope.activeSubmitWatcher) {
                scope.submitCallback();
            }
        });
    }

    return {
        restrict: "E",
        replace: true,
        link: linker,
        template: "",
        scope: {
            loadCallback: '=',
            submitCallback: '=',
            activeSubmitWatcher: '='
        }
    }
});

angular.module("umbraco.directives").directive('archetypeCustomView', function ($compile, $http) {
    var linker = function (scope, element, attrs) {

        var view = "/App_plugins/Archetype/views/archetype.default.html";
        if(scope.model.config.customViewPath) {
            view = scope.model.config.customViewPath;
        }

        $http.get(view, { cache: true }).then(function(data) {

            element.html(data.data).show();

            $compile(element.contents())(scope);
        });
    }

    return {
        restrict: "A",
        replace: true,
        link: linker
    }
});
angular.module("umbraco.directives").directive('archetypeLocalize', function (archetypeLocalizationService) {
	var linker = function (scope, element, attrs){

		var key = scope.key;
        
        archetypeLocalizationService.localize(key).then(function(value){
        	if(value){
        		element.html(value);
        	}
        });
	}   

	return {
	    restrict: "E",
	    replace: true,
	    link: linker,
	    scope: {
	    	key: '@'
	    }
	}
});
angular.module("umbraco.directives")
    .directive('archetypeClickOutside', function ($timeout, $parse) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {
                var fn = $parse(attrs.archetypeClickOutside);

                // add click event handler (delayed so we don't trigger the callback immediately if this directive itself was triggered by a mouse click)
                $timeout(function () {
                  $(document).on("click", mouseClick);
                }, 500);

                function mouseClick(event) {  
                    if($(event.target).closest(element).length > 0) {
                        return;
                    }
                    var callback = function () {
                        fn(scope, { $event: event });
                    };
                    scope.$apply(callback);
                }

                // unbind event
                scope.$on('$destroy', function () {
                    $(document).off("click", mouseClick);
                });
            }
        };
    });
angular.module('umbraco.services').factory('archetypeLocalizationService', function($http, $q, userService){
    var service = {
        resourceFileLoaded: false,
        dictionary: {},
        localize: function(key) {
            var deferred = $q.defer();

            if(service.resourceFileLoaded){
                var value = service._lookup(key);
                deferred.resolve(value);
            }
            else{
                service.initLocalizedResources().then(function(dictionary){
                   var value = service._lookup(key);
                   deferred.resolve(value); 
                });
            } 

            return deferred.promise;
        },
        _lookup: function(key){
            return service.dictionary[key];
        },
        initLocalizedResources:function () {
            var deferred = $q.defer();
            userService.getCurrentUser().then(function(user){
                $http.get("../App_plugins/Archetype/langs/" + user.locale + ".js", { cache: true }) 
                    .then(function(response){
                        service.resourceFileLoaded = true;
                        service.dictionary = response.data;

                        return deferred.resolve(service.dictionary);
                    }, function(err){
                        return deferred.reject("Lang file missing");
                    });
            });
            return deferred.promise;
        }
    }

    return service;
});
var ArchetypeSampleLabelTemplates = (function() {

    //public functions
    return {
        Entity: function (value, scope, args) {

           if(!args.entityType) {
                args = {entityType: "Document", propertyName: "name"}
            }

            if (value) {
                //if handed a csv list, take the first only
                var id = value.split(",")[0];

                if (id) {
                    var entity = scope.services.archetypeLabelService.getEntityById(scope, id, args.entityType);

                    if(entity) {
                        return entity[args.propertyName];
                    }
                }
            }

            return "";
        },
        UrlPicker: function(value, scope, args) {

            if(!args.propertyName) {
                args = {propertyName: "name"}
            }

            var entity;

            switch (value.type) {
                case "content":
                    if(value.typeData.contentId) {
                        entity = scope.services.archetypeLabelService.getEntityById(scope, value.typeData.contentId, "Document");
                    }
                    break;

                case "media":
                    if(value.typeData.mediaId) {
                        entity = scope.services.archetypeLabelService.getEntityById(scope, value.typeData.mediaId, "Media");
                    }
                    break;

                case "url":
                    return value.typeData.url;
                    
                default:
                    break;

            }

            if(entity) {
                return entity[args.propertyName];
            }

            return "";
        },
        Rte: function (value, scope, args) {

            if(!args.contentLength) {
                args = {contentLength: 50}
            }

            return $(value).text().substring(0, args.contentLength);
        }
    }
})();
angular.module('umbraco.resources').factory('archetypePropertyEditorResource', function($q, $http, umbRequestHelper){
    return { 
        getAllDataTypes: function() {
            // Hack - grab DataTypes from Tree API, as `dataTypeService.getAll()` isn't implemented yet
            return umbRequestHelper.resourcePromise(
                $http.get("backoffice/ArchetypeApi/ArchetypeDataType/GetAll"), 'Failed to retrieve datatypes from tree service'
            );
        },
        getDataType: function (guid, useDeepDatatypeLookup, contentTypeAlias, propertyTypeAlias, archetypeAlias, nodeId) {
            if(useDeepDatatypeLookup) {
            	return umbRequestHelper.resourcePromise(
            		$http.get("backoffice/ArchetypeApi/ArchetypeDataType/GetByGuid?guid=" + guid + "&contentTypeAlias=" + contentTypeAlias + "&propertyTypeAlias=" + propertyTypeAlias + "&archetypeAlias=" + archetypeAlias + "&nodeId=" + nodeId), 'Failed to retrieve datatype'
        		);
            }
            else {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/ArchetypeApi/ArchetypeDataType/GetByGuid?guid=" + guid , { cache: true }), 'Failed to retrieve datatype'
                );
            }
        },
        getPropertyEditorMapping: function(alias) {
            return umbRequestHelper.resourcePromise(
                $http.get("backoffice/ArchetypeApi/ArchetypeDataType/GetAllPropertyEditors", { cache: true }), 'Failed to retrieve datatype mappings'
            ).then(function (data) {
                var result = _.find(data, function(d) {
                    return d.alias === alias;
                });

                if (result != null) 
                    return result;

                return "";
            });
        }
    }
}); 

angular.module('umbraco.services').factory('archetypeService', function () {
    //public
    return {
        //helper that returns a JS ojbect from 'value' string or the original string
        jsonOrString: function (value, developerMode, debugLabel){
            if(value && typeof value == 'string'){
                try{
                    if(developerMode == '1'){
                        console.log("Trying to parse " + debugLabel + ": " + value); 
                    }
                    value = JSON.parse(value);
                }
                catch(exception)
                {
                    if(developerMode == '1'){
                        console.log("Failed to parse " + debugLabel + "."); 
                    }
                }
            }

            if(value && developerMode == '1'){
                console.log(debugLabel + " post-parsing: ");
                console.log(value); 
            }

            return value;
        },
        getFieldsetByAlias: function (fieldsets, alias)
        {
            return _.find(fieldsets, function(fieldset){
                return fieldset.alias == alias;
            });
        },
        getPropertyIndexByAlias: function(properties, alias){
            for (var i in properties)
            {
                if (properties[i].alias == alias) {
                    return i;
                }
            }
        },
        getPropertyByAlias: function (fieldset, alias){
            return _.find(fieldset.properties, function(property){
                return property.alias == alias; 
            });
        },
        getUniquePropertyAlias: function (currentScope, propertyAliasParts) {
            if (currentScope.hasOwnProperty('fieldsetIndex') && currentScope.hasOwnProperty('property') && currentScope.hasOwnProperty('propertyConfigIndex'))
            {
                var currentPropertyAlias = "f" + currentScope.fieldsetIndex + "-" + currentScope.property.alias + "-p" + currentScope.propertyConfigIndex;
                propertyAliasParts.push(currentPropertyAlias);
            }
            else if (currentScope.hasOwnProperty('isPreValue')) // Crappy way to identify this is the umbraco property scope
            {
                var umbracoPropertyAlias = currentScope.$parent.$parent.property.alias; // Crappy way to get the umbraco host alias once we identify its scope
                propertyAliasParts.push(umbracoPropertyAlias);
            }

            if (currentScope.$parent)
                this.getUniquePropertyAlias(currentScope.$parent, propertyAliasParts);

            return _.unique(propertyAliasParts).reverse().join("-");
        },
        getFieldset: function(scope) {
            return scope.archetypeRenderModel.fieldsets[scope.fieldsetIndex];
        },
        getFieldsetProperty: function (scope) {
            return this.getFieldset(scope).properties[scope.renderModelPropertyIndex];
        },
        setFieldsetValidity: function (fieldset) {
            // mark the entire fieldset as invalid if there are any invalid properties in the fieldset, otherwise mark it as valid
            fieldset.isValid =
                _.find(fieldset.properties, function (property) {
                    return property.isValid == false
                }) == null;
        },
        validateProperty: function (fieldset, property, configFieldsetModel) {
            var propertyConfig = this.getPropertyByAlias(configFieldsetModel, property.alias);

            if (propertyConfig) {
                // use property.value !== property.value to check for NaN values on numeric inputs
                if (propertyConfig.required && (property.value == null || property.value === "" || property.value !== property.value)) {
                    property.isValid = false;
                }
                // issue 116: RegEx validate property value
                // Only validate the property value if anything has been entered - RegEx is considered a supplement to "required".
                if (property.isValid == true && propertyConfig.regEx && property.value) {
                    var regEx = new RegExp(propertyConfig.regEx);
                    if (regEx.test(property.value) == false) {
                        property.isValid = false;
                    }
                }
            }

            this.setFieldsetValidity(fieldset);
        },
        // called when the value of any property in a fieldset changes
        propertyValueChanged: function (fieldset, property) {
            // it's the Umbraco way to hide the invalid state when altering an invalid property, even if the new value isn't valid either
            property.isValid = true;
            this.setFieldsetValidity(fieldset);
        }
    }
});
angular.module('umbraco.services').factory('archetypeLabelService', function (archetypeCacheService) {
    //private

    function executeFunctionByName(functionName, context) {
        var args = Array.prototype.slice.call(arguments).splice(2);

        var namespaces = functionName.split(".");
        var func = namespaces.pop();

        for(var i = 0; i < namespaces.length; i++) {
            context = context[namespaces[i]];
        }

        if(context && context[func]) {
            return context[func].apply(this, args);
        }

        return "";
    }

    function getNativeLabel(datatype, value, scope) {

    	switch (datatype.selectedEditor) {
    		case "Imulus.UrlPicker":
    			return imulusUrlPicker(value, scope, {});
    		case "Umbraco.TinyMCEv3":
    			return coreTinyMce(value, scope, {});
            case "Umbraco.MultiNodeTreePicker":
                return coreMntp(value, scope, datatype);
            case "Umbraco.MediaPicker":
                return coreMediaPicker(value, scope, datatype);
            case "Umbraco.DropDown":
                return coreDropdown(value, scope, datatype);
    		default:
    			return "";
    	}
    }

    function coreDropdown(value, scope, args) {

        if(!value)
            return "";

        var prevalue = args.preValues[0].value[value];

        if(prevalue) {
            return prevalue.value;
        }

        return "";
    }

    function coreMntp(value, scope, args) {
        var ids = value.split(',');
        var type = "Document";

        switch(args.preValues[0].value.type) {
            case 'content':
                type = 'Document';
                break;
            case 'media':
                type = 'media';
                break;
            case 'member':
                type = 'member';
                break;

            default:
                break;
        }

        var entityArray = [];

        _.each(ids, function(id){
            if(id) {

                var entity = archetypeCacheService.getEntityById(scope, id, type);
                
                if(entity) {
                    entityArray.push(entity.name);
                }
            }
        });

        return entityArray.join(', ');
    }

    function coreMediaPicker(value, scope, args) {
        if(value) {
             var entity = archetypeCacheService.getEntityById(scope, value, "media");     
             
            if(entity) {
                return entity.name; 
            }
        }

        return "";
    }

    function imulusUrlPicker(value, scope, args) {

        if(!args.propertyName) {
            args = {propertyName: "name"}
        }

        var entity;

        switch (value.type) {
            case "content":
                if(value.typeData.contentId) {
                    entity = archetypeCacheService.getEntityById(scope, value.typeData.contentId, "Document");
                }
                break;

            case "media":
                if(value.typeData.mediaId) {
                    entity = archetypeCacheService.getEntityById(scope, value.typeData.mediaId, "Media");
                }
                break;

            case "url":
                return value.typeData.url;
                
            default:
                break;

        }

        if(entity) {
            return entity[args.propertyName];
        }

        return "";
    }

    function coreTinyMce(value, scope, args) {

        if(!args.contentLength) {
            args = {contentLength: 50}
        }

        var suffix = "";
        var strippedText = $("<div/>").html(value).text();

        if(strippedText.length > args.contentLength) {
        	suffix = "…";
        }

        return strippedText.substring(0, args.contentLength) + suffix;
    }

	return {
		getFieldsetTitle: function(scope, fieldsetConfigModel, fieldsetIndex) {

            //console.log(scope.model.config);

            if(!fieldsetConfigModel)
                return "";

            var fieldset = scope.model.value.fieldsets[fieldsetIndex];
            var fieldsetConfig = scope.getConfigFieldsetByAlias(fieldset.alias);
            var template = fieldsetConfigModel.labelTemplate;

            if (template.length < 1)
                return fieldsetConfig.label;

            var rgx = /({{(.*?)}})*/g;
            var results;
            var parsedTemplate = template;

            var rawMatches = template.match(rgx);
            
            var matches = [];

            _.each(rawMatches, function(match){
                if(match) {
                    matches.push(match);
                }
            });

            _.each(matches, function (match) {

                // split the template in case it consists of multiple property aliases and/or functions
                var templates = match.replace("{{", '').replace("}}", '').split("|");
                var templateLabelValue = "";

                for(var i = 0; i < templates.length; i++) {
                    // stop looking for a template label value if a previous template part already yielded a value
                    if(templateLabelValue != "") {
                        break;
                    }
                    
                    var template = templates[i];
                    
                    //test for function
                    var beginParamsIndexOf = template.indexOf("(");
                    var endParamsIndexOf = template.indexOf(")");

                    //if passed a function
                    if(beginParamsIndexOf != -1 && endParamsIndexOf != -1)
                    {
                        var functionName = template.substring(0, beginParamsIndexOf);
                        var propertyAlias = template.substring(beginParamsIndexOf + 1, endParamsIndexOf).split(',')[0];

                        var args = {};

                        var beginArgsIndexOf = template.indexOf(',');

                        if(beginArgsIndexOf != -1) {

                            var argsString = template.substring(beginArgsIndexOf + 1, endParamsIndexOf).trim();

                            var normalizedJsonString = argsString.replace(/(\w+)\s*:/g, '"$1":');

                            args = JSON.parse(normalizedJsonString);
                        }

                        templateLabelValue = executeFunctionByName(functionName, window, scope.getPropertyValueByAlias(fieldset, propertyAlias), scope, args);
                    }
                    //normal {{foo}} syntax
                    else {
                        propertyAlias = template;
                        var rawValue = scope.getPropertyValueByAlias(fieldset, propertyAlias);

                        templateLabelValue = rawValue;

                        //determine the type of editor
                        var propertyConfig = _.find(fieldsetConfigModel.properties, function(property){
                            return property.alias == propertyAlias;
                        });

                        if(propertyConfig) {
                        	var datatype = archetypeCacheService.getDatatypeByGuid(propertyConfig.dataTypeGuid);

                        	if(datatype) {

                            	//try to get built-in label
                            	var label = getNativeLabel(datatype, templateLabelValue, scope);

                            	if(label) {
                        			templateLabelValue = label;
                        		}
                        		else {
                        			templateLabelValue = templateLabelValue;
                        		}
                        	}
                        }
                        else {
                        	return templateLabelValue;
                        }

                    }                
                }

                if(!templateLabelValue) {
                    templateLabelValue = "";
                }
                
                parsedTemplate = parsedTemplate.replace(match, templateLabelValue);
            });

            return parsedTemplate;
        }
	}
});
angular.module('umbraco.services').factory('archetypeCacheService', function (archetypePropertyEditorResource) {
    //private

    var isEntityLookupLoading = false;
    var entityCache = [];

    var isDatatypeLookupLoading = false;
    var datatypeCache = [];

    return {
    	getDataTypeFromCache: function(guid) {
        	return _.find(datatypeCache, function (dt){
	            return dt.dataTypeGuid == guid;
	        });
    	},

    	addDatatypeToCache: function(datatype, dataTypeGuid) {
            var cachedDatatype = this.getDataTypeFromCache(dataTypeGuid);

            if(!cachedDatatype) {
            	datatype.dataTypeGuid = dataTypeGuid;
            	datatypeCache.push(datatype);
            }
    	},
 
		getDatatypeByGuid: function(guid) {
			var cachedDatatype = this.getDataTypeFromCache(guid);

	        if(cachedDatatype) {
	            return cachedDatatype;
	        }

	        //go get it from server, but this should already be pre-populated from the directive, but I suppose I'll leave this in in case used ad-hoc
	        if (!isDatatypeLookupLoading) {
	            isDatatypeLookupLoading = true;

	            archetypePropertyEditorResource.getDataType(guid).then(function(datatype) {

	            	datatype.dataTypeGuid = guid;

	                datatypeCache.push(datatype);

	                isDatatypeLookupLoading = false;

	                return datatype;
	            });
	        }

	        return null;
        },

     	getEntityById: function(scope, id, type) {
	        var cachedEntity = _.find(entityCache, function (e){
	            return e.id == id;
	        });

	        if(cachedEntity) {
	            return cachedEntity;
	        }

	        //go get it from server
	        if (!isEntityLookupLoading) {
	            isEntityLookupLoading = true;

	            scope.resources.entityResource.getById(id, type).then(function(entity) {

	                entityCache.push(entity);

	                isEntityLookupLoading = false;

	                return entity;
	            });
	        }

	        return null;
	    }
    }
});