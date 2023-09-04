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

import { Solution, SolutionListState } from '../solution-core/solution.model';
import { SolutionService } from '../solution-core/solution.service';
import { SolutionEditComponent } from './solution-edit.component';
import { SolutionOpenComponent } from './solution-open.component';

@Component({
  selector: 'app-solution-list',
  templateUrl: './solution-list.component.html',
  styleUrls: ['./solution-list.component.scss']
})
export class SolutionListComponent implements OnInit {
  content: Page<Solution>;
  pageSizeOptions = PageSettings.pageSizeOptions;
  defaultColumns = [
    'select',
    'name',
    'path',
    'templateName',
    'templateVersion',
    'action'
  ];
  columnDescriptions: ColumnDescription[] = [
    { name: 'select', isSystem: true },
    { name: 'name', displayName: this.translocoService.translate('Name') },
    { name: 'path', displayName: this.translocoService.translate('Path') },
    { name: 'templateName', displayName: this.translocoService.translate('Template Name') },
    { name: 'templateVersion', displayName: this.translocoService.translate('Template Version') },
    { name: 'action', isSystem: true },
  ];
  columns = this.storedValueService.getStoredValue('solutionColumns', this.defaultColumns,
    ColumnDescription.filter(this.columnDescriptions));
  selectedIds = new Set<number>();

  @Input()
  state: SolutionListState;

  @Input()
  baseRoute = '/solution';

  constructor(
    private dialog: MatDialog,
    private modalHelper: ModalHelper,
    private solutionService: SolutionService,
    private noticeHelper: NoticeHelper,
    private translocoService: TranslocoService,
    private storedValueService: StoredValueService
  ) {
  }

  ngOnInit() {
    this.getSolutions();
  }

  private getSolutions() {
    this.selectedIds = new Set<number>();
    this.solutionService.getSolutions({
      pageIndex: this.state.pageIndex,
      pageSize: this.state.pageSize,
      filter: this.state.filter,
      orderBy: this.state.orderBy,
      orderDirection: this.state.orderDirection
    }).subscribe(content => this.content = content);
  }

  onSearch() {
    this.state.pageIndex = 0;
    this.getSolutions();
  }

  onReset() {
    this.state.pageIndex = 0;
    this.state.filter.text = null;
    this.getSolutions();
  }

  onCreate() {
    SolutionEditComponent.show(this.dialog, null).subscribe(() => {
      this.getSolutions();
    });
  }

  onExport(): void {
    this.solutionService.exportSolutions({ filter: this.state.filter })
      .subscribe(file => {
        file.download();
      });
  }

  onEdit(id: number) {
    SolutionEditComponent.show(this.dialog, id).subscribe(() => {
      this.getSolutions();
    });
  }

  onDelete(id: number) {
    this.modalHelper.confirmDelete()
      .pipe(
        mergeMap(() => this.solutionService.deleteSolution({ id }))
      )
      .subscribe(() => this.getSolutions(),
        error => this.onError(error));
  }

  onDeleteSelected() {
    this.modalHelper.confirmDelete()
      .pipe(
        mergeMap(() => forkJoin([...this.selectedIds]
          .map(id => this.solutionService.deleteSolution({ id })
            .pipe(
              catchError(error => { this.onError(error); return of({}); })
            ))))
      )
      .subscribe(() => this.getSolutions());
  }

  onPage(page: PageEvent) {
    this.state.pageIndex = page.pageIndex;
    this.state.pageSize = page.pageSize;
    this.getSolutions();
  }

  onSortChange(sortState: Sort) {
    this.state.orderDirection = OrderDirectionManager.getOrderDirectionBySort(sortState);
    this.state.orderBy = OrderDirectionManager.getOrderByBySort(sortState);
    this.getSolutions();
  }

  onError(error: Error) {
    if (error) {
      this.noticeHelper.showError(error);
    }
  }

  open() {
    this.dialog.open(SolutionOpenComponent, {
      width: '600px'
    });
  }
}
