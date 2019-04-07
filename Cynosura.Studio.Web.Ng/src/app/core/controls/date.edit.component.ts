import { Component, Input, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
    selector: "app-date-edit",
    templateUrl: "./date.edit.component.html",
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DateEditComponent),
            multi: true
        }
    ]
})
export class DateEditComponent implements ControlValueAccessor {

    @Input()
    value: Date;

    get innerValue(): string {
        if (this.value) {
            return this.value.toISOString();
        } else {
            return null;
        }
    }

    set innerValue(val: string) {
        if (val) {
            this.value = this.removeTimezone(new Date(val));
        } else {
            this.value = null;
        }
        this.onChange(this.value);
        this.onTouched();
    }

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    readonly = false;

    onChange: any = () => { };
    onTouched: any = () => { };

    removeTimezone(date: Date): Date {
        return new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000);
    }

    registerOnChange(fn) {
        this.onChange = fn;
    }

    registerOnTouched(fn) {
        this.onTouched = fn;
    }

    writeValue(value) {
        this.innerValue = value;
    }
}
