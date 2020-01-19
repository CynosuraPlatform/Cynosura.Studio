import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
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

    confirm(message: string, title: string, okButton: string): Observable<any> {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: "300px",
            data: {
                title: title,
                body: message,
                okButton: okButton,
                cancelButton: "Cancel"
            }
        });

        return dialogRef.afterClosed()
            .pipe(filter(res => res === true));
    }

    alert(message: string, title: string, okButton: string): Observable<any> {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: "300px",
            data: {
                title: title,
                body: message,
                okButton: okButton,
            }
        });

        return dialogRef.afterClosed()
            .pipe(filter(res => res === true));
    }
}
