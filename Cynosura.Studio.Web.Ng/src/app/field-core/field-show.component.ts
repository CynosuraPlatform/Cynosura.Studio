import { Component, Input } from "@angular/core";

import { Field } from "./field.model";

@Component({
    selector: "app-field-show",
    templateUrl: "./field-show.component.html"
})

export class FieldShowComponent {
    @Input()
    value: Field;
}
