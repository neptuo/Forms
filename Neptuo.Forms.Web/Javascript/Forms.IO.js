var Forms = Forms || {};

Forms.IO = Forms.IO || {};

Forms.IO.BaseUrl = 'http://localhost:36258/ws/';

Forms.IO.GetDefinition = function(formID, success, error) {
    $.jsonp({
        url: Forms.IO.BaseUrl + formID + '/definition?p=?',
        success: function (json) {
            success(json);
        },
        error: function () {
            error();
        }
    });
};

Forms.IO.InsertData2 = function (formID, fields) {
    var genID = Forms.IO.GenerateIdentifier();
    $iframe = $('<iframe id="' + genID + '" name="' + genID + '" />').hide().appendTo('body');
    $form = $('<form method="post" action="' + Forms.IO.BaseUrl + formID + '/insert" target="' + genID + '" />').hide().appendTo('body');

    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        $input = $('<input type="hidden" name="' + field.PublicIdentifier + '" value="' + field.Value + '" />').appendTo($form);
    }

    $iframe.load(function (e) {
        $iframe.remove();
        $form.remove();
    });
    $form.submit();
};

Forms.IO.InsertData = function (formID, fields, success, error) {
    var queryString = '';
    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        queryString += field.PublicIdentifier + '=' + encodeURIComponent(field.Value) + '&';
    }

    $.jsonp({
        url: Forms.IO.BaseUrl + formID + '/insert?' + queryString + 'p=?',
        success: function (json) {
            success(json);
        },
        error: function () {
            error();
        }
    });
};

Forms.IO.GenerateIdentifier = function() {
    return Math.floor((Math.random()*10000)+1);
};