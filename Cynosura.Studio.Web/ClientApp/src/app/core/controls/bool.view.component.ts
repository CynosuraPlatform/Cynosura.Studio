import { Component, Input } from "@angular/core";

@Component({
    selector: "bool-view",
    templateUrl: "./bool.view.component.html"
})
export class BoolViewComponent {
    @Input()
    value: boolean;
}
