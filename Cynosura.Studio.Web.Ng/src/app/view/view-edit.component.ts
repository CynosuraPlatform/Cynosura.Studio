import { Component, Input, OnInit, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { filter } from 'rxjs/operators';

import { Error } from '../core/error.model';
import { NoticeHelper } from '../core/notice.helper';
import { ConvertStringTo } from '../core/converter.helper';

import { View } from '../view-core/view.model';
import { ViewService } from '../view-core/view.service';

class DialogData {
    id: string;
    solutionId: number;
}

@Component({
    selector: 'app-view-edit',
    templateUrl: './view-edit.component.html',
    styleUrls: ['./view-edit.component.scss']
})
export class ViewEditComponent implements OnInit {
    id: string;
    viewForm = this.fb.group({
        id: [],
        name: []
    });
    view: View;
    error: Error;
    solutionId: number;

    constructor(public dialogRef: MatDialogRef<ViewEditComponent>,
                @Inject(MAT_DIALOG_DATA) public data: DialogData,
                private viewService: ViewService,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) {
        this.id = data.id;
        this.solutionId = data.solutionId;
    }

    static show(dialog: MatDialog, solutionId: number, id: string): Observable<any> {
        const dialogRef = dialog.open(ViewEditComponent, {
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
        this.getView();
    }

    private getView() {
        const getView$ = !this.id ?
            of(new View()) :
            this.viewService.getView({ solutionId: this.solutionId, id: this.id });
        getView$.subscribe(view => {
            this.view = view;
            this.viewForm.patchValue(this.view);
        });
    }

    onSave(): void {
        this.saveView();
    }

    private saveView() {
        const view = this.viewForm.value;
        view.solutionId = this.solutionId;
        const saveView$ = this.id ?
            this.viewService.updateView(view) :
            this.viewService.createView(view);
        saveView$.subscribe(() => this.dialogRef.close(true),
            error => this.onError(error));
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.viewForm, error);
        }
    }
}
