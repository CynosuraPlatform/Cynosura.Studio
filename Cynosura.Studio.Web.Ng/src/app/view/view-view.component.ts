import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { ConvertStringTo } from '../core/converter.helper';

import { View } from '../view-core/view.model';
import { ViewService } from '../view-core/view.service';
import { ViewEditComponent } from './view-edit.component';

@Component({
    selector: 'app-view-view',
    templateUrl: './view-view.component.html',
    styleUrls: ['./view-view.component.scss']
})
export class ViewViewComponent implements OnInit {
    id: string;
    view: View;
    solutionId: number;

    constructor(private dialog: MatDialog,
                private viewService: ViewService,
                private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            this.id = ConvertStringTo.string(params.id);
            this.solutionId = ConvertStringTo.number(this.route.snapshot.queryParams.solutionId);
            this.getView();
        });
    }

    private getView() {
        this.viewService.getView({ solutionId: this.solutionId, id: this.id })
            .subscribe(view => this.view = view);
    }

    onEdit() {
        ViewEditComponent.show(this.dialog, this.solutionId, this.id).subscribe(() => {
            this.getView();
        });
    }

    onBack(): void {
        window.history.back();
    }
}
