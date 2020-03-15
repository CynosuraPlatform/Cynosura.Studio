import { Component, Input, OnInit, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { filter } from 'rxjs/operators';

import { Error } from '../core/error.model';
import { NoticeHelper } from '../core/notice.helper';
import { ConvertStringTo } from '../core/converter.helper';

import { Solution } from '../solution-core/solution.model';
import { SolutionService } from '../solution-core/solution.service';
import { TemplateModel, TemplateService } from '../solution-core/template-service';

class DialogData {
    id: number;
}

@Component({
    selector: 'app-solution-edit',
    templateUrl: './solution-edit.component.html',
    styleUrls: ['./solution-edit.component.scss']
})
export class SolutionEditComponent implements OnInit {
    id: number;
    solutionForm = this.fb.group({
        id: [],
        name: [],
        path: [],
        templateName: [],
        templateVersion: []
    });
    solution: Solution;
    templates: TemplateModel;
    error: Error;

    constructor(public dialogRef: MatDialogRef<SolutionEditComponent>,
                @Inject(MAT_DIALOG_DATA) public data: DialogData,
                private solutionService: SolutionService,
                private templateService: TemplateService,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) {
        this.id = data.id;
    }

    static show(dialog: MatDialog, id: number): Observable<any> {
        const dialogRef = dialog.open(SolutionEditComponent, {
            width: '600px',
            data: { id: id }
        });
        return dialogRef.afterClosed()
            .pipe(filter(res => res === true));
    }

    ngOnInit(): void {
        this.getSolution();
        this.templateService.getTemplates()
            .subscribe(templates => this.templates = templates);
    }

    private getSolution() {
        const getSolution$ = !this.id ?
            of(new Solution()) :
            this.solutionService.getSolution({ id: this.id });
        getSolution$.subscribe(solution => {
            this.solution = solution;
            this.solutionForm.patchValue(this.solution);
        });
    }

    onSave(): void {
        this.saveSolution();
    }

    private saveSolution() {
        const saveSolution$ = this.id ?
            this.solutionService.updateSolution(this.solutionForm.value) :
            this.solutionService.createSolution(this.solutionForm.value);
        saveSolution$.subscribe(() => this.dialogRef.close(true),
            error => this.onError(error));
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.solutionForm, error);
        }
    }
}
