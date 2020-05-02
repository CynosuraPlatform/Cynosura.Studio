
import { Field } from '../field-core/field.model';

export class Entity {
    id: string;
    name: string;
    pluralName: string;
    displayName: string;
    isAbstract = false;
    baseEntityId: string;

    properties: { [k: string]: any};

    pluralDisplayName: string;
    fields: Field[];

    constructor() {
        this.fields = new Array<Field>();
        this.properties = {};
    }
}
