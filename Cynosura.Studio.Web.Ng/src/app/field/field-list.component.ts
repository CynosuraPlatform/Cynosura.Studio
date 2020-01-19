import { Component, OnInit, Input } from "@angular/core";
import { MatTableDataSource } from "@angular/material";

import { ModalHelper } from "../core/modal.helper";

import { Field, FieldType } from "../field-core/field.model";

import { Guid } from "../core/guid";
import { Error } from "../core/error.model";
import { NoticeHelper } from "../core/notice.helper";

@Component({
    selector: "app-field-list",
    templateUrl: "./field-list.component.html",
    styleUrls: ["./field-list.component.scss"]
})
export class FieldListComponent implements OnInit {

    columns = [
        "name",
        "displayName",
        "type",
        "entity",
        "enum",
        "action"
    ];

    FieldType = FieldType;

    @Input()
    solutionId: number;

    @Input()
    fields: Field[];

    dataSource: MatTableDataSource<Field>;

    field: Field;
    constructor(
        private modalHelper: ModalHelper,
        private noticeHelper: NoticeHelper
        ) {}

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource(this.fields);
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
        this.dataSource.data = this.fields;

        this.field = null;
    }

    delete(id: string): void {
        this.modalHelper.confirmDelete()
            .subscribe(() => {
                const foundField = this.findField(id);
                const index = this.fields.indexOf(foundField);
                this.fields.splice(index, 1);
                this.dataSource.data = this.fields;
            });
    }

    onError(error: Error) {
        if (error) {
            this.noticeHelper.showError(error);
        }
    }
}
