var Forms = Forms || {};

Forms.UI = Forms.UI || {};

Forms.UI.FormBuilder = function (formID) {
    var This = this;
    this.FormDefinition = null;
    this.MetaData = null;
    this.Parent = null;
    this.SendButton = null;
    this.$Form = null;
    this.FormID = formID;
    this.ParentDataID = null;

    this.ReferenceFields = [];
    this.FileFields = [];

    this.SetParentDataID = function (dataID) {
        This.ParentDataID = dataID;
    };

    this.SetDefinition = function (formDefinition) {
        This.FormDefinition = formDefinition;
        return This;
    };

    this.SetMetaData = function (metaData) {
        This.MetaData = metaData;
        return This;
    };

    this.SetParent = function (parent) {
        This.Parent = parent;
        return This;
    };

    this.SetFieldBuilder = function (builder) {
        This.BuildFieldInternal = builder;
        return This;
    };

    this.SetSendButton = function (button) {
        This.SendButton = $(button);
        return This;
    };

    this.SetFieldValuesGetter = function (getter) {
        This.GetFieldValues = getter;
        return This;
    };

    this.SetSavedHandler = function (handler) {
        This.HandleSavedInternal = handler;
        return This;
    };

    this.SetValidationErrorHandler = function (handler) {
        This.HandleValidationErrorsInternal = handler;
        return This;
    };

    this.SetCustomErrorHandler = function (handler) {
        This.HandleCustomErrorInternal = handler;
        return This;
    };

    this.CreateSendButton = function (text) {
        This.SendButton = text;
        return This;
    };

    this.Clear = function () {
        if (This.$Form != null) {
            This.$Form.find('input[type=text]').val('');
            This.$Form.find('textarea').val('');
        }
    };

    this.PrepareDefinition = function () {
        Forms.IO.GetDefinition(this.FormID, function (form) {
            This.FormDefinition = form;
            This.BuildInternal();
        }, function () {
            alert('No such form!');
        });
    };

    this.Build = function () {
        if (This.FormDefinition == null) {
            This.PrepareDefinition();
        } else {
            this.BuildInternal();
        }
    };

    this.BuildInternal = function () {
        This.MetaData = this.CreateMetaDataInternal(this.MetaData);
        if (this.Parent == null) {
            throw new Error('Parent can\'t be null!');
        }
        if (this.FormDefinition.Type != 'Form') {
            throw new Error('FormBuilder can handle only forms!');
        }

        $parent = $(this.Parent);
        $form = $('<form />').appendTo($parent);
        This.$Form = $form;
        for (var i = 0; i < This.FormDefinition.Fields.length; i++) {
            var field = This.FormDefinition.Fields[i];
            var fieldMeta = This.CreateFieldMetaDataInternal(this.MetaData.Fields[field.PublicIdentifier], field);

            if (field.Type == 'ReferenceField') {
                This.ReferenceFields[this.ReferenceFields.length] = field;
            } else if (field.Type == 'FileField') {
                This.FileFields[this.FileFields.length] = field;
            }

            This.BuildFieldInternal(field, $form, fieldMeta);
        }

        if (This.SendButton != null) {
            if (typeof this.SendButton == 'string') {
                This.SendButton = $('<input type="button" name="send" class="form-send" value="' + This.SendButton + '" />').appendTo($form);
            }
            This.SendButton.click(function () {
                This.ProcessSend($form);
            });
        }
    };

    this.BuildFieldInternal = function (field, parent, metaData) {
        var fieldID = 'field-' + field.PublicIdentifier;

        $item = $('<div class="form-field ' + fieldID + '" />').appendTo($(parent));
        $label = $('<label for="' + fieldID + '" class="field-label" />').html(metaData.Label).appendTo($item);
        $field = $(metaData.Template.replace('{id}', fieldID).replace('{name}', field.Type == 'FileField' ? field.FileIdentifier : fieldID).replace('{class}', "field-input").replace('{value}', metaData.DefaultValue)).appendTo($item);
        $validation = $('<div class="field-validation" />').appendTo($item);
    };

    this.ProcessSend = function ($form) {
        if (This.FileFields.length > 0) {
            This.UploadFileInternal(This.ProcessSendInternal);
        } else {
            This.ProcessSendInternal($form);
        }


    };

    this.ProcessSendInternal = function ($form) {
        var form = This.FormDefinition;
        var fields = This.GetFieldValues(form, $form);

        Forms.IO.InsertFormData(form.PublicIdentifier, fields, function (item) {
            This.HandleSavedInternal(item, $form);
        }, function (errors) {
            This.HandleValidationErrorsInternal(errors, $form);
        }, function () {
            This.HandleCustomErrorInternal($form);
        });
        //alert(1);
    };

    this.GetFieldValues = function (formDefinition, $form) {
        var fields = [];
        for (var i = 0; i < formDefinition.Fields.length; i++) {
            fields[i] = {
                PublicIdentifier: formDefinition.Fields[i].PublicIdentifier,
                Value: This.GetFieldValueInternal(formDefinition.Fields[i], $('#field-' + formDefinition.Fields[i].PublicIdentifier))
            };
        }
        return fields;
    };

    this.GetFieldValueInternal = function (fieldDefinition, $input) {
        if (fieldDefinition.Type == 'FileField') {
            return fieldDefinition.FileIdentifier;
        }

        if($input.attr('type') == 'checkbox') {
            return $input.val() == 'on';
        }

        return $input.val();
    };

    this.UploadFileInternal = function (callback) {
        $iframe = $('<iframe id="' + This.FormID + '-upload" />').hide().appendTo($('body'));
        $iframe.load(function () {
            callback(This.$Form);
        });
        This.$Form.attr('method', 'post').attr('enctype', 'multipart/form-data').attr('action', Forms.IO.GetUploadUrl(This.FormID)).attr('target', This.FormID + '-upload').submit();
    };

    this.HandleSavedInternal = function (newItem, $form) {
        alert('Saved');
        This.Clear();
    };

    this.HandleValidationErrorsInternal = function (errors, $form) {
        for (var i = 0; i < errors.length; i++) {
            var error = errors[i];
            var $validator = $('.field-' + error.PublicIdentifier + ' .field-validation').html(This.MetaData.ErrorMessages[error.Error]);
            $field = $('#field-' + error.PublicIdentifier).change(function () {
                $(this).data('validator').html('');
            }).data('validator', $validator);

            if (i == 0) {
                $field.focus();
            }
        }
    };

    this.HandleCustomErrorInternal = function ($form) {
        //alert('Some error');
    };

    this.CreateMetaDataInternal = function (metaData) {
        if (metaData == null) {
            metaData = {};
        }
        if (This.IsNullOrUndefined(metaData.Fields)) {
            metaData.Fields = [];
        }
        if (This.IsNullOrUndefined(metaData.ErrorMessages)) {
            metaData.ErrorMessages = {};
        }
        if (This.IsNullOrUndefined(metaData.ErrorMessages['NoSuchFormDefinition'])) {
            metaData.ErrorMessages['NoSuchFormDefinition'] = 'No such form!';
        }
        if (This.IsNullOrUndefined(metaData.ErrorMessages['NoSuchFieldDefinition'])) {
            metaData.ErrorMessages['NoSuchFieldDefinition'] = 'No such field!';
        }
        if (This.IsNullOrUndefined(metaData.ErrorMessages['IncorrectFieldType'])) {
            metaData.ErrorMessages['IncorrectFieldType'] = 'Not a valid value!';
        }
        if (This.IsNullOrUndefined(metaData.ErrorMessages['IncorrectValue'])) {
            metaData.ErrorMessages['IncorrectValue'] = 'Not a valid value!';
        }
        if (This.IsNullOrUndefined(metaData.ErrorMessages['InvalidCreator'])) {
            metaData.ErrorMessages['InvalidCreator'] = 'Some invalid values!';
        }
        if (This.IsNullOrUndefined(metaData.ErrorMessages['NoSuchFormData'])) {
            metaData.ErrorMessages['NoSuchFormData'] = 'No such parent data!';
        }
        return metaData;
    };

    this.CreateFieldMetaDataInternal = function (metaData, field) {
        if (This.IsNullOrUndefined(metaData)) {
            metaData = {};
        }
        if (This.IsNullOrUndefined(metaData.RenderAs)) {
            if (field.Type == 'StringField' || field.Type == 'DoubleField') {
                metaData.RenderAs = 'textbox';
            } else if (field.Type == 'BoolField') {
                metaData.RenderAs = 'checkbox';
            } else if (field.Type == 'ReferenceField') {
                metaData.RenderAs = 'dropdown';
            } else if (field.Type == 'FileField') {
                metaData.RenderAs = 'file';
            }
        }
        if (This.IsNullOrUndefined(metaData.Template)) {
            switch (metaData.RenderAs) {
                case 'textbox':
                    metaData.Template = '<input type="text" id="{id}" name="{name}" class="{class}" value="{value}" />';
                    break;
                case 'textarea':
                    metaData.Template = '<textarea id="{id}" name="{name}" class="{class}">{value}</textarea>';
                    break;
                case 'checkbox':
                    metaData.Template = '<input type="checkbox" id="{id}" name="{name}" class="{class}" />';
                    break;
                case 'dropdown':
                    metaData.Template = '<select id="{id}" name="{name}" class="{class}" />';
                    break;
                case 'file':
                    metaData.Template = '<input type="file" id="{id}" name="{name}" class="{class}" value="{value}" />';
                    break;
            }
        }
        if (This.IsNullOrUndefined(metaData.Label)) {
            metaData.Label = field.Name + ':';
        }
        if (This.IsNullOrUndefined(metaData.DefaultValue)) {
            metaData.DefaultValue = '';
        }
        return metaData;
    };

    this.IsNullOrUndefined = function (value) {
        return typeof value == 'undefined' || value == null;
    };
};



