import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "time-edit",
    templateUrl: "./time.edit.component.html"
})
export class TimeEditComponent {
    @Input()
    value: string;

    private formattedDateLocal: Date;

    get formattedDate(): Date {
        if (this.value && !this.formattedDateLocal) {
            this.formattedDateLocal = new Date(`2000-01-01T${this.value}`);
        }
        return this.formattedDateLocal;
    }
    set formattedDate(value: Date) {
        this.formattedDateLocal = value;
        if (value)
            this.value = value.toTimeString().substring(0, 5);
        else
            this.value = null;
    }

    @Output()
    valueChange = new EventEmitter<string>();

    @Input()
    name: string;

    @Input()
    label: string;

    onFormattedDateChange(value: Date) {
        this.formattedDate = value;
        this.valueChange.emit(this.value);
    }
}
