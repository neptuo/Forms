var Forms = Forms || {};

Forms.UI = Forms.UI || {};

Forms.UI.FormBuilder = function (formID) {
    This = this;
    this.FormDefinition = null;
    this.MetaData = null;
    this.Parent = null;
    this.SendButton = null;
    this.FormID = formID;
};

Forms.UI.FormBuilder.prototype.SetDefinition = function (formDefinition) {
    this.FormDefinition = formDefinition;
    return this;
};

Forms.UI.FormBuilder.prototype.SetMetaData = function (metaData) {
    this.MetaData = metaData;
    return this;
};

Forms.UI.FormBuilder.prototype.SetParent = function (parent) {
    this.Parent = parent;
    return this;
};

Forms.UI.FormBuilder.prototype.SetFieldBuilder = function (builder) {
    Forms.UI.FormBuilder.prototype.BuildFieldInternal = builder;
    return this;
};

Forms.UI.FormBuilder.prototype.SetSendButton = function (button) {
    this.SendButton = $(button);
    return this;
};

Forms.UI.FormBuilder.prototype.CreateSendButton = function (text) {
    this.SendButton = text;
    return this;
};

Forms.UI.FormBuilder.prototype.PrepareDefinition = function () {
    Forms.IO.GetDefinition(this.FormID, function (form) {
        This.FormDefinition = form;
        This.BuildInternal();
    }, function () {
        alert('No such form!');
    });
};

Forms.UI.FormBuilder.prototype.Build = function () {
    if (this.FormDefinition == null) {
        this.PrepareDefinition();
    } else {
        this.BuildInternal();
    }
};

Forms.UI.FormBuilder.prototype.BuildInternal = function () {
    this.MetaData = this.CreateMetaDataInternal(this.MetaData);
    if (this.Parent == null) {
        throw new Error('Parent can\'t be null!');
    }
    if (this.FormDefinition.Type != 'Form') {
        throw new Error('FormBuilder can handle only forms!');
    }

    $parent = $(this.Parent);
    $form = $('<form />').appendTo($parent);
    for (var i = 0; i < this.FormDefinition.Fields.length; i++) {
        var field = this.FormDefinition.Fields[i];
        var fieldMeta = this.CreateFieldMetaDataInternal(this.MetaData.Fields[field.PublicIdentifier], field);
        this.BuildFieldInternal(field, $form, fieldMeta);
    }

    if (this.SendButton != null) {
        if (typeof this.SendButton == 'string') {
            this.SendButton = $('<input type="button" name="send" class="form-send" value="' + this.SendButton + '" />').appendTo($form);
        }
        this.SendButton.click(function () {
            This.ProcessSend(form);
        });
    }
};

Forms.UI.FormBuilder.prototype.BuildFieldInternal = function (field, parent, metaData) {
    var fieldID = 'field-' + field.PublicIdentifier;

    $item = $('<div class="form-field ' + fieldID + '" />').appendTo($(parent));
    $label = $('<label for="' + fieldID + '" class="field-label" />').html(metaData.Label).appendTo($item);
    $field = $(metaData.Template.replace('{id}', fieldID).replace('{name}', fieldID).replace('{class}', "field-input").replace('{value}', metaData.DefaultValue)).appendTo($item);
    $validation = $('<div class="field-validation" />').appendTo($item);
};

Forms.UI.FormBuilder.prototype.ProcessSend = function ($form) {
    var form = this.FormDefinition;
    var fields = [];
    for (var i = 0; i < form.Fields.length; i++) {
        fields[i] = {
            PublicIdentifier: form.Fields[i].PublicIdentifier,
            Value: $('#field-' + form.Fields[i].PublicIdentifier).val()
        };
    }

    //TODO: Continue here...
    Forms.IO.InsertFormData(form.PublicIdentifier, fields, function (item) {
        alert('Saved');
        $form.find('input[type=text]').val('');
    }, function (errors) {
        for (var i = 0; i < errors.length; i++) {
            var error = errors[i];
            $field = $('#field-' + error.PublicIdentifier).css('border', '1px solid red').attr('placeholder', This.MetaData.ErrorMessages[error.Error]).focusout(function () {
                $(this).css('border', '').attr('placeholder', '');
            });

            if (i == 0) {
                $field.focus();
            }
        }
    }, function () {
        alert('Some error');
    });
};

