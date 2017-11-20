function TextAreaControlModel(mode) {
    this.mode = mode ? mode : ControlMode.disable;
    this.maxLength = 400;
    this.isValidationRequired = true;
    this.onSave = function () { };
    this.triggerRefresh = function () { };
    this.triggerValidate = function () { };
    this.triggerCopySavedData = function () { };
    this.triggerCheckControlChanged = function () { };
}