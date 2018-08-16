export class User {
    id: number;
    userName: string;
    email: string;
    password: string | null;
    confirmPassword: string;
    roleIds: number[];
}
