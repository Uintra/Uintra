function panelContainerController(
    $scope,
    editorState,
    panelContainerResource,
    contentResource,
    editorService,
    $routeParams,
    $q,
    $timeout,
    formHelper,
    serverValidationManager)
{
    var vm = this;
    var currentNode = editorState.getCurrent();
    vm.nextOrderIndex = 0;
    vm.selectedPanels = [];

    $scope.sortableOptions = {
        axis: 'y',
        cancel: '.panel',
        update: (e, ui) => {
            let from = ui.item.sortable.index;
            let to = ui.item.sortable.dropindex;
            move(from, to);
            renderSelected();
        }
    }

    vm.allowDrag = (isAllow) => {
        $scope.sortableOptions.cancel = isAllow ?  '' : '.panel';
    }

    vm.removePanel = function(panel)
    {
        $scope.model.value.localPanels.forEach((existedPanel, idx) => {
            let found = existedPanel.order === panel.order;
            if (found) $scope.model.value.localPanels.splice(idx, 1)
        });

        $scope.model.value.globalPanels.forEach((existedPanel, idx) => {
            let found = existedPanel.order === panel.order;
            if (found) $scope.model.value.globalPanels.splice(idx, 1)
        });

        renderSelected();
    }

    vm.editPanel = function(panel)
    {
        panel.editMode = true;
        vm.setupPanelEditorFor(panel, false, panel.isGlobal);
    }

    vm.showLocalPanelList = function()
    {
        var options = {
            title: 'Local panels',
            view: '/App_Plugins/PanelContainer/custom-editor.html',
            size: 'small',
            panels: vm.localPanelTypes,
            selectPanel: panel => {
               vm.setupPanelEditorFor(panel, true);
            },
            close: () => editorService.close()
        }

        editorService.open(options);
    }

    vm.showGlobalPanelList = function()
    {
        getGlobalPanelTypes().then(response => {
            let panels = response.map(data => {
                data.isGlobal = true;
                return data;
            });

            var options = {
                title: 'Global panels',
                view: '/App_Plugins/PanelContainer/custom-editor.html',
                size: 'small',
                panels: panels,
                selectPanel: panel => {
                    $scope.model.value.globalPanels.push({
                        nodeId: panel.nodeId,
                        order: vm.nextOrderIndex++
                    });

                    vm.selectedPanels.push(panel)
                    renderSelected();
                    editorService.close();
                },
                close: () => editorService.close()
            }

            editorService.open(options);
        })
    }

    vm.setupPanelEditorFor = function (panel, isNew, isGlobal)
    {
        if (isGlobal)
        {
            const editor = {
                id: panel.nodeId,
                create: false,
                contentTypeAlias: panel.alias,
                animating: true,
                close: model => {
                    editorService.closeAll();
                }
            }

            editorService.contentEditor(editor);
            return;
        }

        if (isNew)
        {
            setupPanelEditorForNew(panel).then(editor => {
                vm.showPanelEditor(editor.title, editor.alias, editor.tabs, isNew, isGlobal, saveNewPanelMethod);
            });
        } else {
            setupPanelEditorForExisted(panel).then(editor => {
                vm.showPanelEditor(editor.title, editor.alias, editor.tabs, isNew, isGlobal, saveExistedPanelMethod);
            });
        }
    }

    vm.showPanelEditor = function(title, contentTypeAlias, tabs, isNew, isGlobal, saveMethod)
    {
        let options = {
            title: title,
            contentTypeAlias: contentTypeAlias,
            view: '/App_Plugins/PanelContainer/panel-editor.html',
            tabs:  mapPropertyAliasForValidation(tabs, {prefix: contentTypeAlias}),
            isGlobal: isGlobal,
            isNew: isNew,
            animating: true,
            save: (model, formCtrl) => {
                if (!angular.isFunction(saveMethod)) throw new Error(`Provide save method`);
                // It's important. Many editors fills their models on 'formSubmitting' event.
                serverValidationManager.reset();
                $scope.$root.$broadcast('formSubmitting');
                $timeout(() => {
                    validate(model).then(result => {
                        $scope.$evalAsync(() => {
                            if (!result.isValid)
                            {
                                formHelper.handleServerValidation(result.ModelState);
                                return;
                            }

                            if (formCtrl.$invalid) return;

                            saveMethod(model);
                            renderSelected();
                            editorService.closeAll();
                        });

                    })
                }, 50, true);
            },
            close: () => {
                editorService.close();
            }
        }

        editorService.open(options);
    }

    function validate(model) {
        return new Promise(resolve => {

            let result = {isValid: true, ModelState: {}};
            const promises = model.tabs.map(tab => {
                const innerPromises = tab.properties.map(async property => {
                    if (property.validation.mandatory)
                    {
                        const error = await validateAsRequired(property.value, property.alias, property.editor);
                        if (error) return Object.assign(result.ModelState, error);
                    }

                    if (property.validation.pattern && property.validation.pattern.length)
                    {
                        const error = validateByPattern(property.value, property.alias, property.validation.pattern)
                        if (!error) Object.assign(result.ModelState, error)
                    }
                });

                return $q.all(innerPromises);
            });

            $q.all(promises).then(resolved => {
                if (Object.keys(result.ModelState).length > 0) result.isValid = false;
                resolve(JSON.parse(angular.toJson(result)));
            });
        });
    }

    function validateAsRequired(value, alias, editor) {
        return new Promise(resolve => {

            let isValid = true;

            if (angular.isString(value))
            {
                if (value.length) return resolve();
                return resolve(formatValidationError(alias, "Required"));
            }

            if (isNestedContent(value))
            {
                validateNestedContent(value, alias).then(result => resolve(result));
            } else {
                if ('UBaseline.ImagePicker'.toLocaleLowerCase() === editor.toLowerCase())
                {
                    isValid = value && value.image && !!value.image.mediaId;
                    if (!isValid)
                    {
                        return resolve(formatValidationError(alias, "Required"));
                    }
                }

                isValid = value !== null && angular.isDefined(value) && value.toString().length > 0;
                if (!isValid) return resolve(formatValidationError(alias, "Value cannot be empty"));

                return resolve();
            }
        })
    }

    function validateNestedContent(nestedContent, parent)
    {
       return new Promise(resolve => {
        contentResource.getScaffold($routeParams.id, nestedContent[0].ncContentTypeAlias).then(scaffold => {
            nestedContent.forEach((nested, index) => {
                scaffold.variants[0].tabs[0].properties.forEach(property => {

                    const input = $(`#${parent}___${property.alias}`).removeClass('custom-invalid');
                    if (property.validation.mandatory && !nested[property.alias])
                    {
                        input.addClass('custom-invalid');

                        const validation = formatValidationError(parent, `Item ${index + 1} '${property.label}' cannot be empty`);
                        if (!scaffold.ModelState[Object.keys(validation)[0]]) scaffold.ModelState[Object.keys(validation)[0]] = [];

                        scaffold.ModelState[Object.keys(validation)[0]].push(validation[Object.keys(validation)[0]].pop());
                        return;
                    }
                });
            })

            resolve(scaffold.ModelState);
        });
       })
    }

    function isNestedContent(value)
    {
        const isArray = Array.isArray(value);
        return isArray && value[0] && !!value[0]['ncContentTypeAlias'];
    }

    function validateByPattern(value, alias, pattern)
    {
        try {
            let isValid = new RegExp(pattern).test(value);
            if (!isValid) return formatValidationError(alias, `Value is invalid, it does not match the correct pattern. Pattern: ${pattern}`);
        } catch (err) {
            console.error(`Invalid reqular expression ${pattern}`);
        }
    }

    function formatValidationError(alias, message)
    {
        let result = {};
        result[`_Properties.${alias}..value`] = [message];

        return result;
    }

    function setupPanelEditorForNew(panel)
    {
        return contentResource.getScaffold($routeParams.id, panel.alias).then(scaffold => {
            return {
                title: scaffold.contentTypeName,
                alias: scaffold.contentTypeAlias,
                tabs: scaffold.variants[0].tabs
            };
        })
    }

    function mapPropertyAliasForValidation(tabs, options)
    {
        if (!options)
        {
            options = {
                prefix: new Date(Date.now()).getTime()
            }
        }
        return tabs.map(tab => {
            tab.properties.forEach(property => {
                property.alias = `${options.prefix}__${property.alias}`
            });

            return tab;
        });
    }

    function normalizePropertyAliasForSave(property)
    {
       return property.alias.split('__').pop();
    }

    function setupPanelEditorForExisted(panel)
    {
       return setupPanelEditorForNew(panel).then(scaffold => {
           const existed = [...$scope.model.value.globalPanels, ...$scope.model.value.localPanels].find(p => p.order === panel.order);

           if (!existed) throw new Error(`Panel doesn't exist`);

           scaffold.tabs.forEach(tab => {
               tab.properties.forEach(property => {
                    const existedProperty = existed.properties.find(existed => existed.alias === property.alias);

                    if (!existedProperty) return;

                    property.value = existedProperty.value;
               });
           });

           return scaffold;
       });
    }

    function saveNewPanelMethod (model)
    {
        const saveModel = panelEditorPropertiesToSaveModel({
            properties: model.tabs.reduce((acc, next) => [...acc, ...next.properties], []),
            contentTypeAlias: model.contentTypeAlias
        });

        model.isGlobal ?
            $scope.model.value.globalPanels.push(saveModel) :
            $scope.model.value.localPanels.push(saveModel);
    }

    function saveExistedPanelMethod(model)
    {
        let panelVm = vm.selectedPanels.find(p => p.editMode);
        const saveModel = panelEditorPropertiesToSaveModel({
            properties: model.tabs.reduce((acc, next) => [...acc, ...next.properties], []),
            contentTypeAlias: model.contentTypeAlias
        }, {reuseOrder: panelVm.order});

        if (model.isGlobal)
        {

        } else {
            let existModel = $scope.model.value.localPanels.find(p => p.order === panelVm.order);
            if (!existModel) throw new Error(`Can't find panel in exited model`);

            if (existModel)
            {
                existModel.properties = saveModel.properties;
            }
        }
    }

    function move(from, to)
    {
        let allPanels = [...$scope.model.value.globalPanels, ...$scope.model.value.localPanels]
            .sort((a, b) => a.order - b.order);
        let affected = allPanels.splice(from, 1);

        // insert
        allPanels.splice(to, 0, affected[0]);

        // update order
        allPanels.forEach((p, idx) => p.order = idx);
    }
    function panelEditorPropertiesToSaveModel(model, options) {
        const properties = model.properties.map(prop => {
            return {
                alias: normalizePropertyAliasForSave(prop),
                value: prop.value,
                propertyEditorAlias: prop.editor
            }
        });

        return {
            contentTypeAlias: model.contentTypeAlias,
            order: (options && options.reuseOrder !== undefined) ? options.reuseOrder : vm.nextOrderIndex++,
            properties: properties
        }
    }

    function getPanels()
    {
        var nodeId = currentNode.id || currentNode.parentId;
        return $q.all([
            panelContainerResource.getLocalPanelTypes(currentNode.contentTypeAlias),
            panelContainerResource.getGlobalPanelTypes(currentNode.contentTypeAlias, nodeId),
            panelContainerResource.getRequiredPanelTypes(currentNode.contentTypeAlias)
        ])
    }

    function getGlobalPanelTypes()
    {
        var nodeId = currentNode.id || currentNode.parentId;
        return panelContainerResource.getGlobalPanelTypes(currentNode.contentTypeAlias, nodeId);
    }

    function init()
    {
        if ($scope.model.value === undefined) return;

        if (!$scope.model.value.globalPanels && !$scope.model.value.localPanels)
        {
            $scope.model.value = {globalPanels: [], localPanels: []}
        }

        vm.loading = true;

        getPanels().then(panels => {
            vm.localPanelTypes = panels[0];
            vm.globalPanelTypes = panels[1];
            vm.requiredPanelTypes = panels[2];

            try {
                renderSelected();
            } catch (err) {
                debugger
            }

            vm.loading = false;
        });
    }

    function updateModelOrders()
    {
        let allPanels = [...$scope.model.value.localPanels, ...$scope.model.value.globalPanels];
        let sorted = allPanels.sort((a, b) => a.order - b.order);
        let updatedValue = {localPanels: [], globalPanels: []}
        sorted.forEach((panel, idx) => {
            panel.order = idx;
            isGlobal(panel) ? updatedValue.globalPanels.push(panel) : updatedValue.localPanels.push(panel);
        });

        $scope.model.value = updatedValue;
    }

    function renderSelected()
    {
        updateModelOrders();
        let allPanels = [...$scope.model.value.localPanels, ...$scope.model.value.globalPanels];
        allPanels = allPanels.sort((a, b) => a.order - b.order);
        vm.selectedPanels = panelModelsToViewModels(allPanels);
    }

    function panelModelsToViewModels(panels)
    {
        let vms = panels.map(panel => {
            let panelType;

            if (isGlobal(panel))
            {
                let existingPanel = vm.globalPanelTypes.find(type => type.nodeId === panel.nodeId);
                if (existingPanel) {
                    panelType = JSON.parse(angular.toJson(existingPanel));
                }
                if (!panelType) {
                    const idx = $scope.model.value.globalPanels.indexOf(panel);
                    $scope.model.value.globalPanels.splice(idx, 1);
                    return;
                } else {
                    panelType.isGlobal = true;
                }
            } else {
                let obj = vm.localPanelTypes.find(type => type.alias === panel.contentTypeAlias);
                if (obj) panelType = JSON.parse(angular.toJson(obj));
            }

            if (!panelType) removePanelFromModel(panel);

            return panelType;
        })
        .filter(panel => !!panel);

        return updateOrdersFor(vms);
    }

    function removePanelFromModel(panel)
    {
        debugger
        // panel has been removed or renamed
        if (panel.contentTypeAlias)
        {
            $scope.model.value.localPanels = $scope.model.value.localPanels.filter(panel => panel.contentTypeAlias !== panel.contentTypeAlias);
        } else {
            $scope.model.value.globalPanels = $scope.model.value.globalPanels.filter(panel => panel.nodeId !== panel.nodeId);
        }
    }

    function updateOrdersFor(panels)
    {
        if (!(panels instanceof Array)) return;

        let updated = panels.map((panel, idx) => {
            panel.order = idx;
            return panel;
        });

        vm.nextOrderIndex = updated.length;

        return updated;
    }

    function isGlobal(panel)
    {
        return panel.nodeId !== undefined;
    }

    init();
}
angular.module("umbraco").controller("UBaseline.PanelContainerController", panelContainerController);