Forms.UI.DataRenderer = function (formID) {
    var This = this;
    this.FormID = formID;
    this.ItemTemplate = '<p>Created: {created}</p>';
    this.Data = null;
    this.PageSize = 10;
    this.PageIndex = 0;
    this.Parent = null;
    this.DateTimeFormat = null;

    this.SetItemRenderer = function (renderer) {
        This.RenderItemInternal = renderer;
        return This;
    };

    this.SetItemTemplate = function (template) {
        this.ItemTemplate = template;
        return This;
    };

    this.SetPageSize = function (pageSize) {
        This.PageSize = pageSize;
        This.Data = null;
        return This;
    };

    this.SetPageIndex = function (pageIndex) {
        This.PageIndex = pageIndex;
        This.Data = null;
        return This;
    };

    this.SetParent = function (parent) {
        This.Parent = parent;
        return This;
    };

    this.GetData = function () {
        return This.Data;
    };

    this.SetData = function (data) {
        This.Data = data;
        return This;
    };

    this.SetDateTimeFormat = function (dateTimeFormat) {
        This.DateTimeFormat = dateTimeFormat;
        return This;
    };

    this.LoadData = function (callback) {
        Forms.IO.GetFormData(This.FormID, This.PageSize, This.PageIndex, function (data) {
            This.Data = data;
            callback(data);
        });
        return This;
    };

    this.Render = function () {
        if (This.Data == null) {
            This.LoadData(function () {
                This.RenderInternal();
            });
        } else {
            This.RenderInternal();
        }
        return This;
    };

    this.ReRender = function () {
        $(This.Parent).html('');
        This.Render();
    };

    this.RenderInternal = function () {
        $parent = $(This.Parent);
        for (var i = 0; i < This.Data.length; i++) {
            var item = This.Data[i];
            This.RenderItemInternal(item, $parent);
        }
    };

    this.RenderItemInternal = function (item, $parent) {
        var created = eval('new ' + item.Created.replace('/', '').replace('/', ''));
        var template = This.ItemTemplate;
        template = template.replace('{created}', This.DateTimeFormat != null ? created.format(This.DateTimeFormat) : created);

        for (var j = 0; j < item.Fields.length; j++) {
            var field = item.Fields[j];
            template = template
                .replace('{' + field.PublicIdentifier + '}', field.Value)
                .replace('{' + field.PublicIdentifier + ':Name}', field.Name)
                .replace('{' + field.PublicIdentifier + ':FileUrl}', field.FileUrl);
        }
        $(template).appendTo($parent);
    };
};





