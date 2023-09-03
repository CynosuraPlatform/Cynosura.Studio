import { Component, OnInit, Input } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { forkJoin, of } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';
import { TranslocoService } from '@ngneat/transloco';

import { ModalHelper } from '../core/modal.helper';
import { StoreService } from '../core/store.service';
import { Error } from '../core/error.model';
import { Page, PageSettings } from '../core/page.model';
import { NoticeHelper } from '../core/notice.helper';
import { OrderDirectionManager } from '../core/models/order-direction.model';
import { ColumnDescription } from '../core/column-settings.component';
import { StoredValueService } from '../core/stored-value.service';

import { Enum, EnumListState } from '../enum-core/enum.model';
import { EnumService } from '../enum-core/enum.service';
import { EnumEditComponent } from './enum-edit.component';

@Component({
  selector: 'app-enum-list',
  templateUrl: './enum-list.component.html',
  styleUrls: ['./enum-list.component.scss']
})
export class EnumListComponent implements OnInit {
  content: Page<Enum>;
  pageSizeOptions = PageSettings.pageSizeOptions;
  defaultColumns = [
    'select',
    'name',
    'displayName',
    'action'
  ];
  private innerSolutionId: number;
  get solutionId(): number {
    if (!this.innerSolutionId) {
      this.innerSolutionId = this.storeService.get('solutionId', 0);
    }
    return this.innerSolutionId;
  }
  set solutionId(val: number) {
    this.innerSolutionId = val;
    this.storeService.set('solutionId', this.innerSolutionId);
    this.getEnums();
  }
  columns = this.storedValueService.getStoredValue('enumColumns', this.defaultColumns);
  columnDescriptions: ColumnDescription[] = [
    { name: 'select', isSystem: true },
    { name: 'name', displayName: this.translocoService.translate('Name') },
    { name: 'displayName', displayName: this.translocoService.translate('Display Name') },
    { name: 'action', isSystem: true },
  ];
  selectedIds = new Set<string>();

  @Input()
  state: EnumListState;

  @Input()
  baseRoute = '/enum';

  constructor(
    private dialog: MatDialog,
    private modalHelper: ModalHelper,
    private enumService: EnumService,
    private storeService: StoreService,
    private noticeHelper: NoticeHelper,
    private translocoService: TranslocoService,
    private storedValueService: StoredValueService
  ) {
  }

  ngOnInit(): void {
    this.getEnums();
  }

  private getEnums() {
    this.selectedIds = new Set<string>();
    if (this.solutionId) {
      this.enumService.getEnums({
        solutionId: this.solutionId,
        pageIndex: this.state.pageIndex,
        pageSize: this.state.pageSize,
      filter: this.state.filter,
      orderBy: this.state.orderBy,
      orderDirection: this.state.orderDirection
      }).subscribe(content => this.content = content);
    } else {
      this.content = null;
    }
  }

  onSearch() {
    this.state.pageIndex = 0;
    this.getEnums();
  }

  onReset() {
    this.state.pageIndex = 0;
    this.state.filter.text = null;
    this.getEnums();
  }

  onCreate() {
    EnumEditComponent.show(this.dialog, this.solutionId, null).subscribe(() => {
      this.getEnums();
    });
  }

  onExport(): void {
    this.enumService.exportEnums({ filter: this.state.filter })
      .subscribe(file => {
        file.download();
      });
  }

  onEdit(id: string) {
    EnumEditComponent.show(this.dialog, this.solutionId, id).subscribe(() => {
      this.getEnums();
    });
  }

  onDelete(id: string) {
    this.modalHelper.confirmDelete()
      .pipe(
        mergeMap(() => this.enumService.deleteEnum({ solutionId: this.solutionId, id }))
      )
      .subscribe(() => this.getEnums(),
        error => this.onError(error));
  }

  onDeleteSelected() {
    this.modalHelper.confirmDelete()
      .pipe(
        mergeMap(() => forkJoin([...this.selectedIds]
          .map(id => this.enumService.deleteEnum({ solutionId: this.solutionId, id })
            .pipe(
              catchError(error => { this.onError(error); return of({}); })
            ))))
      )
      .subscribe(() => this.getEnums());
  }

  onPage(page: PageEvent) {
    this.state.pageIndex = page.pageIndex;
    this.state.pageSize = page.pageSize;
    this.getEnums();
  }

  onSortChange(sortState: Sort) {
    this.state.orderDirection = OrderDirectionManager.getOrderDirectionBySort(sortState);
    this.state.orderBy = OrderDirectionManager.getOrderByBySort(sortState);
    this.getEnums();
  }

  onError(error: Error) {
    if (error) {
      this.noticeHelper.showError(error);
    }
  }
}
