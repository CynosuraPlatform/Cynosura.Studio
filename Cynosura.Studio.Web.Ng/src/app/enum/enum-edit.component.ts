import { Component, Input, OnInit, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { filter } from 'rxjs/operators';

import { Error } from '../core/error.model';
import { NoticeHelper } from '../core/notice.helper';
import { ConvertStringTo } from '../core/converter.helper';

import { Enum } from '../enum-core/enum.model';
import { EnumService } from '../enum-core/enum.service';
import { UpdateEnum, CreateEnum } from '../enum-core/enum-request.model';

class DialogData {
    id: string;
    solutionId: number;
}
@Component({
    selector: 'app-enum-edit',
    templateUrl: './enum-edit.component.html',
    styleUrls: ['./enum-edit.component.scss']
})
export class EnumEditComponent implements OnInit {
    id: string;
    enumForm = this.fb.group({
        id: [],
        name: [],
        displayName: []
    });
    enum: Enum;
    error: Error;
    solutionId: number;

    constructor(public dialogRef: MatDialogRef<EnumEditComponent>,
                @Inject(MAT_DIALOG_DATA) public data: DialogData,
                private enumService: EnumService,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) {
        this.id = data.id;
        this.solutionId = data.solutionId;
    }

    static show(dialog: MatDialog, solutionId: number, id: string): Observable<any> {
        const dialogRef = dialog.open(EnumEditComponent, {
            width: '600px',
            data: {
                solutionId: solutionId,
                id: id
            }
        });
        return dialogRef.afterClosed()
            .pipe(filter(res => res === true));
    }

    ngOnInit(): void {
        this.getEnum();
    }

    private getEnum() {
        const getEnum$ = !this.id ?
            of(new Enum()) :
            this.enumService.getEnum({ solutionId: this.solutionId, id: this.id });
        getEnum$.subscribe(e => {
            this.enum = e;
            this.enumForm.patchValue(this.enum);
        });
    }

    onSave(): void {
        this.saveEnum();
    }

    private saveEnum() {
        let saveEnum$: Observable<{}>;
        if (this.id) {
            const updateEnum: UpdateEnum = this.enumForm.value;
            updateEnum.solutionId = this.solutionId;
            updateEnum.properties = this.enum.properties;
            updateEnum.values = this.enum.values;
            saveEnum$ = this.enumService.updateEnum(updateEnum);
        } else {
            const createEnum: CreateEnum = this.enumForm.value;
            createEnum.solutionId = this.solutionId;
            createEnum.properties = this.enum.properties;
            createEnum.values = this.enum.values;
            saveEnum$ = this.enumService.createEnum(createEnum);
        }
        saveEnum$.subscribe(() => this.dialogRef.close(true),
            error => this.onError(error));
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.enumForm, error);
        }
    }
}
