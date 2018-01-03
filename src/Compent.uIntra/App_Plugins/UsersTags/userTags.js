const tagControlHolderSelector = '.js-user-tag-picker-holder';
const tagPickerControlSelector = '.js-user-tags-picker';
const userTagCollectionSelector = '#userTagCollection';
const selectedTagIdsCollectionSelector = '#selectedTagIds';

let controller = {
    init: function (holder) {
        let allTags = loadTagCollection(holder);
        let availableForPickingTags = allTags;

        let tagPickerControl = initControl(holder);
        let selectedTagsDataControl = getSelectedTagsInput(holder);
        setInitialSelectedTags(tagPickerControl, selectedTagsDataControl);

        initListeners();

        function initControl(holder) {
            let container = holder.find(tagPickerControlSelector).first();
            let placeholder = $(tagPickerControlSelector).attr('placeholder');

            let tagPickerControl = $(container).select2({
                data: availableForPickingTags,
                placeholder: placeholder,
                allowClear: true,
                width: '100%'
            });
            return tagPickerControl;
        }

        function setInitialSelectedTags(tagPickerControl, selectedTagsDataControl) {
            let selectedTags = selectedTagsDataControl.val().split(",");
            tagPickerControl.val(selectedTags).change();
        }

        function loadTagCollection(holder) {
            let json = holder.find(userTagCollectionSelector).first().val();
            return JSON.parse(json);
        }

        function initListeners() {
            tagPickerControl.on('change', updateSelectedTags);
        }

        function updateSelectedTags() {
            let selectedTagIds = tagPickerControl.val();
            selectedTagsDataControl.val(selectedTagIds);
        }

        function getSelectedTagsInput(holder) {
            return holder.find(selectedTagIdsCollectionSelector).first();
        }
    }
}

function init() {
    let pickerHolders = $(tagControlHolderSelector);
    pickerHolders.each((i, el) => controller.init($(el)));

    //prevent opening dropdown when deleting tag
    pickerHolders.on("select2:unselect", function (evt) {
        if (!evt.params.originalEvent) {
            return;
        }

        evt.params.originalEvent.stopPropagation();
    });
}


export default init;