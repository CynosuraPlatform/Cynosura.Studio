import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { ConvertStringTo } from '../core/converter.helper';

import { EnumValue } from '../enum-value-core/enum-value.model';
import { EnumValueEditComponent } from './enum-value-edit.component';

@Component({
    selector: 'app-enum-value-view',
    templateUrl: './enum-value-view.component.html',
    styleUrls: ['./enum-value-view.component.scss']
})
export class EnumValueViewComponent {
    id: string;
    enumValue: EnumValue;

    constructor(private dialog: MatDialog,
                private route: ActivatedRoute) {
    }

    onEdit() {
    }

    onBack(): void {
        window.history.back();
    }
}
