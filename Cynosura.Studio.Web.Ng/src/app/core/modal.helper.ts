import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material";
import { Observable } from "rxjs";
import { filter } from "rxjs/operators";

import { ModalComponent } from "./modal.component";


@Injectable()
export class ModalHelper {
    constructor(private dialog: MatDialog) {
    }

    confirmDelete(): Observable<any> {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: "300px",
            data: {
                title: "Delete?",
                body: "Are you sure you want to delete?",
                okButton: "Delete",
                cancelButton: "Cancel"
            }
        });

        return dialogRef.afterClosed()
            .pipe(filter(res => res === true));
    }
}
