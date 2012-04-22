var Forms = Forms || {};

Forms.IO = Forms.IO || {};

Forms.IO.DomainName = 'localhost:36258';
Forms.IO.BaseUrl = 'http://' + Forms.IO.DomainName + '/ws/';

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


Forms.IO.GetFormData = function (formID, pageSize, pageIndex, success, error) {
    $.jsonp({
        url: Forms.IO.BaseUrl + formID + '/data?pageSize=' + pageSize + '&pageIndex=' + pageIndex + '&p=?',
        success: function (json) {
            success(json);
        },
        error: function () {
            error();
        }
    });
};

Forms.IO.InsertFormData = function (formID, fields, success, validation, error) {
    var queryString = '';
    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        queryString += field.PublicIdentifier + '=' + encodeURIComponent(field.Value) + '&';
    }

    $.jsonp({
        url: Forms.IO.BaseUrl + formID + '/insert?' + queryString + 'p=?',
        success: function (json) {
            if (typeof json.Errors == 'undefined') {
                success(json);
            } else {
                validation(json.Errors);
            }
        },
        error: function () {
            error();
        }
    });
};



Forms.IO.GetInquiryData = function (inquiryID, success, error) {
    throw new Error('Not yet implemented!');
};