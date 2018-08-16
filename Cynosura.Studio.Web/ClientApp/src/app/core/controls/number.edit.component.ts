import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "number-edit",
    templateUrl: "./number.edit.component.html"
})
export class NumberEditComponent {
    @Input()
    value: number;

    @Output()
    valueChange = new EventEmitter<number>();

    @Input()
    name: string;

    @Input()
    label: string;

    onValueChange(value: number) {
        this.value = value;
        this.valueChange.emit(value);
    }
}
