function mediaPickerController($scope, $timeout) {
    vm = this;
    vm.showImagePicker = false;
    vm.showVideoPicker = false;

    vm.items = [
        {name: 'Image', type: 'image'},
        {name: 'Video', type: 'video'}
    ];

    init($scope.model);

    vm.select = (item) => {
        vm.selectedPickerLabel = item.name;
        if ($scope.model.value === null)
        {
            $scope.model.value = {image: null, video: null};
        }
        if (item.type === 'image')
        {
            vm.showImagePicker = true;
            vm.showVideoPicker = false;
            $scope.model.value.video = null;
        } else {
            vm.showImagePicker = false;
            vm.showVideoPicker = true;
            $scope.model.value.image = null;
        }
    }

    vm.dropdownClose = () => vm.dropdownOpen = false;

    function init(model)
    {
        model.value = getValue(model);
        showEditorForValue(model.value);

        $scope.$on('formSubmitting', () => {
            if ($scope.model.value === null) return;
            if ($scope.model.value && !$scope.model.value.image && !$scope.model.value.video) $scope.model.value = null;
        })
    }

    function getValue(model)
    {
        if (angular.isString(model.value) || !angular.isDefined(model.value)) return getDefaultValue();

        return model.value;
    }

    function showEditorForValue(value)
    {
        vm.selectedPickerLabel = 'Select picker';
        vm.showImagePicker = false;
        vm.showVideoPicker = false;

        if (!value) return;
        if (value.image) showPicker('image');
        if (value.video) showPicker('video');
    }
    function getDefaultValue() {return {image: null, video: null}};

    function showPicker(type)
    {
        if (type === 'image')
        {
            vm.showImagePicker = true;
            vm.showVideoPicker = false;
            vm.selectedPickerLabel = vm.items.find(i => i.type === 'image').name;
            return;
        }

        vm.showImagePicker = false;
        vm.showVideoPicker = true;
        vm.selectedPickerLabel = vm.items.find(i => i.type === 'video').name;
    }
}
angular.module("umbraco").controller("UBaseline.MediaPickerController", mediaPickerController);