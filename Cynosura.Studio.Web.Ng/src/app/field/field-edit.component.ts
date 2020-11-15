import { Component, Input, OnInit, Output, EventEmitter, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Error } from '../core/error.model';
import { NoticeHelper } from '../core/notice.helper';

import { Field, FieldType } from '../field-core/field.model';

class DialogData {
    field: Field;
    solutionId: number;
}

@Component({
    selector: 'app-field-edit',
    templateUrl: './field-edit.component.html',
    styleUrls: ['./field-edit.component.scss']
})
export class FieldEditComponent implements OnInit {

    FieldType = FieldType;

    solutionId: number;

    fieldForm = this.fb.group({
        id: [],
        name: [],
        displayName: [],
        type: [],
        size: [],
        entityId: [],
        isRequired: [],
        enumId: [],
        isSystem: []
    });

    private localField: Field;

    get field(): Field {
        return this.localField;
    }

    @Input()
    set field(value: Field) {
        this.localField = value;
        this.fieldForm.reset();
        this.fieldForm.patchValue(value);
    }

    error: Error;

    constructor(public dialogRef: MatDialogRef<FieldEditComponent>,
                @Inject(MAT_DIALOG_DATA) public data: DialogData,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) {
        this.solutionId = data.solutionId;
        this.field = data.field;
    }

    ngOnInit(): void {

    }

    onSave(): void {
        this.saveField();
    }

    private saveField(): void {
        const field: Field = this.fieldForm.value;
        field.properties = this.field.properties;
        this.dialogRef.close(field);
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.fieldForm, error);
        }
    }
}
