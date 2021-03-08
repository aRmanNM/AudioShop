import {Component, OnInit} from '@angular/core';
import {AppFileService} from '../services/app-file.service';
import {environment} from '../../environments/environment';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit {
  latest: string;
  baseUrl = environment.apiUrl;

  constructor(private appFileService: AppFileService) {
  }

  ngOnInit(): void {
    this.getLatest();
  }

  getLatest(): void {
    this.appFileService.getLatest().subscribe((res) => {
      this.latest = this.baseUrl + 'mobile/' + res.value;
    });
  }

}
