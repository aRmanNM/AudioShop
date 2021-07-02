import {Component, OnInit} from '@angular/core';
import {StatService} from '../../services/stat.service';
import {Stat} from '../../models/stat';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SpinnerService} from '../../services/spinner.service';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss']
})
export class StatsComponent implements OnInit {
  type1 = 'line';
  data1;
  options1;
  start: Date;
  end: Date;
  statsTotalGroup: Stat[];

  constructor(private statService: StatService, private snackBar: MatSnackBar, public spinnerService: SpinnerService) {
  }

  ngOnInit(): void {
    // this.getAllStatsInRange();
    this.getAllStatsTotal();
  }

  // TODO: Comment Explain this pile of shit
  //
  getAllStatsInRange(): void {
    const DayToMiliSecond = 1000 * 60 * 60 * 24;
    const startParsed = new Date(this.start);
    const endParsed = new Date(this.end);
    const difference = Math.ceil((Math.abs(endParsed.getTime() - startParsed.getTime()) / DayToMiliSecond));

    this.statService.getAllStatsInRange(startParsed.toLocaleDateString(), endParsed.toLocaleDateString()).subscribe((res) => {

      const datelabels: string[] = [];

      const ipgValues: number[] = [];
      const appFirstPage: number[] = [];
      const successfulOrdersCount: number[] = [];
      const successfulOrdersSum: number[] = [];
      const failedOrdersCount: number[] = [];
      const failedOrdersSum: number[] = [];
      const registerCount: number[] = [];
      const phoneRegisterCount: number[] = [];

      for (let i = 0; i <= difference; i++) {
        const dateForLabel = new Date(new Date().setDate(new Date().getDate() + (i - difference)));
        const dateForData = new Date(new Date().setDate(new Date().getDate() + (i - difference)));

        // console.log(dateForData);

        datelabels.push(dateForLabel.getDate().toString());

        // console.log(res.filter(s => new Date(Date.parse(s.dateOfStat.toString())).getDate()
        //   === new Date(Date.parse(dateForData.toString())).getDate()));

        ipgValues.push(res
          .filter(s => s.titleEn === 'IPG'
            && new Date(Date.parse(s.dateOfStat.toString())).getDate() === new Date(Date.parse(dateForData.toString())).getDate())
          .map(s => s.counter)[0] ?? 0);

        appFirstPage.push(res
          .filter(s => s.titleEn === 'AppFirstPage'
            && new Date(Date.parse(s.dateOfStat.toString())).getDate() === new Date(Date.parse(dateForData.toString())).getDate())
          .map(s => s.counter)[0] ?? 0);

        successfulOrdersCount.push(res
          .filter(s => s.titleEn === 'SuccessfulOrdersCount'
            && new Date(Date.parse(s.dateOfStat.toString())).getDate() === new Date(Date.parse(dateForData.toString())).getDate())
          .map(s => s.counter)[0] ?? 0);

        successfulOrdersSum.push(res
          .filter(s => s.titleEn === 'SuccessfulOrdersSum'
            && new Date(Date.parse(s.dateOfStat.toString())).getDate() === new Date(Date.parse(dateForData.toString())).getDate())
          .map(s => s.counter)[0] ?? 0);

        failedOrdersCount.push(res
          .filter(s => s.titleEn === 'FailedOrdersCount'
            && new Date(Date.parse(s.dateOfStat.toString())).getDate() === new Date(Date.parse(dateForData.toString())).getDate())
          .map(s => s.counter)[0] ?? 0);

        failedOrdersSum.push(res
          .filter(s => s.titleEn === 'FailedOrdersSum'
            && new Date(Date.parse(s.dateOfStat.toString())).getDate() === new Date(Date.parse(dateForData.toString())).getDate())
          .map(s => s.counter)[0] ?? 0);

        registerCount.push(res
          .filter(s => s.titleEn === 'RegisterCount'
            && new Date(Date.parse(s.dateOfStat.toString())).getDate() === new Date(Date.parse(dateForData.toString())).getDate())
          .map(s => s.counter)[0] ?? 0);

        phoneRegisterCount.push(res
          .filter(s => s.titleEn === 'PhoneRegisterCount'
            && new Date(Date.parse(s.dateOfStat.toString())).getDate() === new Date(Date.parse(dateForData.toString())).getDate())
          .map(s => s.counter)[0] ?? 0);
      }

      this.data1 = {
        labels: datelabels,
        datasets: [{
          label: 'آمار روزانه ورود به درگاه پرداخت',
          data: ipgValues
        }, {
          label: 'ورودی صفحه اول برنامه',
          data: appFirstPage
        }, {
          label: 'تعداد ثبت نام کاربران',
          data: registerCount
        }, {
          label: 'تعداد شماره های تایید شده',
          data: phoneRegisterCount
        }, {
          label: 'تعداد پرداخت های موفق',
          data: successfulOrdersCount
        }, {
          label: 'تعداد پرداخت های ناموفق',
          data: failedOrdersCount
        }, {
          label: 'مجموع پرداخت های موفق',
          data: successfulOrdersSum
        }, {
          label: 'مجموع پرداخت های ناموفق',
          data: failedOrdersSum
        }]
      };
    }, error => {
      this.snackBar.open('حد فاصل انتخابی معتبر نیست', null, {
        duration: 2000,
      });
    });
  }

  refresh(): void {
    this.getAllStatsInRange();
    this.getAllStatsTotal();
  }

  getAllStatsTotal(): void {
    this.statService.getAllStatsTotal().subscribe((res) => {
      this.statsTotalGroup = res;
    });
  }
}
