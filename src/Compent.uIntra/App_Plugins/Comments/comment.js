import helpers from "./../Core/Content/scripts/Helpers";
import ajax from './../Core/Content/scripts/Ajax';

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
        /*var toolbarSelector = document.querySelector(".js-create-bulletin__toolbar");
        var quill = helpers.initQuill(descriptionElem, dataStorage, {
            placeholder: description.dataset["placeholder"],
            modules: {
                toolbar: {
                    container: toolbarSelector
                }
            }
        });*/
        var quill = helpers.initQuill(descriptionElem, dataStorage);

        var isOneLinkDetected = false;

        quill.onLinkDetected(function (link) {
            if (!isOneLinkDetected) {
                showLinkPreview(link);
                isOneLinkDetected = true;
            }
        });

        var button = $this.find('.js-comment-create-btn');
        var toolbarBtns = $this.find('.ql-formats button');

        function showLinkPreview(link) {
            ajax.get('/umbraco/api/LinkPreview/Preview?url=' + link)
                .then(function (response) {
                    var data = response.data;
                    var imageElem = getImageElem(data);
                    var hiddenSaveElem = getHiddenSaveElem(data);
                    $this.append(imageElem);
                    $this.append(hiddenSaveElem);
                })
                .catch(err => {
                    // Ignore error and do not crash if server returns non-success code
                });
        }
        
        function getImageElem(data) {
            var divElem = document.createElement('div');
            divElem.className += "link-preview";

            divElem.innerHTML = 
                `<h3>
                     <a href="${data.uri}">${data.title}</a>
                 </h3>
                 <p>${data.description}</p>
                 <div class="link-preview-image">
                     <img src="${data.imageUri}" />
                 </div>`;

            return divElem;
        }

        function getHiddenSaveElem(data) {
            return createHiddenInput('linkPreviewId', data.id);
        }

        function createHiddenInput(name, value) {
            var input = document.createElement('input');

            input.setAttribute('type', 'hidden');
            input.setAttribute('name', name);
            input.setAttribute('value', value);

            return input;
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
    var linkPreviewContainer = findControl(holder, '.js-comment-link-preview-container');
    var editlink = findControl(holder, '.js-comment-editlink');
    var hideEditlink = findControl(holder, '.js-comment-hideEditLink');
    var removeLinkPreviewButton = findControl(holder, '.js-link-preview-remove-preview');
    var linkPreviewIdContainer = findControl(holder, 'input[name="linkPreviewId"]')[0];
    var linkPreviewEditContainer = findControl(holder, '.js-link-preview-edit-preview-container');

    if (editlink.length === 0 || hideEditlink.length === 0) {
        return;
    }

    var editControlContainer = findControl(holder, '.js-comment-editContainer');
    var descriptionControl = findControl(holder, '.js-comment-description');

    removeLinkPreviewButton.on('click', function () {
        linkPreviewIdContainer.value = null;
        linkPreviewEditContainer.hide();
    });

    editlink.on('click', function () {
        linkPreviewContainer.hide();
        editlink.hide();
        hideEditlink.show();
        descriptionControl.hide();
        editControlContainer.show();
    });

    hideEditlink.on('click', function () {
        linkPreviewContainer.show();
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