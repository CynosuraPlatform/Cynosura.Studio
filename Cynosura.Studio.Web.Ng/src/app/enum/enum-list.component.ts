import { Component, OnInit, Input } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { mergeMap } from 'rxjs/operators';

import { ModalHelper } from '../core/modal.helper';
import { StoreService } from '../core/store.service';
import { Error } from '../core/error.model';
import { Page, PageSettings } from '../core/page.model';
import { NoticeHelper } from '../core/notice.helper';
import { OrderDirectionManager } from '../core/models/order-direction.model';

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
  columns = [
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

  @Input()
  state: EnumListState;

  @Input()
  baseRoute = '/enum';

  constructor(
    private dialog: MatDialog,
    private modalHelper: ModalHelper,
    private enumService: EnumService,
    private storeService: StoreService,
    private noticeHelper: NoticeHelper
  ) {
  }

  ngOnInit(): void {
    this.getEnums();
  }

  private getEnums() {
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
