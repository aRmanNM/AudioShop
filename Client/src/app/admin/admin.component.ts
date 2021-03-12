import {Component, OnDestroy, OnInit} from '@angular/core';
import {CoursesAndEpisodesService} from '../services/courses-and-episodes.service';
import {AuthService} from '../services/auth.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit, OnDestroy {
  opened = true;

  constructor(private authService: AuthService) {
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.authService.logout();
  }
}
