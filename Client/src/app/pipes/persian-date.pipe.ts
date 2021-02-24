import * as jalaliMoment from 'jalali-moment';
import {Pipe, PipeTransform} from '@angular/core';

@Pipe({name: 'PersianDate'})
export class PersianDatePipe implements PipeTransform {
  transform(value: string, exponent?: number): any {
    return jalaliMoment(value, 'YYYY/MM/DD').format('jYYYY/jMM/jDD');
  }
}