Forms.UI.FormBuilder.prototype.CreateMetaDataInternal = function (metaData) {
    if (metaData == null) {
        metaData = {};
    }
    if (typeof metaData.Fields == 'undefined' || metaData.Fields == null) {
        metaData.Fields = [];
    }
    if (typeof metaData.ErrorMessages == 'undefined' || metaData.ErrorMessages == null) {
        metaData.ErrorMessages = {};
    }
    if (typeof metaData.ErrorMessages['NoSuchFormDefinition'] == 'undefined' || metaData.ErrorMessages['NoSuchFormDefinition'] == null) {
        metaData.ErrorMessages['NoSuchFormDefinition'] = 'No such form!';
    }
    if (typeof metaData.ErrorMessages['NoSuchFieldDefinition'] == 'undefined' || metaData.ErrorMessages['NoSuchFieldDefinition'] == null) {
        metaData.ErrorMessages['NoSuchFieldDefinition'] = 'No such field!';
    }
    if (typeof metaData.ErrorMessages['IncorrectFieldType'] == 'undefined' || metaData.ErrorMessages['IncorrectFieldType'] == null) {
        metaData.ErrorMessages['IncorrectFieldType'] = 'Not a valid value!';
    }
    if (typeof metaData.ErrorMessages['IncorrectValue'] == 'undefined' || metaData.ErrorMessages['IncorrectValue'] == null) {
        metaData.ErrorMessages['IncorrectValue'] = 'Not a valid value!';
    }
    return metaData;
};

Forms.UI.FormBuilder.prototype.CreateFieldMetaDataInternal = function (metaData, field) {
    if (typeof metaData == 'undefined' || metaData == null) {
        metaData = {};
    }
    if (typeof metaData.RenderAs == 'undefined' || metaData.RenderAs == null) {
        if (field.Type == 'StringField' || field.Type == 'DoubleField') {
            metaData.RenderAs = 'textbox';
        } else if (field.Type == 'BoolField') {
            metaData.RenderAs = 'checkbox';
        } else if (field.Type == 'ReferenceField') {
            metaData.RenderAs = 'dropdown';
        }
    }
    if (typeof metaData.Template == 'undefined' || metaData.Template == null) {
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
        }
    }
    if (typeof metaData.Label == 'undefined' || metaData.Label == null) {
        metaData.Label = field.Name + ':';
    }
    if (typeof metaData.DefaultValue == 'undefined' || metaData.DefaultValue == null) {
        metaData.DefaultValue = '';
    }
    return metaData;
};




Forms.UI.DataRenderer = function (formID) {
    This = this;
    this.FormID = formID;
    this.ItemTemplate = '<p>Created: {created}</p>';
    this.Data = null;
    this.PageSize = 10;
    this.PageIndex = 0;
    this.Parent = null;
    this.DateTimeFormat = null;
};

Forms.UI.DataRenderer.prototype.SetItemRenderer = function (renderer) {
    this.RenderItemInternal = renderer;
    return this;
};

Forms.UI.DataRenderer.prototype.SetItemTemplate = function (template) {
    this.ItemTemplate = template;
    return this;
};

Forms.UI.DataRenderer.prototype.SetPageSize = function (pageSize) {
    this.PageSize = pageSize;
    this.Data = null;
    return this;
};

Forms.UI.DataRenderer.prototype.SetPageIndex = function (pageIndex) {
    this.PageIndex = pageIndex;
    this.Data = null;
    return this;
};

Forms.UI.DataRenderer.prototype.SetParent = function (parent) {
    this.Parent = parent;
    return this;
};

Forms.UI.DataRenderer.prototype.SetDateTimeFormat = function (dateTimeFormat) {
    this.DateTimeFormat = dateTimeFormat;
    return this;
};

Forms.UI.DataRenderer.prototype.LoadData = function (callback) {
    Forms.IO.GetFormData(this.FormID, this.PageSize, this.PageIndex, function (data) {
        This.Data = data;
        callback(data);
    });
    return this;
};

Forms.UI.DataRenderer.prototype.Render = function () {
    if (this.Data == null) {
        this.LoadData(function () {
            This.RenderInternal();
        });
    } else {
        This.RenderInternal();
    }
    return this;
};

Forms.UI.DataRenderer.prototype.RenderInternal = function () {
    $parent = $(this.Parent);
    for (var i = 0; i < this.Data.length; i++) {
        var item = this.Data[i];
        this.RenderItemInternal(item, $parent);
    }
};

Forms.UI.DataRenderer.prototype.RenderItemInternal = function (item, $parent) {
    var created = eval('new ' + item.Created.replace('/', '').replace('/', ''));
    var template = this.ItemTemplate;
    template = template.replace('{created}', this.DateTimeFormat != null ? created.toString(this.DateTimeFormat) : created);

    for (var j = 0; j < item.Fields.length; j++) {
        var field = item.Fields[j];
        template = template.replace('{' + field.PublicIdentifier + '}', field.Value);
    }
    $(template).appendTo($parent);
};

