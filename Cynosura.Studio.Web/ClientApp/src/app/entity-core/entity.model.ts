
import { Field } from "../field-core/field.model";

export class Entity {
	id: string;
	name: string;
	pluralName: string;
	displayName: string;
    pluralDisplayName: string;
    fields: Field[];

    constructor() {
        this.fields = new Array<Field>();
    }
}
