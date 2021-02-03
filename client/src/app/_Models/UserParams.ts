import { User } from "./User";

export class UserParams {
    gender: string;
    minAge: number = 18;
    maxAge: number = 90;
    pageNumber: number = 1;
    pageSize: number = 5;
    orderBy = "lastActive";

    constructor(user: User) {
        this.gender = user.Gender == "female" ? "male" : "female";
    }
}