
export class EnumValue {
    id: string;
    name: string;
    displayName: string;
    value: number;
    properties: { [k: string]: any };

    constructor() {
        this.properties = {};
    }
}

