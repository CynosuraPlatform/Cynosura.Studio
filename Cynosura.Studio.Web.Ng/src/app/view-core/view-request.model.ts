import { ViewFilter } from './view-filter.model';
import { OrderDirection } from '../core/models/order-direction.model';

export class GetViews {
    solutionId: number;
    pageIndex?: number;
    pageSize?: number;
    filter?: ViewFilter;
    orderBy?: string;
    orderDirection?: OrderDirection;
}

export class GetView {
    solutionId: number;
    id: string;
}

export class UpdateView {
    solutionId: number;
    id: string;
    name: string;
}

export class CreateView {
    solutionId: number;
    name: string;
}

export class DeleteView {
    solutionId: number;
    id: string;
}
