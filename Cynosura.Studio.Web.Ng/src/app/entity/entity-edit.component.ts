import { Component, Input, OnInit, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { filter } from 'rxjs/operators';
import { plural } from 'pluralize';

import { Error } from '../core/error.model';
import { NoticeHelper } from '../core/notice.helper';
import { ConvertStringTo } from '../core/converter.helper';

import { Entity } from '../entity-core/entity.model';
import { EntityService } from '../entity-core/entity.service';
import { UpdateEntity, CreateEntity } from '../entity-core/entity-request.model';

class DialogData {
    id: string;
    solutionId: number;
}
@Component({
    selector: 'app-entity-edit',
    templateUrl: './entity-edit.component.html',
    styleUrls: ['./entity-edit.component.scss']
})
export class EntityEditComponent implements OnInit {
    id: string;
    entityForm = this.fb.group({
        id: [],
        name: [],
        pluralName: [],
        displayName: [],
        pluralDisplayName: [],
        isAbstract: [],
        baseEntityId: []
    });
    entity: Entity;
    error: Error;
    solutionId: number;
    previousValue: string;

    constructor(public dialogRef: MatDialogRef<EntityEditComponent>,
                @Inject(MAT_DIALOG_DATA) public data: DialogData,
                private entityService: EntityService,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) {
        this.id = data.id;
        this.solutionId = data.solutionId;
    }

    static show(dialog: MatDialog, solutionId: number, id: string): Observable<any> {
        const dialogRef = dialog.open(EntityEditComponent, {
            width: '800px',
            data: {
                solutionId: solutionId,
                id: id
            }
        });
        return dialogRef.afterClosed()
            .pipe(filter(res => res === true));
    }

    ngOnInit(): void {
        this.getEntity();

        this.entityForm.controls.name.valueChanges.subscribe((value: string) => {
            if (!this.entityForm.controls.pluralName.value ||
                (plural(this.previousValue) === this.entityForm.controls.pluralName.value)) {
                this.entityForm.controls.pluralName.setValue(
                    plural(value)
                );
            }
            this.previousValue = value;
        });
    }

    private getEntity() {
        const getEntity$ = !this.id ?
            of(new Entity()) :
            this.entityService.getEntity({ solutionId: this.solutionId, id: this.id });
        getEntity$.subscribe(entity => {
            this.entity = entity;
            this.entityForm.patchValue(this.entity);
        });
    }

    onSave(): void {
        this.saveEntity();
    }

    private saveEntity() {
        let saveEntity$: Observable<{}>;
        if (this.id) {
            const updateEntity: UpdateEntity = this.entityForm.value;
            updateEntity.solutionId = this.solutionId;
            updateEntity.properties = this.entity.properties;
            updateEntity.fields = this.entity.fields;
            saveEntity$ = this.entityService.updateEntity(updateEntity);
        } else {
            const createEntity: CreateEntity = this.entityForm.value;
            createEntity.solutionId = this.solutionId;
            createEntity.properties = this.entity.properties;
            createEntity.fields = this.entity.fields;
            saveEntity$ = this.entityService.createEntity(createEntity);
        }
        saveEntity$.subscribe(() => this.dialogRef.close(true),
            error => this.onError(error));
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.entityForm, error);
        }
    }
}
