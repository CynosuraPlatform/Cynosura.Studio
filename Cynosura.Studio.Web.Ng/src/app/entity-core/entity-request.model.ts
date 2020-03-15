import { EntityFilter } from './entity-filter.model';
import { OrderDirection } from '../core/models/order-direction.model';
import { Field } from '../field-core/field.model';

export class GetEntities {
    solutionId: number;
    pageIndex?: number;
    pageSize?: number;
    filter?: EntityFilter;
    orderBy?: string;
    orderDirection?: OrderDirection;
}

export class GetEntity {
    solutionId: number;
    id: string;
}

export class UpdateEntity {
    solutionId: number;
    id: string;
    name: string;
    pluralName: string;
    displayName: string;
    pluralDisplayName: string;
    isAbstract: boolean;
    baseEntityId: string;

    properties: { [k: string]: any};
    fields: Field[];
}

export class CreateEntity {
    solutionId: number;
    name: string;
    pluralName: string;
    displayName: string;
    pluralDisplayName: string;
    isAbstract: boolean;
    baseEntityId: string;

    properties: { [k: string]: any};
    fields: Field[];
}

export class DeleteEntity {
    solutionId: number;
    id: string;
}

export class GenerateEntity {
    solutionId: number;
    id: string;
}

