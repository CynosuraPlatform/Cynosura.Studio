import { Component, Input, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
    selector: "app-number-edit",
    templateUrl: "./number.edit.component.html",
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => NumberEditComponent),
            multi: true
        }
    ]
})
export class NumberEditComponent implements ControlValueAccessor {

    @Input()
    value: string;

    @Input()
    name: string;

    @Input()
    label: string;

    @Input()
    readonly = false;

    get innerValue() {
        return this.value;
    }

    set innerValue(val) {
        this.value = val;
        this.onChange(val);
        this.onTouched();
    }

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
