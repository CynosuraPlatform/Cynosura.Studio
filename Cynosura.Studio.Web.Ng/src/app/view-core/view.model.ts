import { ViewFilter } from './view-filter.model';

export class View {
    id: string;
    name: string;
}

export class ViewListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new ViewFilter();
}
