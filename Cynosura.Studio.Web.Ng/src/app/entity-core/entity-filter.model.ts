import { EntityFilter as CoreEntityFilter } from '../core/models/entity-filter.model';

export class EntityFilter extends CoreEntityFilter {
    name?: string;
    pluralName?: string;
    displayName?: string;
    pluralDisplayName?: string;
    isAbstract?: boolean;
    baseEntityId?: string;
}
