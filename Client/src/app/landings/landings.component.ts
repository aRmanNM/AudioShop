import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {LandingsService} from '../services/landings.service';
import {Landing} from '../models/landing';
import {environment} from '../../environments/environment';
import {LandingPhoneNumber} from '../models/landing-phone-number';

@Component({
  selector: 'app-landings',
  templateUrl: './landings.component.html',
  styleUrls: ['./landings.component.scss']
})
export class LandingsComponent implements OnInit {
  landing: Landing;
  landingId;
  phoneNumber = '';
  baseUrl = environment.apiUrl + 'Landings/';

  backgroundCSSVariable;
  backgroundColor;
  boxShadow;
  borderBottom;

  constructor(private route: ActivatedRoute,
              private landingService: LandingsService) {
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      // console.log(params.id);
      this.landingId = params.id;
      this.getLanding();
    });
  }

  getLanding(): void {
    this.landingService.getLanding(this.landingId).subscribe((res) => {
      this.landing = res;
      this.backgroundCSSVariable = `url(${this.baseUrl + this.landing.id + '/' + this.landing.background?.fileName})`;

      this.backgroundColor = this.landing.buttonsColor;
      this.boxShadow = `0px 2px 0 ${this.landing.buttonsColor}`;
      this.borderBottom = `5px solid ${this.landing.buttonsColor}`;

      console.log(res);
    });
  }

  createLandingPhoneNumber(): void {
    const landingPhoneNumber: LandingPhoneNumber = {
      id: 0,
      landingId: this.landingId,
      phoneNumber: this.phoneNumber
    };

    this.landingService.createLandingPhoneNumber(landingPhoneNumber).subscribe(() => {
      alert('شماره شما با موفقیت در سیستم ثبت شد.');
    });

  }

  updateStat(): void {
    this.landingService.updateLandingStat(this.landingId).subscribe(() => {
      alert('هم اکنون به لینک انتقال داده خواهید شد');
    });
  }

}
