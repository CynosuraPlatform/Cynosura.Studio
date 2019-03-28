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

    onChange: any = () => { };
    onTouched: any = () => { };

    @Input("value")
    val: Date;

    get value(): string {
        if (this.val) {
            return this.val.toISOString();
        } else {
            return null;
        }
    }

    set value(val: string) {
        if (val) {
            this.val = this.removeTimezone(new Date(val));
        } else {
            this.val = null;
        }
        this.onChange(this.val);
        this.onTouched();
    }

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    readonly = false;

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
        this.value = value;
    }
}
