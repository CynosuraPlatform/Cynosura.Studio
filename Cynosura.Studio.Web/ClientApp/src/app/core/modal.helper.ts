import { Injectable } from "@angular/core";

import { Modal } from "ngx-modialog/plugins/bootstrap";

@Injectable()
export class ModalHelper {
    constructor(private modal: Modal) { }

    confirmDelete(): Promise<void> {
        const dialogRef = this.modal
            .confirm()
            .size("sm")
            .keyboard(27)
            .title("Delete?")
            .body("Are you sure you want to delete?")
            .okBtn("Delete")
            .cancelBtn("Cancel")
            .open();
        return dialogRef.result;
    }
}
