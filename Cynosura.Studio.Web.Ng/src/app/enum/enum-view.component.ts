import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { Error } from '../core/error.model';
import { NoticeHelper } from '../core/notice.helper';
import { ConvertStringTo } from '../core/converter.helper';

import { Enum } from '../enum-core/enum.model';
import { EnumService } from '../enum-core/enum.service';
import { EnumEditComponent } from './enum-edit.component';

@Component({
    selector: 'app-enum-view',
    templateUrl: './enum-view.component.html',
    styleUrls: ['./enum-view.component.scss']
})
export class EnumViewComponent implements OnInit {
    id: string;
    enum: Enum;
    error: Error;
    solutionId: number;

    constructor(private dialog: MatDialog,
                private enumService: EnumService,
                private route: ActivatedRoute,
                private noticeHelper: NoticeHelper) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            this.id = ConvertStringTo.string(params.id);
            this.solutionId = ConvertStringTo.number(this.route.snapshot.queryParams.solutionId);
            this.getEnum();
        });
    }

    private getEnum() {
        this.enumService.getEnum({ solutionId: this.solutionId, id: this.id })
            .subscribe(e => this.enum = e);
    }

    onEdit() {
        EnumEditComponent.show(this.dialog, this.solutionId, this.id).subscribe(() => {
            this.getEnum();
        });
    }

    onBack(): void {
        window.history.back();
    }

    onGenerate() {
        this.enumService.generateEnum({ solutionId: this.solutionId, id: this.enum.id })
            .subscribe(() => this.noticeHelper.showMessage('Generation completed'),
                error => this.onError(error));
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
        }
    }
}
