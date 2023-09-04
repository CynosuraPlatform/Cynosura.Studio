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

import { Entity, EntityListState } from '../entity-core/entity.model';
import { EntityService } from '../entity-core/entity.service';
import { EntityEditComponent } from './entity-edit.component';

@Component({
  selector: 'app-entity-list',
  templateUrl: './entity-list.component.html',
  styleUrls: ['./entity-list.component.scss']
})
export class EntityListComponent implements OnInit {
  content: Page<Entity>;
  pageSizeOptions = PageSettings.pageSizeOptions;
  defaultColumns = [
    'select',
    'name',
    'pluralName',
    'displayName',
    'pluralDisplayName',
    'isAbstract',
    'baseEntity',
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
    this.getEntities();
  }
  columnDescriptions: ColumnDescription[] = [
    { name: 'select', isSystem: true },
    { name: 'name', displayName: this.translocoService.translate('Name') },
    { name: 'pluralName', displayName: this.translocoService.translate('Plural Name') },
    { name: 'displayName', displayName: this.translocoService.translate('Display Name') },
    { name: 'pluralDisplayName', displayName: this.translocoService.translate('Plural Display Name') },
    { name: 'isAbstract', displayName: this.translocoService.translate('Abstract') },
    { name: 'baseEntity', displayName: this.translocoService.translate('Base Entity') },
    { name: 'action', isSystem: true },
  ];
  columns = this.storedValueService.getStoredValue('entityColumns', this.defaultColumns,
    ColumnDescription.filter(this.columnDescriptions));
  selectedIds = new Set<string>();

  @Input()
  state: EntityListState;

  @Input()
  baseRoute = '/entity';

  constructor(
    private dialog: MatDialog,
    private modalHelper: ModalHelper,
    private entityService: EntityService,
    private storeService: StoreService,
    private noticeHelper: NoticeHelper,
    private translocoService: TranslocoService,
    private storedValueService: StoredValueService
  ) {
  }

  ngOnInit(): void {
    this.getEntities();
  }

  private getEntities() {
    this.selectedIds = new Set<string>();
    if (this.solutionId) {
      this.entityService.getEntities({
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
    this.getEntities();
  }

  onReset() {
    this.state.pageIndex = 0;
    this.state.filter.text = null;
    this.getEntities();
  }

  onCreate(): void {
    EntityEditComponent.show(this.dialog, this.solutionId, null).subscribe(() => {
      this.getEntities();
    });
  }

  onExport(): void {
    this.entityService.exportEntities({ filter: this.state.filter })
      .subscribe(file => {
        file.download();
      });
  }

  onEdit(id: string) {
    EntityEditComponent.show(this.dialog, this.solutionId, id).subscribe(() => {
      this.getEntities();
    });
  }

  onDelete(id: string) {
    this.modalHelper.confirmDelete()
      .pipe(
        mergeMap(() => this.entityService.deleteEntity({ solutionId: this.solutionId, id }))
      )
      .subscribe(() => this.getEntities(),
        error => this.onError(error));
  }

  onDeleteSelected() {
    this.modalHelper.confirmDelete()
      .pipe(
        mergeMap(() => forkJoin([...this.selectedIds]
          .map(id => this.entityService.deleteEntity({ solutionId: this.solutionId, id })
            .pipe(
              catchError(error => { this.onError(error); return of({}); })
            ))))
      )
      .subscribe(() => this.getEntities());
  }

  onPage(page: PageEvent) {
    this.state.pageIndex = page.pageIndex;
    this.state.pageSize = page.pageSize;
    this.getEntities();
  }

  onSortChange(sortState: Sort) {
    this.state.orderDirection = OrderDirectionManager.getOrderDirectionBySort(sortState);
    this.state.orderBy = OrderDirectionManager.getOrderByBySort(sortState);
    this.getEntities();
  }

  onError(error: Error) {
    if (error) {
      this.noticeHelper.showError(error);
    }
  }
}
