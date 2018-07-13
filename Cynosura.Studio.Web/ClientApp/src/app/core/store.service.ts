import { Injectable } from '@angular/core';

@Injectable()
export class StoreService {
    map: { [key: string]: any; } = {};

    set(key: string, value: any) {
        this.map[key] = value;
    }

    get(key: string) {
        return this.map[key];
    }
}