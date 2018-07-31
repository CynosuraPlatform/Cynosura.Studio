import { Component, OnInit, Input } from "@angular/core";

import { Modal } from "ngx-modialog/plugins/bootstrap";

import { Field, FieldType } from "./field.model";

import { Guid } from "../core/guid";
import { Error } from "../core/error.model";

@Component({
    selector: "field-list",
    templateUrl: "./list.component.html"
})
export class FieldListComponent implements OnInit {

    FieldType = FieldType;

    @Input()
    fields: Field[];

    field: Field;

    error: Error;

    constructor(
        private modal: Modal
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
        const dialogRef = this.modal
            .confirm()
            .size("sm")
            .keyboard(27)
            .title("Удалить?")
            .body("Действительно хотите удалить?")
            .okBtn("Удалить")
            .cancelBtn("Отмена")
            .open();
        dialogRef.result.then(dialog => dialog.result)
            .then(() => {
                const foundField = this.findField(id);
                const index = this.fields.indexOf(foundField);
                this.fields.splice(index, 1);
            })
            .catch(() => {});
    }

}