Forms.Util = Forms.Util || {};

Forms.Util.DateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
        timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        timezoneClip = /[^-+\dA-Z]/g,
        pad = function (val, len) {
            val = String(val);
            len = len || 2;
            while (val.length < len) val = "0" + val;
            return val;
        };

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {
        var dF = Forms.Util.DateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        mask = String(dF.Masks[mask] || mask || dF.Masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
            d = date[_ + "Date"](),
            D = date[_ + "Day"](),
            M = date[_ + "Month"](),
            y = date[_ + "FullYear"](),
            H = date[_ + "Hours"](),
            m = date[_ + "Minutes"](),
            s = date[_ + "Seconds"](),
            L = date[_ + "Milliseconds"](),
            o = utc ? 0 : date.getTimezoneOffset(),
            flags = {
                d: d,
                dd: pad(d),
                ddd: dF.I18N.dayNames[D],
                dddd: dF.I18N.dayNames[D + 7],
                M: M + 1,
                MM: pad(M + 1),
                MMM: dF.I18N.monthNames[M],
                MMMM: dF.I18N.monthNames[M + 12],
                yy: String(y).slice(2),
                yyyy: y,
                h: H % 12 || 12,
                hh: pad(H % 12 || 12),
                H: H,
                HH: pad(H),
                m: m,
                mm: pad(m),
                s: s,
                ss: pad(s),
                l: pad(L, 3),
                L: pad(L > 99 ? Math.round(L / 10) : L),
                t: H < 12 ? "a" : "p",
                tt: H < 12 ? "am" : "pm",
                T: H < 12 ? "A" : "P",
                TT: H < 12 ? "AM" : "PM",
                Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
            };

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
} ();

// Some common format strings
Forms.Util.DateFormat.Masks = {
    "default": "ddd MMM dd yyyy HH:mm:ss",
    shortDate: "M/d/yy",
    mediumDate: "MMM d, yyyy",
    longDate: "MMMM d, yyyy",
    fullDate: "dddd, MMMM d, yyyy",
    shortTime: "h:mm TT",
    mediumTime: "h:mm:ss TT",
    longTime: "h:mm:ss TT Z",
    isoDate: "yyyy-MM-dd",
    isoTime: "HH:mm:ss",
    isoDateTime: "yyyy-MM-dd'T'HH:mm:ss",
    isoUtcDateTime: "UTC:yyyy-MM-dd'T'HH:mm:ss'Z'"
};

// Internationalization strings
Forms.Util.DateFormat.I18N = {
    dayNames: [
        "Ne", "Po", "Út", "St", "Čt", "Pá", "So",
        "Neděle", "Pondělí", "Úterý", "Středa", "Čtvrtek", "Pátek", "Sobota"
    ],
    monthNames: [
        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
        "Leden", "Únor", "Březen", "Duben", "Květen", "Červen", "Červenec", "Srpen", "Září", "Říjen", "Listopad", "Prosinec"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc) {
    return Forms.Util.DateFormat(this, mask, utc);
};