import { Component, OnInit, Input } from "@angular/core";
import * as uuid_ from "uuid/v4";
import { PropertiesService } from "./properties.service";
const uuid = uuid_;

@Component({
    selector: "app-properties",
    templateUrl: "./properties.component.html",
    styleUrls: ["./properties.component.css"]
})
export class PropertiesComponent implements OnInit {

    id = uuid();

    @Input()
    target: string;

    @Input()
    set properties(properties) {
        this.innerProperties = properties;
        this.boolProperties = {};
        Object.keys(properties)
            .filter((f) => typeof properties[f] === "boolean")
            .reduce((prev, cur) => {
                prev[cur] = properties[cur];
                return prev;
            }, this.boolProperties);
    }
    get properties() {
        return this.innerProperties;
    }

    defaults: { [k: string]: any };
    innerProperties: { [k: string]: any };

    boolProperties: { [k: string]: any };

    newProperty = "";
    constructor(
        private propertiesService: PropertiesService
    ) { }

    ngOnInit() {
        this.propertiesService.getProperties()
            .then((data) => {
                this.defaults = data;
                Object.keys(data)
                    .filter((f) => typeof data[f] === "boolean")
                    .reduce((boolProperties, cur) => {
                        if (boolProperties[cur] === undefined || boolProperties[cur] === null) {
                            boolProperties[cur] = data[cur];
                        }
                        return boolProperties;
                    }, this.boolProperties);
            });
    }

    addBool() {
        if (this.newProperty && this.newProperty.length > 0 && this.properties[this.newProperty] === undefined) {
            this.boolProperties[this.newProperty] = false;
            this.properties[this.newProperty] = false;
            this.newProperty = "";
        }
    }

    handleChange(key, value) {
        if (this.defaults[key] === value) {
            delete this.properties[key];
        } else {
            this.properties[key] = value;
        }
    }
}
