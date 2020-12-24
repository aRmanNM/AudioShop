import { Component, OnInit } from "@angular/core";
import { BasketService } from "../Services/basket.service";

@Component({
  selector: "app-tabs",
  templateUrl: "tabs.page.html",
  styleUrls: ["tabs.page.scss"],
})
export class TabsPage implements OnInit {
  courseCount: number;
  constructor(private basketService: BasketService) {}

  ngOnInit() {
    this.courseCount = this.basketService.getBasketCount();
    this.basketService.watchStorage().subscribe((data: any) => {
      this.courseCount = this.basketService.getBasketCount();
    });
  }
}
