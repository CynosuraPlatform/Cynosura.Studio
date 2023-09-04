import { Component, OnInit, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';

import { ModalHelper } from '../core/modal.helper';
import { Error } from '../core/error.model';
import { Guid } from '../core/guid';
import { NoticeHelper } from '../core/notice.helper';
import { ColumnDescription } from '../core/column-settings.component';
import { StoredValueService } from '../core/stored-value.service';

import { EnumValue } from '../enum-value-core/enum-value.model';
import { EnumValueEditComponent } from './enum-value-edit.component';

@Component({
  selector: 'app-enum-value-list',
  templateUrl: './enum-value-list.component.html',
  styleUrls: ['./enum-value-list.component.scss']
})
export class EnumValueListComponent implements OnInit {
  defaultColumns = [
    'name',
    'displayName',
    'value',
    'action'
  ];
  columnDescriptions: ColumnDescription[] = [
    { name: 'name', displayName: this.translocoService.translate('Name') },
    { name: 'displayName', displayName: this.translocoService.translate('Display Name') },
    { name: 'value', displayName: this.translocoService.translate('Value') },
    { name: 'action', isSystem: true },
  ];
  columns = this.storedValueService.getStoredValue('enumValueColumns', this.defaultColumns,
    ColumnDescription.filter(this.columnDescriptions));

  @Input()
  solutionId: number;

  @Input()
  enumValues: EnumValue[];

  dataSource: MatTableDataSource<EnumValue>;

  constructor(
    private modalHelper: ModalHelper,
    private noticeHelper: NoticeHelper,
    private dialog: MatDialog,
    private translocoService: TranslocoService,
    private storedValueService: StoredValueService
  ) {
  }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.enumValues);
  }

  findEnumValue(id: string): EnumValue {
    return this.enumValues.find(v => v.id === id);
  }

  onEdit(id: string): void {
    this.openEditDialog(this.findEnumValue(id));
  }

  onCreate() {
    this.openEditDialog(new EnumValue());
  }

  openEditDialog(enumValue: EnumValue) {
    const dialogRef = this.dialog.open(EnumValueEditComponent, {
      width: '600px',
      data: { enumValue: enumValue, solutionId: this.solutionId }
    });
    dialogRef.afterClosed().subscribe(result => {
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

  onDelete(id: string): void {
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
