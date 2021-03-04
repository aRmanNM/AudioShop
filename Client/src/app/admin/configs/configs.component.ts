import {Component, OnInit} from '@angular/core';
import {ConfigService} from '../../services/config.service';
import {Config} from '../../models/config';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../services/spinner.service';
import uniq from 'lodash/uniq';
import map from 'lodash/map';


interface ConfigGroup {
  groupFa: string;
  configs: Config[];
}

@Component({
  selector: 'app-configs',
  templateUrl: './configs.component.html',
  styleUrls: ['./configs.component.scss']
})
export class ConfigsComponent implements OnInit {
  configs: Config[];
  configGroups: ConfigGroup[] = [];

  constructor(private configService: ConfigService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    this.configService.configUpdateEmitter.subscribe(() => {
      this.getAllConfigs();
    });

    this.configService.onConfigUpdate();
  }

  getAllConfigs(): void {
    this.configService.getAllConfigs().subscribe((res) => {
      this.configs = res;
      const titles = uniq(map(this.configs, 'groupFa'));
      this.configGroups = [];
      for (const title of titles) {
        this.configGroups.push({
          groupFa: title,
          configs: this.configs.filter(c => c.groupFa === title)
        });
      }
    });
  }

  setConfig(config: Config): void {
    this.configService.setConfig(config).subscribe((res) => {
      this.snackBar.open('تغییرات اعمال شد', null, {
        duration: 2000,
      });
    });
  }
}
