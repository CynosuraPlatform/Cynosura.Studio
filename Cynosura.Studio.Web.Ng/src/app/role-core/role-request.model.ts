import { RoleFilter } from './role-filter.model';
import { OrderDirection } from '../core/models/order-direction.model';

export class GetRoles {
    pageIndex?: number;
    pageSize?: number;
    filter?: RoleFilter;
    orderBy?: string;
    orderDirection?: OrderDirection;
}

export class GetRole {
    id: number;
}

export class UpdateRole {
    id: number;
    name: string;
}

export class CreateRole {
    name: string;
}

export class DeleteRole {
    id: number;
}
