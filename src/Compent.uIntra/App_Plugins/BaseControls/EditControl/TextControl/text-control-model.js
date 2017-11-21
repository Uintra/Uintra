function TextControlModel(mode) {
    this.mode = mode ? mode : ControlMode.disable;
    this.inputType = InputType.text;
    this.isRequired = false;
    this.requiredValidationMessage = '';
    this.maxLength = 200;
    this.maxLengthValidationMessage = '';
    this.isValidationRequired = true;
    this.onSave = function () { };
    this.triggerRefresh = function () { };
    this.triggerValidate = function () { };
    this.triggerCopySavedData = function () { };
    this.triggerCheckControlChanged = function () { };
}