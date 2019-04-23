import { UserFilter } from "./user-filter.model";
import { OrderDirection } from "../core/models/order-direction.model";

export class GetUsers {
    pageIndex?: number;
    pageSize?: number;
    filter?: UserFilter;
    orderBy?: string;
    orderDirection?: OrderDirection;
}

export class GetUser {
    id: number;
}

export class UpdateUser {
    id: number;
    password?: string;
    confirmPassword?: string;
    roleIds: number[];
}

export class CreateUser {
    email: string;
    password?: string;
    confirmPassword?: string;
    roleIds: number[];
}

export class DeleteUser {
    id: number;
}
