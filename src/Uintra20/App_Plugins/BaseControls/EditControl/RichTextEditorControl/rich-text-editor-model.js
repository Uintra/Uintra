function RichTextEditorModel(mode) {
    this.mode = mode ? mode : ControlMode.disable;
    this.value = '';
    this.isRequired = false;
    this.requiredValidationMessage = '';
    this.maxLength = 4000;
    this.maxLengthValidationMessage = '';
    this.tinyMceOptions = {};
    this.onSave = function () { };
    this.triggerRefresh = function () { };
    this.triggerValidate = function () { };
    this.triggerCopySavedData = function () { };
    this.triggerCheckControlChanged = function () { };
}