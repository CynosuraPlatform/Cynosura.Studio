import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "date-edit",
    templateUrl: "./date.edit.component.html"
})
export class DateEditComponent {
    @Input()
    value: Date;

    get formattedDate(): string {
        return this.value.toISOString();
    }
    set formattedDate(value: string) {
        if (value)
            this.value = new Date(value);
        else
            this.value = null;
    }

    @Output()
    valueChange = new EventEmitter<Date>();

    @Input()
    name: string;

    @Input()
    label: string;

    onFormattedDateChange(value: string) {
        this.formattedDate = value;
        this.valueChange.emit(this.value);
    }
}
