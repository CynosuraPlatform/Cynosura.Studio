import { Component, OnInit, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { TranslocoService } from '@ngneat/transloco';

import { ModalHelper } from '../core/modal.helper';
import { Guid } from '../core/guid';
import { Error } from '../core/error.model';
import { NoticeHelper } from '../core/notice.helper';
import { ColumnDescription } from '../core/column-settings.component';
import { StoredValueService } from '../core/stored-value.service';

import { Field, FieldType } from '../field-core/field.model';
import { FieldEditComponent } from './field-edit.component';

@Component({
  selector: 'app-field-list',
  templateUrl: './field-list.component.html',
  styleUrls: ['./field-list.component.scss']
})
export class FieldListComponent implements OnInit {
  defaultColumns = [
    'name',
    'displayName',
    'type',
    'entity',
    'enum',
    'action'
  ];

  FieldType = FieldType;
  columnDescriptions: ColumnDescription[] = [
    { name: 'name', displayName: this.translocoService.translate('Name') },
    { name: 'displayName', displayName: this.translocoService.translate('Display Name') },
    { name: 'size', displayName: this.translocoService.translate('Size') },
    { name: 'entity', displayName: this.translocoService.translate('Entity') },
    { name: 'isRequired', displayName: this.translocoService.translate('Required') },
    { name: 'enum', displayName: this.translocoService.translate('Enum') },
    { name: 'isSystem', displayName: this.translocoService.translate('System') },
    { name: 'action', isSystem: true },
  ];
  columns = this.storedValueService.getStoredValue('fieldColumns', this.defaultColumns,
    ColumnDescription.filter(this.columnDescriptions));

  @Input()
  solutionId: number;

  @Input()
  fields: Field[];

  dataSource: MatTableDataSource<Field>;

  constructor(
    private modalHelper: ModalHelper,
    private noticeHelper: NoticeHelper,
    private dialog: MatDialog,
    private translocoService: TranslocoService,
    private storedValueService: StoredValueService
  ) {
  }


  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.fields);
  }

  findField(id: string): Field {
    return this.fields.find(f => f.id === id);
  }

  onEdit(id: string): void {
    this.openEditDialog(this.findField(id));
  }

  onCreate() {
    this.openEditDialog(new Field());
  }

  openEditDialog(field: Field) {
    const dialogRef = this.dialog.open(FieldEditComponent, {
      width: '600px',
      data: { field: field, solutionId: this.solutionId }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.fieldSave(result);
      }
    });
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
  }

  onDelete(id: string): void {
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
