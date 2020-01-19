import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { ConfigService } from "../config/config.service";
import { CreatedEntity } from "../core/models/created-entity.model";
import { User } from "./user.model";
import { GetUsers, GetUser, UpdateUser, CreateUser, DeleteUser } from "./user-request.model";
import { UserFilter } from "./user-filter.model";
import { Page } from "../core/page.model";

@Injectable()
export class UserService {
    private apiUrl = this.configService.config.apiBaseUrl + "/api";

    constructor(private httpClient: HttpClient, private configService: ConfigService) { }

    getUsers(getUsers: GetUsers): Promise<Page<User>> {
        const url = `${this.apiUrl}/GetUsers`;
        return this.httpClient.post<Page<User>>(url, getUsers)
            .toPromise();
    }

    getUser(getUser: GetUser): Promise<User> {
        const url = `${this.apiUrl}/GetUser`;
        return this.httpClient.post<User>(url, getUser)
            .toPromise();
    }

    updateUser(updateUser: UpdateUser): Promise<{}> {
        const url = `${this.apiUrl}/UpdateUser`;
        if (!updateUser.password) {
            updateUser.password = null;
        }
        return this.httpClient.post(url, updateUser)
            .toPromise();
    }

    createUser(createUser: CreateUser): Promise<CreatedEntity<number>> {
        const url = `${this.apiUrl}/CreateUser`;
        return this.httpClient.post<CreatedEntity<number>>(url, createUser)
            .toPromise();
    }

    deleteUser(deleteUser: DeleteUser): Promise<{}> {
        const url = `${this.apiUrl}/DeleteUser`;
        return this.httpClient.post(url, deleteUser)
            .toPromise();
    }
}
