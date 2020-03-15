import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { Error } from '../core/error.model';
import { NoticeHelper } from '../core/notice.helper';
import { ConvertStringTo } from '../core/converter.helper';

import { Solution } from '../solution-core/solution.model';
import { SolutionService } from '../solution-core/solution.service';
import { SolutionEditComponent } from './solution-edit.component';

@Component({
    selector: 'app-solution-view',
    templateUrl: './solution-view.component.html',
    styleUrls: ['./solution-view.component.scss']
})
export class SolutionViewComponent implements OnInit {
    id: number;
    solution: Solution;
    error: Error;

    constructor(private dialog: MatDialog,
                private solutionService: SolutionService,
                private route: ActivatedRoute,
                private noticeHelper: NoticeHelper) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            this.id = ConvertStringTo.number(params.id);
            this.getSolution();
        });
    }

    private getSolution() {
        this.solutionService.getSolution({ id: this.id })
            .subscribe(solution => this.solution = solution);
    }

    onEdit() {
        SolutionEditComponent.show(this.dialog, this.id).subscribe(() => {
            this.getSolution();
        });
    }

    onBack(): void {
        window.history.back();
    }

    onGenerate() {
        this.error = null;
        this.solutionService.generateSolution({ id: this.solution.id })
            .subscribe(() => this.noticeHelper.showMessage('Generation completed'),
                error => this.onError(error));
    }

    onUpgrade() {
        this.error = null;
        this.solutionService.upgradeSolution({ id: this.solution.id })
            .subscribe(() => this.noticeHelper.showMessage('Upgrade completed'),
                error => this.onError(error));
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
        }
    }
}
