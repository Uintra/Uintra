require("./../Core/Content/libs/jquery.validate.min.js");
require("./../Core/Content/libs/jquery.unobtrusive-ajax.min.js");
require("./../Core/Content/libs/jquery.validate.unobtrusive.min.js");

import appInitializer from "./../Core/Content/scripts/AppInitializer";
import helpers from "./../Core/Content/scripts/Helpers";

require("./_comments.css");

var CommentOverview = function (selector) {
    var commentOverviewQuillOptions = {
        theme: 'snow',
        modules: {
            toolbar: [['bold', 'italic', 'underline'], ['link']]
        }
    };

    var holders = $(selector);
    if (!holders || holders.length === 0) {
        return;
    }
    $.validator.unobtrusive.parse(selector);
    holders.each(function () {
        var $this = $(this);
        initCreateControl($this);
        $this.find('[id^=js-comment-view-]').each(function () {
            new Comment(this);
        });
    });

    function initCreateControl(holder) {
        var createControls = holder.find('.js-comment-create');
        createControls.each(function () {
            var $this = $(this);
            var button = $this.find('.js-comment-create-btn');

            if ($this.data('parentid')) {
                return true;
            }

            $this.on('submit', function () {
                $this.valid();
            });

            var dataStorage = $this.find('.js-hidden-comment-create-description')[0];
            var descriptionElem = $this.find('.js-comment-create-description')[0];
            var quill = helpers.initQuill(descriptionElem, dataStorage, commentOverviewQuillOptions);
            quill.on('text-change', function () {
                if (quill.getLength() > 1) {
                    button.removeAttr("disabled");
                } else {
                    button.attr("disabled", "disabled");
                }
            });

            quill.setText('');
            dataStorage.value = '';
        });
    }
}

var Comment = function (selector) {
    var commentQuillOptions = {
        theme: 'snow',
        modules: {
            toolbar: [['bold', 'italic', 'underline'], ['link']]
        }
    };

    var holders = $(selector);
    if (!holders || holders.length === 0) {
        return;
    }

    holders.each(function () {
        var $this = $(this);
        initEdit($this);
        initReply($this);
        initDelete($this);
    });

    function initEdit(holder) {
        var editlink = findControl(holder, '.js-comment-editlink');
        var hideEditlink = findControl(holder, '.js-comment-hideEditLink');

        if (editlink.length === 0 || hideEditlink.length === 0) {
            return;
        }

        var editControlContainer = findControl(holder, '.js-comment-editContainer');
        var descriptionControl = findControl(holder, '.js-comment-description');

        editlink.on('click', function () {
            editlink.hide();
            hideEditlink.show();
            descriptionControl.hide();
            editControlContainer.show();
            
        });

        hideEditlink.on('click', function () {
            editlink.show();
            hideEditlink.hide();
            descriptionControl.show();
            editControlContainer.hide();
        });

        var dataStorage = findControl(holder, '.js-hidden-comment-edit-description')[0];
        var descriptionElem = findControl(holder, '.js-comment-edit-description')[0];
        var quill = helpers.initQuill(descriptionElem, dataStorage, commentQuillOptions);
        var button = holder.find('.js-comment-edit-btn');

        button.removeAttr("disabled");
        quill.on('text-change', function () {
            if (quill.getLength() > 1) {
                button.removeAttr("disabled");
            } else {
                button.attr("disabled", "disabled");
            }
        });
    }

    function initReply(holder) {
        var showReplyLink = findControl(holder, '.js-comment-showReplyLink');
        var hideReplyLink = findControl(holder, '.js-comment-hideReplyLink');

        if (showReplyLink.length === 0 || hideReplyLink.length === 0) {
            return;
        }

        var commentReply = findControl(holder, '.js-comment-reply');

        showReplyLink.on('click', function () {
            showReplyLink.hide();
            hideReplyLink.show();
            commentReply.show();
            scrollToComment($(this));
        });

        hideReplyLink.on('click', function () {
            showReplyLink.show();
            hideReplyLink.hide();
            commentReply.hide();
        });

        var dataStorage = findControl(holder, '.js-hidden-comment-create-description')[0];
        var descriptionElem = findControl(holder, '.js-comment-create-description')[0];
        var quill = helpers.initQuill(descriptionElem, dataStorage, commentQuillOptions);
        var button = holder.find('.js-comment-create-btn');
        quill.on('text-change', function () {
            if (quill.getLength() > 1) {
                button.removeAttr("disabled");
            } else {
                button.attr("disabled", "disabled");
            }
        });

        function scrollToComment(el) {
            var comment = el.closest('.comments__list-body').find('.js-comment-reply');
            $('html, body').animate({
                scrollTop: comment.offset().top
            }, 500);
        }
    }

    function initDelete(holder) {
        var deleteLink = findControl(holder, '.js-comment-delete');

        if (deleteLink.length === 0) {
            return;
        }

        deleteLink.on('click', function () {
            return confirm($(this).data('text'));
        });
    }

    function findControl(holder, selector) {
        return holder.find(selector).filter(function () {
            var $this = $(this);
            var parent = $this.closest('[id^=js-comment-view-]');
            return parent.data('id') === holder.data('id');
        });
    }
}

appInitializer.add(function () {
    new CommentOverview('[id^=js-comments-overview-]');
});

export default CommentOverview;