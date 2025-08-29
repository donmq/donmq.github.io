import { Pipe, PipeTransform, Injectable } from '@angular/core';
import { DatePipe, DecimalPipe, CurrencyPipe } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
@Pipe({
  name: 'sharedPipe',
  standalone: true,
})
export class SharedPipe implements PipeTransform {
  private datePipe: DatePipe = new DatePipe('en-US');
  private decimalPipe: DecimalPipe = new DecimalPipe('en-US');
  private currencyPipe: CurrencyPipe = new CurrencyPipe('en-US');

  constructor() { }

  transform(value: any, pipeString: string): any {
    if (!pipeString) {
      return value; // Return the value directly if there's no pipe info
    }

    const [pipeName, ...args] = pipeString.split(':');

    switch (pipeName) {
      case 'date':
        return this.datePipe.transform(value, ...args);
      case 'decimal':
        return this.decimalPipe.transform(value, ...args);
      case 'currency':
        return this.currencyPipe.transform(value, ...args);
      default:
        return value; // Return the value unaltered if the pipe is unrecognized
    }
  }
}
