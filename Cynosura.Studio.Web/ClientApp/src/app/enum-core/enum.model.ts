
import { EnumValue } from "../enum-value-core/enum-value.model";

export class Enum {
    id: string;

    name: string;
    displayName: string;
    values: EnumValue[];

    constructor() {
        this.values = new Array<EnumValue>();
    }
}
