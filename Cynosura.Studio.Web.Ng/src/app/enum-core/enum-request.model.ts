import { EnumFilter } from "./enum-filter.model";
import { OrderDirection } from "../core/models/order-direction.model";

export class GetEnums {
    solutionId: number;
    pageIndex?: number;
    pageSize?: number;
    filter?: EnumFilter;
    orderBy?: string;
    orderDirection?: OrderDirection;
}

export class GetEnum {
    solutionId: number;
    id: string;
}

export class UpdateEnum {
    solutionId: number;
    id: string;
    name: string;
    displayName: string;
}

export class CreateEnum {
    solutionId: number;
    name: string;
    displayName: string;
}

export class DeleteEnum {
    solutionId: number;
    id: string;
}

export class GenerateEnum {
    solutionId: number;
    id: string;
}
