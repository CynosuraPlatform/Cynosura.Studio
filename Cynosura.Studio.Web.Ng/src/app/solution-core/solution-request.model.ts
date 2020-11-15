import { SolutionFilter } from './solution-filter.model';
import { OrderDirection } from '../core/models/order-direction.model';

export class GetSolutions {
    pageIndex?: number;
    pageSize?: number;
    filter?: SolutionFilter;
    orderBy?: string;
    orderDirection?: OrderDirection;
}

export class GetSolution {
    id: number;
}

export class ExportSolutions {
    filter?: SolutionFilter;
    orderBy?: string;
    orderDirection?: OrderDirection;
}

export class UpdateSolution {
    id: number;
    name: string;
    path: string;
}

export class CreateSolution {
    name: string;
    path: string;
}

export class DeleteSolution {
    id: number;
}

export class GenerateSolution {
    id: number;
}

export class UpgradeSolution {
    id: number;
}
