import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "bool",
    templateUrl: "./bool.component.html"
})

export class BoolComponent {
    @Input()
    boolValue: boolean | null = null;

    @Output()
    boolValueChange = new EventEmitter<boolean>();

    onChangeObj(value: boolean) {
        this.boolValue = value;
        this.boolValueChange.emit(value);
    }
}
