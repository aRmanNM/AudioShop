import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { Basket } from "../Models/Basket";
import { Course } from "../Models/course";

@Injectable({
  providedIn: "root",
})
export class BasketService {
  private basketSub = new Subject<any>();
  constructor() {}

  addCourseToBasket(course: Course) {
    let basket = localStorage.getItem("basket");
    if (basket === null) {
      let basket: any = {};
      basket.courses = [course];
      localStorage.setItem("basket", JSON.stringify(basket));
      this.basketSub.next("changed");
      return;
    }

    let basketValue: Basket = JSON.parse(localStorage.getItem("basket"));
    if (basketValue.courses.find((c) => c.id === course.id)) {
      return;
    }

    basketValue.courses.push(course);
    localStorage.setItem("basket", JSON.stringify(basketValue));
    this.basketSub.next("changed");
    return;
  }

  removeCourseFromBasket(course: Course) {
    let basket: Basket = JSON.parse(localStorage.getItem("basket"));
    let index = basket.courses.findIndex((c) => c.id === course.id);
    if (index >= 0) {
      basket.courses.splice(index, 1);
      localStorage.setItem("basket", JSON.stringify(basket));
      this.basketSub.next("changed");
      return;
    } else {
      return null;
    }
  }

  getBasketCount() {
    let basket = localStorage.getItem("basket");
    if (basket === null) return 0;
    return JSON.parse(localStorage.getItem("basket")).courses.length;
  }

  watchStorage(): Observable<any> {
    return this.basketSub.asObservable();
  }

  getBasket() {
    let basket = localStorage.getItem("basket");
    if (basket === null) return null;
    return JSON.parse(localStorage.getItem("basket"));
  }

  clearBasket() {
    let basket: any = {};
    basket.courses = [];
    localStorage.setItem("basket", JSON.stringify(basket));
    this.basketSub.next("changed");
  }

  calculateBasket() {
    let basket: Basket = this.getBasket();
    let totalPrice: number = 0;
    basket?.courses.forEach((course) => {
      totalPrice += course.price;
    });

    return totalPrice;
  }
}
