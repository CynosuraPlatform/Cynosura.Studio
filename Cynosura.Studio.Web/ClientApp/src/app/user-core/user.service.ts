import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { User } from "./user.model";
import { Page } from "../core/page.model";

@Injectable()
export class UserService {
    private userUrl = "/api/user";
    private headers = new HttpHeaders({ "Content-Type": "application/json" });

    constructor(private httpClient: HttpClient) { }

    getUsers(pageIndex?: number, pageSize?: number): Promise<Page<User>> {
        const url = this.userUrl;

        let params = new HttpParams();

        if (pageIndex !== undefined && pageIndex !== null) {
            params = params.set("pageIndex", pageIndex.toString());
        }

        if (pageSize !== undefined && pageSize !== null) {
            params = params.set("pageSize", pageSize.toString());
        }

        return this.httpClient.get<Page<User>>(url, {
            params: params
        }).toPromise();
    }

    getUser(id: number): Promise<User> {
        const url = `${this.userUrl}/${id}`;
        return this.httpClient.get<User>(url)
          .toPromise();
    }

    updateUser(user: User): Promise<User> {
        const url = `${this.userUrl}/${user.id}`;
        if (!user.password) {
            user.password = null;
        }

        return this.httpClient.put<User>(url, JSON.stringify(user), { headers: this.headers })
            .toPromise();
    }

    createUser(user: User): Promise<User> {
        return this.httpClient.post<User>(this.userUrl, JSON.stringify(user), { headers: this.headers })
            .toPromise();
    }

    deleteUser(id: number): Promise<{}> {
        const url = `${this.userUrl}/${id}`;
        return this.httpClient.delete(url)
            .toPromise();
    }
}
