import { EntityFilter } from "../core/models/entity-filter.model";

export class EnumValueFilter extends EntityFilter {
    name?: string;
    displayName?: string;
    valueFrom?: number;
    valueTo?: number;
    enumId?: number;
}
