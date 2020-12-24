import { Course } from "./course";

export interface Basket {
    id: number;
    courses: Course[];
}

// maybe i should add another class
// for adding courses to basket
// and not using course class directly
