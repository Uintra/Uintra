var Taggle = require("taggle");

require('devbridge-autocomplete');

var TagsController = {
    init: function () {
        var activityTagsContainer = $('#activityTags');

        var activityTags;
        var activityTagsProperty = activityTagsContainer.data('activity-tags');
        if (activityTagsProperty && Array.isArray(activityTagsProperty)) {
            activityTags = activityTagsProperty.split(',');
        } else {
            activityTags = [activityTagsProperty];
        }

        var tagsControl = new Taggle(activityTagsContainer[0], {
            preserveCase: true,
            placeholder : activityTagsContainer.data('placeholder'),
            hiddenInputName : activityTagsContainer.data('model-name'),
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
}

export default TagsController;