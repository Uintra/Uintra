import appInitializer from "./../Core/Content/scripts/AppInitializer";

require("./taggle.css");
require("./tags.css");

var Taggle = require("taggle");

require('devbridge-autocomplete');

function initTagsControl() {
    var holder = $('.js-activity-tags-holder');
    if (!holder.length) return;

    var activityTagsProperty = holder.data('activity-tags');
    var activityTags = $.map(activityTagsProperty, function (tag) { return tag.text; });

    var existedTags = $.map(activityTagsProperty, function (tag) {
        return { value: tag.text, id: tag.id };
    });

    var tagIndex = 0;
    var modelName = holder.data('model-name');

    var tagsControl = new Taggle(holder[0],
        {
            preserveCase: true,
            placeholder: holder.data('placeholder'),
            hiddenInputName: modelName,
            tags: activityTags,
            tagFormatter: function (element) {
                var oldHiddenInput = $(element).find("input");
                var hiddenValue = oldHiddenInput.val();
                var existedTag = existedTags.filter(function (tag) { return tag.value === hiddenValue })[0];

                var hiddenTagIndex = document.createElement('input');
                hiddenTagIndex.type = 'hidden';
                hiddenTagIndex.value = tagIndex;
                hiddenTagIndex.name = modelName + '.Index';

                var hiddenTagId = document.createElement('input');
                hiddenTagId.type = 'hidden';
                hiddenTagId.value = existedTag ? existedTag.id : null;
                hiddenTagId.name = modelName + '[' + tagIndex + '].Id';

                var hiddenTagText = document.createElement('input');
                hiddenTagText.type = 'hidden';
                hiddenTagText.value = hiddenValue;
                hiddenTagText.name = modelName + '[' + tagIndex + '].Text';

                oldHiddenInput.remove();
                $(element).append(hiddenTagIndex);
                $(element).append(hiddenTagId);
                $(element).append(hiddenTagText);

                tagIndex++;
            }
        });

    var tagsControlInput = tagsControl.getInput();

    $(tagsControlInput).devbridgeAutocomplete({
        serviceUrl: '/umbraco/surface/Tags/Autocomplete',
        paramName: 'query',
        minChars: 3,
        dataType: 'json',
        transformResult: function (response, originalQuery) {
            var result = {
                suggestions: $.map(response.Tags, function (dataItem) {
                    return { value: dataItem.Text, id: dataItem.Id };
                })
            };

            return result;
        },
        formatResult: function (suggestion, currentValue) {
            return "<span>" + suggestion.value + "</span>";
        },
        onSelect: function (suggestion) {
            existedTags.push(suggestion);
            tagsControl.add(suggestion.value);
        }
    });
}

var controller = {
    init: function() {
        initTagsControl();
    }
}

export default controller;