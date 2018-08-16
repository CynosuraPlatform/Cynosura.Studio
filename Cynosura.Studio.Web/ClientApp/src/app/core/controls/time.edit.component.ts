import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "time-edit",
    templateUrl: "./time.edit.component.html"
})
export class TimeEditComponent {
    @Input()
    value: string;

    @Output()
    valueChange = new EventEmitter<string>();

    @Input()
    name: string;

    @Input()
    label: string;

    onValueChange(value: string) {
        this.value = value;
        this.valueChange.emit(value);
    }
}
