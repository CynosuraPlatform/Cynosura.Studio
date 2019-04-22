export class User {
    userName: string;
    roles: string[];
    permissions: { [permission: string]: boolean } = {};
    initPermissions() {
        const forRoles = (...r: string[]) => this.roles.some((s) => r.indexOf(s) >= 0);
    }
}
