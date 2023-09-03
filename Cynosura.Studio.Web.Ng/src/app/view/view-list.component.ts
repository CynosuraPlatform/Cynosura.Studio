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

import { View, ViewListState } from '../view-core/view.model';
import { ViewService } from '../view-core/view.service';
import { ViewEditComponent } from './view-edit.component';

@Component({
  selector: 'app-view-list',
  templateUrl: './view-list.component.html',
  styleUrls: ['./view-list.component.scss']
})
export class ViewListComponent implements OnInit {
  content: Page<View>;
  pageSizeOptions = PageSettings.pageSizeOptions;
  defaultColumns = [
    'select',
    'name',
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
    this.getViews();
  }
  columns = this.storedValueService.getStoredValue('viewColumns', this.defaultColumns);
  columnDescriptions: ColumnDescription[] = [
    { name: 'select', isSystem: true },
    { name: 'name', displayName: this.translocoService.translate('Name') },
    { name: 'action', isSystem: true },
  ];
  selectedIds = new Set<string>();

  @Input()
  state: ViewListState;

  @Input()
  baseRoute = '/view';

  constructor(
    private dialog: MatDialog,
    private modalHelper: ModalHelper,
    private viewService: ViewService,
    private storeService: StoreService,
    private noticeHelper: NoticeHelper,
    private translocoService: TranslocoService,
    private storedValueService: StoredValueService
  ) {
  }

  ngOnInit() {
    this.getViews();
  }

  private getViews() {
    this.selectedIds = new Set<string>();
    if (this.solutionId) {
      this.viewService.getViews({
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
    this.getViews();
  }

  onReset() {
    this.state.pageIndex = 0;
    this.state.filter.text = null;
    this.getViews();
  }

  onCreate() {
    ViewEditComponent.show(this.dialog, this.solutionId, null).subscribe(() => {
      this.getViews();
    });
  }

  onExport(): void {
    this.viewService.exportViews({ solutionId: this.solutionId, filter: this.state.filter })
      .subscribe(file => {
        file.download();
      });
  }

  onEdit(id: string) {
    ViewEditComponent.show(this.dialog, this.solutionId, id).subscribe(() => {
      this.getViews();
    });
  }

  onDelete(id: string) {
    this.modalHelper.confirmDelete()
      .pipe(
        mergeMap(() => this.viewService.deleteView({ solutionId: this.solutionId, id }))
      )
      .subscribe(() => this.getViews(),
        error => this.onError(error));
  }

  onDeleteSelected() {
    this.modalHelper.confirmDelete()
      .pipe(
        mergeMap(() => forkJoin([...this.selectedIds]
          .map(id => this.viewService.deleteView({ solutionId: this.solutionId, id })
            .pipe(
              catchError(error => { this.onError(error); return of({}); })
            ))))
      )
      .subscribe(() => this.getViews());
  }

  onPage(page: PageEvent) {
    this.state.pageIndex = page.pageIndex;
    this.state.pageSize = page.pageSize;
    this.getViews();
  }

  onSortChange(sortState: Sort) {
    this.state.orderDirection = OrderDirectionManager.getOrderDirectionBySort(sortState);
    this.state.orderBy = OrderDirectionManager.getOrderByBySort(sortState);
    this.getViews();
  }

  onError(error: Error) {
    if (error) {
      this.noticeHelper.showError(error);
    }
  }
}
