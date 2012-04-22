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
    Forms.IO.GetDefinition(formID, function (form) {
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
        var fieldMeta = this.CreateFieldMetaDataInternal(this.MetaData.Fields[i], field);
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

    $item = $('<div class="form-field" />').appendTo($(parent));
    $label = $('<label for="' + fieldID + '" />').html(metaData.Label).appendTo($item);
    $field = $(metaData.Template.replace('{id}', fieldID).replace('{name}', fieldID)).appendTo($item);
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
                metaData.Template = '<input type="text" id="{id}" name="{name}" class="{class}" />';
                break;
            case 'textarea':
                metaData.Template = '<textarea id="{id}" name="{name}" class="{class}" />';
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
    return metaData;
};