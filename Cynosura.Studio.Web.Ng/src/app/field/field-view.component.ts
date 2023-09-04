import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { ConvertStringTo } from '../core/converter.helper';

import { Field } from '../field-core/field.model';
import { FieldEditComponent } from './field-edit.component';

@Component({
    selector: 'app-field-view',
    templateUrl: './field-view.component.html',
    styleUrls: ['./field-view.component.scss']
})
export class FieldViewComponent {
    id: string;
    field: Field;

    constructor(private dialog: MatDialog,
                private route: ActivatedRoute) {
    }

    onEdit() {
    }

    onBack(): void {
        window.history.back();
    }
}
