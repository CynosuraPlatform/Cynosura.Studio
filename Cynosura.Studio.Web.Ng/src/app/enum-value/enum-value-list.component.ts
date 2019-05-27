import { Component, OnInit, Input } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { PageEvent } from "@angular/material/paginator";
import { MatSnackBar, MatTableDataSource } from "@angular/material";

import { EnumValue } from "../enum-value-core/enum-value.model";
import { EnumValueFilter } from "../enum-value-core/enum-value-filter.model";

import { ModalHelper } from "../core/modal.helper";
import { Guid } from "../core/guid";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "app-enum-value-list",
    templateUrl: "./enum-value-list.component.html",
    styleUrls: ["./enum-value-list.component.scss"]
})
export class EnumValueListComponent implements OnInit {
    columns = [
        "name",
        "displayName",
        "value",
    ];
    @Input()
    solutionId: number;

    @Input()
    enumValues: EnumValue[];

    dataSource: MatTableDataSource<EnumValue>;

    enumValue: EnumValue;

    constructor(
        private modalHelper: ModalHelper,
        private snackBar: MatSnackBar
        ) {}

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource(this.enumValues);
    }

    findEnumValue(id: string): EnumValue {
        return this.enumValues.find(v => v.id === id);
    }

    edit(id: string): void {
        this.enumValue = this.findEnumValue(id);
    }

    add(): void {
        this.enumValue = new EnumValue();
    }

    enumValueSave(enumValue: EnumValue): void {
        if (enumValue.id) {
            const foundEnumValue = this.findEnumValue(enumValue.id);
            if (foundEnumValue) {
                const index = this.enumValues.indexOf(foundEnumValue);
                this.enumValues[index] = enumValue;
            }
        } else {
            enumValue.id = Guid.newGuid();
            this.enumValues.push(enumValue);
        }
        this.dataSource.data = this.enumValues;

        this.enumValue = null;
    }

    delete(id: string): void {
        this.modalHelper.confirmDelete()
            .subscribe(() => {
                const foundEnumValue = this.findEnumValue(id);
                const index = this.enumValues.indexOf(foundEnumValue);
                this.enumValues.splice(index, 1);
                this.dataSource.data = this.enumValues;
            });
    }

    onError(error: Error) {
        if (error) {
            this.snackBar.open(error.message, "Ok");
        }
    }
}
