import appInitializer from "./../Core/Content/scripts/AppInitializer";

require("./tags.css");

var Taggle = require("taggle");

require('devbridge-autocomplete');

function initTagsControl() {
    var holder = $('#activityTagsHolder');
    if (!holder.length) return;

    var activityTags = [];
    var activityTagsProperty = holder.data('activity-tags');
    if (activityTagsProperty) {
        if ((activityTagsProperty + '').indexOf(',') !== -1) {
            activityTags = activityTagsProperty.split(',');
        } else {
            activityTags = [activityTagsProperty];
        }
    }

    var tagsControl = new Taggle(holder[0], {
        preserveCase: true,
        placeholder : holder.data('placeholder'),
        hiddenInputName : holder.data('model-name'),
        tags: activityTags
    });

    var tagsControlInput = tagsControl.getInput();

    $(tagsControlInput).devbridgeAutocomplete({
        serviceUrl: '/umbraco/surface/Tags/TagsAutocomplete',
        paramName: 'query',
        minChars: 3,
        dataType: 'json',
        transformResult: function (response, originalQuery) {
            var result = {
                suggestions: $.map(response.Tags, function (dataItem) {
                    return { value: dataItem };
                })
            };

            return result;
        },
        formatResult: function (suggestion, currentValue) {
            return "<span>" + suggestion.value + "</span>";
        },
        onSelect: function (suggestion) {
            tagsControl.add(suggestion.value);
        }
    });
}

appInitializer.add(function () {
    initTagsControl();
});