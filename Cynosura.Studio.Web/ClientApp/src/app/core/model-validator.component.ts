import { Component, Input } from "@angular/core";

@Component({
    selector: "model-validator",
    templateUrl: "./model-validator.component.html"
})

export class ModelValidatorComponent {
    @Input()
    model: Object | null = null;
}
