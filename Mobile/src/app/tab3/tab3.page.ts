import { Component, OnInit } from "@angular/core";
import { Basket } from "../Models/Basket";
import { BasketService } from "../Services/basket.service";
import { ShopService } from "../Services/shop.service";
import { ToastService } from "../Services/toast.service";

@Component({
  selector: "app-tab3",
  templateUrl: "tab3.page.html",
  styleUrls: ["tab3.page.scss"],
})
export class Tab3Page implements OnInit {
  basket: Basket;
  totalPrice: number;
  constructor(
    private shopService: ShopService,
    private toast: ToastService,
    private basketService: BasketService
  ) {}

  ngOnInit() {
    this.updateBasket();
    this.basketService.watchStorage().subscribe((data: any) => {
      this.updateBasket();
    });
  }

  getBasket() {
    this.basket = this.basketService.getBasket();
  }

  getTotalPrice() {
    this.totalPrice = this.basketService.calculateBasket();
  }

  removeCourseFromBasket(course) {
    this.basketService.removeCourseFromBasket(course);
  }

  addCoursesToUser() {
    this.shopService.addCoursesToUser(this.basket.courses);
    this.basketService.clearBasket();
    this.toast.presentToast("به همین راحتی بسته هاتو خریدی");
  }

  updateBasket() {
    this.getBasket();
    this.getTotalPrice();
  }


}
