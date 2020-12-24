import { Course } from "./course";

export interface User {
    email: string;
    password: string;
    courses: Course[];
    token: string
}