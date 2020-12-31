import {Course} from './course';

export interface Basket {
    userId?: string;
    courseDtos: Course[];
    totalPrice: number;
}
