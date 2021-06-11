import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {AppFileService} from '../services/app-file.service';
import {environment} from '../../environments/environment';
import {MediaMatcher} from '@angular/cdk/layout';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit {
  latest: string;
  baseUrl = environment.apiUrl;
  opened = false;
  mobileQuery: MediaQueryList;

  private _mobileQueryListener: () => void;

  constructor(private appFileService: AppFileService,
              private changeDetectorRef: ChangeDetectorRef,
              private media: MediaMatcher) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addEventListener('change', this._mobileQueryListener);
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
