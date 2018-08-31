import { Component, OnInit, Input } from "@angular/core";

import { ModalHelper } from "../core/modal.helper";

import { Field, FieldType } from "../field-core/field.model";

import { Guid } from "../core/guid";
import { Error } from "../core/error.model";

@Component({
    selector: "field-list",
    templateUrl: "./list.component.html"
})
export class FieldListComponent implements OnInit {

    FieldType = FieldType;

    @Input()
    solutionId: number;

    @Input()
    fields: Field[];

    field: Field;

    error: Error;

    constructor(
        private modalHelper: ModalHelper
        ) {}

    ngOnInit(): void {

    }

    findField(id: string): Field {
        return this.fields.find(f => f.id === id);
    }

    edit(id: string): void {
        this.field = this.findField(id);
    }

    add(): void {
        this.field = new Field();
    }

    fieldSave(field: Field): void {
        if (field.id) {
            const foundField = this.findField(field.id);
            if (foundField) {
                const index = this.fields.indexOf(foundField);
                this.fields[index] = field;
            }
        } else {
            field.id = Guid.newGuid();
            this.fields.push(field);
        }

        this.field = null;
    }

    delete(id: string): void {
        this.modalHelper.confirmDelete()
            .then(() => {
                const foundField = this.findField(id);
                const index = this.fields.indexOf(foundField);
                this.fields.splice(index, 1);
            });
    }

}
