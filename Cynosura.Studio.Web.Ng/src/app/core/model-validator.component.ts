import { Component, Input } from "@angular/core";

@Component({
    selector: "app-model-validator",
    templateUrl: "./model-validator.component.html"
})

export class ModelValidatorComponent {
    @Input()
    model: any;
}
