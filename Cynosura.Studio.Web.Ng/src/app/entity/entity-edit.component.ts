import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";

import { plural } from "pluralize";

import { Entity } from "../entity-core/entity.model";
import { EntityService } from "../entity-core/entity.service";
import { UpdateEntity, CreateEntity } from "../entity-core/entity-request.model";

import { Error } from "../core/error.model";
import { NoticeHelper } from "../core/notice.helper";
import { ConvertStringTo } from "../core/converter.helper";

@Component({
    selector: "app-entity-edit",
    templateUrl: "./entity-edit.component.html",
    styleUrls: ["./entity-edit.component.scss"]
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

    constructor(private entityService: EntityService,
                private route: ActivatedRoute,
                private router: Router,
                private fb: FormBuilder,
                private noticeHelper: NoticeHelper) {

        this.previousValue = "";
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id: string = params.id === "0" ? null : params.id;
            this.solutionId = ConvertStringTo.number(this.route.snapshot.queryParams.solutionId);
            this.getEntity(id);
        });

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
    private getEntity(id: string): void {
        this.id = id;
        if (!id) {
            this.entity = new Entity();
            this.entityForm.patchValue(this.entity);
        } else {
            this.entityService.getEntity({ solutionId: this.solutionId, id }).then(entity => {
                this.entity = entity;
                this.entityForm.patchValue(this.entity);
            });
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveEntity();
    }

    private saveEntity(): void {
        if (this.id) {
            const updateEntity: UpdateEntity = this.entityForm.value;
            updateEntity.solutionId = this.solutionId;
            updateEntity.properties = this.entity.properties;
            updateEntity.fields = this.entity.fields;
            this.entityService.updateEntity(updateEntity)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        } else {
            const createEntity: CreateEntity = this.entityForm.value;
            createEntity.solutionId = this.solutionId;
            createEntity.properties = this.entity.properties;
            createEntity.fields = this.entity.fields;
            this.entityService.createEntity(createEntity)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        }
    }

    generate(): void {
        this.entityService.generateEntity({ solutionId: this.solutionId, id: this.id })
            .then(
                () => { },
                error => this.onError(error)
            );
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.noticeHelper.showError(error);
            Error.setFormErrors(this.entityForm, error);
        }
    }
}
