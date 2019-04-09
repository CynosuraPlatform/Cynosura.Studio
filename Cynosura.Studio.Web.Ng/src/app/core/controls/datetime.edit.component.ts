import { Component, Input, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
    selector: "app-datetime-edit",
    templateUrl: "./datetime.edit.component.html",
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DateTimeEditComponent),
            multi: true
        }
    ]
})
export class DateTimeEditComponent implements ControlValueAccessor {

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
            this.value = new Date(val);
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
