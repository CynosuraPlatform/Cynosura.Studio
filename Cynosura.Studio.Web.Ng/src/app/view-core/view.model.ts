import { PageSettings } from '../core/page.model';
import { ViewFilter } from './view-filter.model';

export class View {
  id: string;
  name: string;
}

export class ViewListState {
  pageSize = PageSettings.pageSize;
  pageIndex = 0;
  filter = new ViewFilter();
}
