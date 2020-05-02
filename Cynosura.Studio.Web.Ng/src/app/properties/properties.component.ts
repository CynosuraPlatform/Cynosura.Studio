import { Component, OnInit, Input } from '@angular/core';
import { MatDialog } from '@angular/material';

import { PropertiesPopupComponent } from './properties-popup.component';

@Component({
    selector: 'app-properties',
    templateUrl: './properties.component.html',
    styleUrls: ['./properties.component.css']
})
export class PropertiesComponent {

    @Input()
    target: string;

    @Input()
    properties: { [k: string]: any };
    constructor(
        private dialog: MatDialog
    ) { }

    open() {
        this.dialog.open(PropertiesPopupComponent, {
            width: '600px',
            data: {
                properties: this.properties,
                target: this.target,
            }
        });
    }
}
