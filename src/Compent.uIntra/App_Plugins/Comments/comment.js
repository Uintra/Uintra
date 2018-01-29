import helpers from "./../Core/Content/scripts/Helpers";

require("./../Core/Content/libs/jquery.validate.min.js");
require("./../Core/Content/libs/jquery.unobtrusive-ajax.min.js");
require("./../Core/Content/libs/jquery.validate.unobtrusive.min.js");
require("./comments.css");

var initSubmitButton  = function(holder) {
    var createControls = holder.find('.js-comment-create');
    createControls.each(function () {
        var $this = $(this);
        var btn = $this.find('.js-disable-submit');
        btn.click(function (event) {
            if (!$this.valid()) {
                return;
            }
            $this.submit();
        });
    });
};

var initCreateControl = function (holder) {
    var createControls = holder.find('.js-comment-create');

    createControls.each(function () {
        var $this = $(this);

        if ($this.data('parentid')) {
            return true;
        }

        $this.on('submit', function () {
            $this.valid();
        });

        var dataStorage = $this.find('.js-hidden-comment-create-description')[0];
        var descriptionElem = $this.find('.js-comment-create-description')[0];
        var quill = helpers.initQuill(descriptionElem, dataStorage);

        quill.onLinkDetected(function(link) {
            showLinkPreview(link);
        });



        var button = $this.find('.js-comment-create-btn');
        var toolbarBtns = $this.find('.ql-formats button');

        function showLinkPreview(link) {
            $.get('/umbraco/api/LinkPreviewApi/Preview?url=' + link,
                function (data) {
                    console.log(data);
                    var imageElem = getImageElem(data);
                    $this.append(imageElem);
                });
        }

        function getImageElem(data) {
            var titleElem = createParagraph(data.title);
            var descriptionElem = createParagraph(data.description);
            var imgElem = createImg(data.imageUri);

            var divElem = document.createElement('div');
            divElem.appendChild(imgElem);
            divElem.appendChild(titleElem);
            divElem.appendChild(descriptionElem);

            return divElem;
        }

        function createImg(imageUri) {
            var imgElem = document.createElement('img');
            var srcAttr = document.createAttribute('src');
            srcAttr.value = imageUri;
            imgElem.setAttributeNode(srcAttr);
            return imgElem;
        }

        function createParagraph(content) {
            var paragraph = document.createElement('p');
            var contentNode = document.createTextNode(content);
            paragraph.appendChild(contentNode);
            return paragraph;
        }

        toolbarBtns.each(function(){
            var className = $(this).attr('class').split("-");
            var tooltip = className[className.length - 1];
            $(this).attr('title', tooltip);
        });

        quill.on('text-change',
            (delta, oldDelta, source) =>
                quillTextChangeEventHandler(quill, button, delta, oldDelta, source));

        quill.setText('');
        dataStorage.value = '';
    });
};

var initEdit = function (holder) {
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
    var quill = helpers.initQuill(descriptionElem, dataStorage);
    var button = holder.find('.js-comment-edit-btn');
    var form = holder.find('.js-comment-edit');
    
    button.click(function (event) {
        if (!form.valid()) {
            return;
        }
        $(event.target).closest(form).submit();
    });

    button.removeAttr("disabled");

    var toolbarBtns = editControlContainer.find('.ql-formats button');

    toolbarBtns.each(function(){
        var className = $(this).attr('class').split("-");
        var tooltip = className[className.length - 1];
        $(this).attr('title', tooltip);
    });

    quill.on('text-change',
        (delta, oldDelta, source) =>
            quillTextChangeEventHandler(quill, button, delta, oldDelta, source));
};

var initReply = function (holder) {
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
    var quill = helpers.initQuill(descriptionElem, dataStorage);
    var button = holder.find('.js-comment-create-btn');

    var toolbarBtns = commentReply.find('.ql-formats button');

    toolbarBtns.each(function(){
        var className = $(this).attr('class').split("-");
        var tooltip = className[className.length - 1];
        $(this).attr('title', tooltip);
    });

    quill.on('text-change',
        (delta, oldDelta, source) =>
            quillTextChangeEventHandler(quill, button, delta, oldDelta, source));
};

var initDelete = function (holder) {
    var deleteLink = findControl(holder, '.js-comment-delete');

    if (deleteLink.length === 0) {
        return;
    }

    deleteLink.on('click', function () {
        return confirm($(this).data('text'));
    });
}

function scrollToComment(el) {
    var comment = el.closest('.comments__list-body').find('.js-comment-reply');
    var tabset = $('.tabset');
    var topIndent = 80; //header height + 30px gap
    if(tabset.length){
        topIndent += tabset.height();
    }
    $('html, body').animate({ scrollTop: comment.offset().top - topIndent}, 500);
}

function findControl(holder, selector) {
    return holder.find(selector).filter(function () {
        var $this = $(this);
        var parent = $this.closest('.js-comment-view');
        return parent.data('id') === holder.data('id');
    });
}

function quillTextChangeEventHandler(quill, button, delta, oldDelta, source) {
    setButtonDisableState(quill, button);
}

function setButtonDisableState(quill, button) {
    var n = quill.container.querySelectorAll("img").length;
    if (quill.getText().trim().length > 0 || n > 0) {
        button.removeAttr("disabled");
    } else {
        button.attr("disabled", "disabled");
    }
}

var CommentOverview = function (selector) {
    var holders = $(selector);
    if (!holders || holders.length === 0) {
        return;
    }
    $.validator.unobtrusive.parse(selector);
    holders.each(function () {
        var $this = $(this);
        initCreateControl($this);
        initSubmitButton($this)
        $this.find('.js-comment-view').each(function () {
            new Comment(this);
        });
    });
};

var Comment = function (selector) {
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
};

function init() {
    new CommentOverview('.js-comments-overview');
}

var controller = {
    init: init,
    factory: CommentOverview
};

uIntra.methods.add("CommentOverview", CommentOverview);

export default controller;