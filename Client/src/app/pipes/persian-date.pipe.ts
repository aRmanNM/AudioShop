import * as jalaliMoment from 'jalali-moment';
import {Pipe, PipeTransform} from '@angular/core';
import {formatDate} from '@angular/common';

@Pipe({name: 'PersianDate'})
export class PersianDatePipe implements PipeTransform {
  transform(value: string, withTime: boolean = true): any {
    if (withTime) {
      return jalaliMoment(formatDate(value, 'yyyy/MM/dd - H:mm', 'en-US', '+0430'), 'YYYY/MM/DD - H:mm').format('jYYYY/jMM/jDD - H:mm');
    } else {
      return jalaliMoment(formatDate(value, 'yyyy/MM/dd', 'en-US', '+0430'), 'YYYY/MM/DD').format('jYYYY/jMM/jDD');
    }
  }
}
