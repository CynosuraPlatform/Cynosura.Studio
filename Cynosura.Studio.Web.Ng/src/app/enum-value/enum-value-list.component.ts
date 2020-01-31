import { Component, OnInit, Input } from "@angular/core";
import { MatTableDataSource, MatDialog } from "@angular/material";

import { ModalHelper } from "../core/modal.helper";
import { Guid } from "../core/guid";
import { Error } from "../core/error.model";
import { NoticeHelper } from "../core/notice.helper";
import { EnumValue } from "../enum-value-core/enum-value.model";
import { EnumValueEditComponent } from "./enum-value-edit.component";

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
        "action"
    ];
    @Input()
    solutionId: number;

    @Input()
    enumValues: EnumValue[];

    dataSource: MatTableDataSource<EnumValue>;

    constructor(
        private modalHelper: ModalHelper,
        private noticeHelper: NoticeHelper,
        private dialog: MatDialog,
        ) {}

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource(this.enumValues);
    }

    findEnumValue(id: string): EnumValue {
        return this.enumValues.find(v => v.id === id);
    }

    edit(id: string): void {
        this.openEditDialog(this.findEnumValue(id));
    }

    add(): void {
        this.openEditDialog(new EnumValue());
    }

    openEditDialog(enumValue: EnumValue): Promise<any> {
        const dialogRef = this.dialog.open(EnumValueEditComponent, {
            width: "600px",
            data: { enumValue: enumValue, solutionId: this.solutionId }
        });
        return dialogRef.afterClosed().toPromise().then(result => {
            if (result) {
                this.enumValueSave(result);
            }
        });
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
            this.noticeHelper.showError(error);
        }
    }
}
