(function (angular) {
    'use strict';

    var controller = function ($rootScope, $scope, $location, appState, notificationsService, notificationSettingsService, notificationSettingsConfig, navigationService) {

        var self = this;
        self.settings = {};
        self.selectedNotifierSettings = {};

        const notifierType = {
            email: 1,
            ui: 2
        };

        let selectedNotifierType;


        self.isEmailTabSelected = function () {
            return selectedNotifierType === notifierType.email;
        }

        self.isUiTabSelected = function () {
            return selectedNotifierType === notifierType.ui;
        }

        self.isPopupTabSelected = function () {
            return selectedNotifierType === notifierType.popup;
        }

        self.selectEmailTab = function () {
            selectedNotifierType = notifierType.email;
            self.selectedNotifierSettings = self.settings.emailNotifierSetting;
        }

        self.selectPopupTab = function () {
            if (!self.settings.emailNotifierSetting && !self.settings.uiNotifierSetting) {
                selectedNotifierType = notifierType.popup;
                self.selectedNotifierSettings = self.settings.popupNotifierSetting;
            }
        }

        self.selectUiTab = function () {
            selectedNotifierType = notifierType.ui;
            self.selectedNotifierSettings = self.settings.uiNotifierSetting;
        }

        self.save = function () {
            saveSettings(self.settings);
        }

        self.splitOnUpperCaseCharacters = function (text) {
            if (!text || text.length === 0) return '';
            return text.split(/(?=[A-Z])/).join(' ');
        }

        function initalize() {
            initLocationChangeStartEvent();
            initCurrentNodeHighlighting();

            var params = getCurrentUrlParams();
            notificationSettingsService.getSettings(params.activityType, params.notificationType).then(
                function (result) {
                    self.settings = result.data;

                    if (self.settings.emailNotifierSetting != null) {
                        self.selectEmailTab();
                        initEmailControlConfig();
                    }

                    if (self.settings.uiNotifierSetting != null) {
                        initUiMessageControlConfig();
                        initDesktopMessageControlConfig();
                        initDesktopTitleControlConfig();
                    }

                    if (self.settings.popupNotifierSetting != null) {
                        self.selectPopupTab();
                        initPopupMessageControlConfig();
                    }
                },
                showGetErrorMessage);
        }

        function getUrlParams(url) {
            var params = {};
            (url + '?').split('?')[1].split('&').forEach(function (pair) {
                pair = (pair + '=').split('=').map(decodeURIComponent);
                if (pair[0].length) {
                    params[pair[0]] = pair[1];
                }
            });
            return params;
        };

        function getCurrentUrlParams() {
            var params = $location.search();

            if (angular.equals(params, {})) {
                params = getUrlParams($location.path());
            }
            return params;
        }

        function initCurrentNodeHighlighting() {
            var queryString = getCurrentUrlParams();
            var parentId = queryString.activityType;
            var currentNodeId = queryString.id;
            navigationService.syncTree({ tree: 'NotificationSettingsTree', path: ["-1", parentId, currentNodeId], forceReload: false });
        }

        function saveSettings(settings) {
            if (self.isEmailTabSelected()) {
                notificationSettingsService.seveEmailSettings(settings.emailNotifierSetting).then(showSaveSuccessMessage, showSaveErrorMessage);
                return;
            }
            if (self.isUiTabSelected()) {
                console.log(settings.uiNotifierSetting);
                notificationSettingsService.seveUiSettings(settings.uiNotifierSetting).then(showSaveSuccessMessage, showSaveErrorMessage);
                return;
            }

            if (self.isPopupTabSelected()) {
                notificationSettingsService.sevePopupSettings(settings.popupNotifierSetting).then(showSaveSuccessMessage, showSaveErrorMessage);
                return;
            }
        }

        function showGetErrorMessage() {
            notificationsService.error("Error", "Notification settings were not loaded, because some error has occurred");
        }

        function showSaveSuccessMessage() {
            notificationsService.success("Success", "Notification settings were updated successfully");
        }

        function showSaveErrorMessage() {
            notificationsService.error("Error", "Notification settings were not updated, because some error has occurred");
        }

        function initEmailControlConfig() {
            initEmailSubjectControlConfig();
            initEmailBodyControlConfig();
        }

        function initEmailSubjectControlConfig() {
            self.emailSubjectControlConfig = new TextControlModel(ControlMode.view);
            self.emailSubjectControlConfig.value = self.settings.emailNotifierSetting.template.subject;

            self.emailSubjectControlConfig.isRequired = true;
            self.emailSubjectControlConfig.requiredValidationMessage = 'E-mail subject is required';
            self.emailSubjectControlConfig.maxLength = 400;
            self.emailSubjectControlConfig.maxLengthValidationMessage = 'E-mail subject max length is 400 symbols';

            self.emailSubjectControlConfig.onSave = function (emailSubject) {
                self.settings.emailNotifierSetting.template.subject = emailSubject;
                self.save();
            };

            self.emailSubjectControlConfig.triggerRefresh();
        }

        function initEmailBodyControlConfig() {
            self.emailBodyControlConfig = new RichTextEditorModel(ControlMode.view);
            self.emailBodyControlConfig.tinyMceOptions = notificationSettingsConfig.emailMessageTinyMceOptions;
            self.emailBodyControlConfig.value = self.settings.emailNotifierSetting.template.body;

            self.emailBodyControlConfig.isRequired = true;
            self.emailBodyControlConfig.requiredValidationMessage = 'E-mail content is required';
            self.emailBodyControlConfig.maxLength = 4000;
            self.emailBodyControlConfig.maxLengthValidationMessage = 'E-mail content max length is 4000 symbols';

            self.emailBodyControlConfig.onSave = function (emailBody) {
                self.settings.emailNotifierSetting.template.body = emailBody;
                self.save();
            };

            self.emailBodyControlConfig.triggerRefresh();
        }

        function initUiMessageControlConfig() {
            self.uiMessageControlConfig = new RichTextEditorModel(ControlMode.view);
            self.uiMessageControlConfig.value = self.settings.uiNotifierSetting.template.message;

            self.uiMessageControlConfig.tinyMceOptions = notificationSettingsConfig.uiMessageTinyMceOptions;

            self.uiMessageControlConfig.isRequired = true;
            self.uiMessageControlConfig.requiredValidationMessage = 'In-App message is required';
            self.uiMessageControlConfig.maxLength = 200;
            self.uiMessageControlConfig.maxLengthValidationMessage = 'In-App message max length is 200 symbols';

            self.uiMessageControlConfig.onSave = function (uiMessage) {
                self.settings.uiNotifierSetting.template.message = uiMessage;
                self.save();
            };

            self.uiMessageControlConfig.triggerRefresh();
        }

        function initDesktopMessageControlConfig() {
            self.desktopMessageControlConfig = new TextAreaControlModel(ControlMode.view);
            self.desktopMessageControlConfig.value = self.settings.uiNotifierSetting.template.desktopMessage;

            self.desktopMessageControlConfig.isRequired = true;
            self.desktopMessageControlConfig.requiredValidationMessage = 'Desktop message is required';
            self.desktopMessageControlConfig.maxLength = 200;
            self.desktopMessageControlConfig.maxLengthValidationMessage = 'Desktop message max length is 200 symbols';

            self.desktopMessageControlConfig.onSave = function (desktopMessage) {
                self.settings.uiNotifierSetting.template.desktopMessage = desktopMessage;
                self.save();
            };

            self.emailSubjectControlConfig.triggerRefresh();
        }

        function initDesktopTitleControlConfig() {

            self.desktopTitleControlConfig = new TextControlModel(ControlMode.view);
            self.desktopTitleControlConfig.value = self.settings.uiNotifierSetting.template.desktopTitle;

            self.desktopTitleControlConfig.isRequired = true;
            self.desktopTitleControlConfig.requiredValidationMessage = 'Desktop title is required';
            self.desktopTitleControlConfig.maxLength = 100;
            self.desktopTitleControlConfig.maxLengthValidationMessage = 'Desktop title max length is 100 symbols';

            self.desktopTitleControlConfig.onSave = function (desktopTitle) {
                self.settings.uiNotifierSetting.template.desktopTitle = desktopTitle;
                self.save();
            };

            self.desktopTitleControlConfig.triggerRefresh();
        }

        function initPopupMessageControlConfig() {
            self.popupMessageControlConfig = new RichTextEditorModel(ControlMode.view);
            self.popupMessageControlConfig.value = self.settings.popupNotifierSetting.template.message;

            self.popupMessageControlConfig.tinyMceOptions = notificationSettingsConfig.popupMessageTinyMceOptions;

            self.popupMessageControlConfig.isRequired = true;
            self.popupMessageControlConfig.requiredValidationMessage = 'Popup message is required';
            self.popupMessageControlConfig.maxLength = 2000;
            self.popupMessageControlConfig.maxLengthValidationMessage = 'Popup message max length is 2000 symbols';

            self.popupMessageControlConfig.onSave = function (uiMessage) {
                self.settings.popupNotifierSetting.template.message = uiMessage;
                self.save();
            };

            self.popupMessageControlConfig.triggerRefresh();
        }

        function initLocationChangeStartEvent() {
            $rootScope.$on('$locationChangeStart', function () {
                var settingsForm = $scope.settingsForm;
                if (!settingsForm) return;
                settingsForm.$setPristine();
                var activeEditControls = angular.element(document.querySelectorAll('form[name="' + settingsForm.$name + '"] .js-active-edit-control'));
                if (activeEditControls.length > 0) {
                    settingsForm.$setDirty(); // for showing umbraco confirmation popup
                }
            });
        }

        initalize();
    }

    controller.$inject = ['$rootScope', '$scope', '$location', 'appState', 'notificationsService', 'notificationSettingsService', 'notificationSettingsConfig', 'navigationService'];

    angular.module('umbraco').controller('notificationSettingController', controller);
})(angular);