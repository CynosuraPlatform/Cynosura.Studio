import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { Error } from '../core/error.model';
import { NoticeHelper } from '../core/notice.helper';
import { ConvertStringTo } from '../core/converter.helper';

import { Entity } from '../entity-core/entity.model';
import { EntityService } from '../entity-core/entity.service';
import { EntityEditComponent } from './entity-edit.component';

@Component({
    selector: 'app-entity-view',
    templateUrl: './entity-view.component.html',
    styleUrls: ['./entity-view.component.scss']
})
export class EntityViewComponent implements OnInit {
    id: string;
    entity: Entity;
    error: Error;
    solutionId: number;

    constructor(private dialog: MatDialog,
                private entityService: EntityService,
                private route: ActivatedRoute,
                private noticeHelper: NoticeHelper) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            this.id = ConvertStringTo.string(params.id);
            this.solutionId = ConvertStringTo.number(this.route.snapshot.queryParams.solutionId);
            this.getEntity();
        });
    }

    private getEntity() {
        this.entityService.getEntity({ solutionId: this.solutionId, id: this.id })
            .subscribe(entity => this.entity = entity);
    }

    onEdit() {
        EntityEditComponent.show(this.dialog, this.solutionId, this.id).subscribe(() => {
            this.getEntity();
        });
    }

    onBack(): void {
        window.history.back();
    }

    onGenerate() {
        this.entityService.generateEntity({ solutionId: this.solutionId, id: this.id })
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
