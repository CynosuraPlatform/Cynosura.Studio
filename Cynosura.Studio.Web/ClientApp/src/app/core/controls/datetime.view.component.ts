import { Component, Input } from "@angular/core";

@Component({
    selector: "datetime-view",
    templateUrl: "./datetime.view.component.html"
})
export class DateTimeViewComponent {
    @Input()
    value: Date;
}
