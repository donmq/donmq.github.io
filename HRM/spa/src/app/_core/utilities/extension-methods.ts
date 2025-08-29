
declare global {
  interface Date {
    toDate(): Date;
    toUTCDate(): Date;
    toStringDate(): string;
    toStringTime(): string;
    toStringDateTime(): string;
    toStringYearMonth(): string;
    toFirstDateOfMonth(): Date;
    toLastDateOfMonth(): Date;
    toFirstDateOfYear(): Date;
    toLastDateOfYear(): Date;
    toBeginDate(): Date;
    toEndDate(): Date;
    addDays(days: number): Date;
    addMonths(months: number): Date;
    addYears(years: number): Date;
    removeTime(): Date;
    toLastMonday(): Date;
    toLastSaturday(): Date;
    isValidDate(): boolean
  }

  interface String {
    toDate(): Date;
    toUTCDate(): Date;
    toUrl(): String;
    isNullOrWhiteSpace(): boolean;
  }

  interface Number {
    toStringLeadingZeros(targetLength: number): string;
  }

  /**
   * Array
   * @template T
   * @param => string | Array<string> ex: 'id' | '['id', 'name', 'age']'
   * @returns this T[]
   */
  interface Array<T> {
    findDuplicateItems(filterConditions: string | Array<string>): T[];
  }
}

Date.prototype.toDate = function (): Date {
  const _this = this as string;
  return new Date(_this);
}

Date.prototype.toUTCDate = function (): Date {
  const _this = this as Date;
  return new Date(Date.UTC(
    _this.getFullYear(),
    _this.getMonth(),
    _this.getDate(),
    _this.getHours(),
    _this.getMinutes(),
    _this.getSeconds(),
    _this.getMilliseconds()));
}

Date.prototype.toStringDate = function (): string {
  const _this = this as Date;
  const year = _this.getFullYear();
  const month = (_this.getMonth() + 1).toStringLeadingZeros(2);
  const date = _this.getDate().toStringLeadingZeros(2);
  return `${year}/${month}/${date}`;
}

Date.prototype.toStringTime = function (): string {
  const _this = this as Date;
  const hours = _this.getHours().toStringLeadingZeros(2);
  const minutes = _this.getMinutes().toStringLeadingZeros(2);
  const seconds = _this.getSeconds().toStringLeadingZeros(2);
  return `${hours}:${minutes}:${seconds}`;
}

Date.prototype.toStringDateTime = function (): string {
  const _this = this as Date;
  const year = _this.getFullYear();
  const month = (_this.getMonth() + 1).toStringLeadingZeros(2);
  const date = _this.getDate().toStringLeadingZeros(2);
  const hours = _this.getHours().toStringLeadingZeros(2);
  const minutes = _this.getMinutes().toStringLeadingZeros(2);
  const seconds = _this.getSeconds().toStringLeadingZeros(2);
  return `${year}/${month}/${date} ${hours}:${minutes}:${seconds}`;
}

Date.prototype.toStringYearMonth = function (): string {
  const _this = this as Date;
  const year = _this.getFullYear();
  const month = (_this.getMonth() + 1).toStringLeadingZeros(2);
  return `${year}/${month}`;
}

Date.prototype.toFirstDateOfMonth = function (): Date {
  const _this = this as Date;
  return new Date(_this.getFullYear(), _this.getMonth(), 1);
}

Date.prototype.toLastDateOfMonth = function (): Date {
  const _this = this as Date;
  return new Date(_this.getFullYear(), _this.getMonth() + 1, 0);
}

Date.prototype.toFirstDateOfYear = function (): Date {
  const _this = this as Date;
  return new Date(_this.getFullYear(), 0, 1);
}

Date.prototype.toLastDateOfYear = function (): Date {
  const _this = this as Date;
  return new Date(_this.getFullYear(), 11, 31);
}

Date.prototype.toBeginDate = function (): Date {
  const _this = this as Date;
  _this.setHours(0, 0, 0);
  return _this;
}

Date.prototype.toEndDate = function (): Date {
  const _this = this as Date;
  _this.setHours(23, 59, 59);
  return _this;
}

Date.prototype.addDays = function (days: number) {
  const _this = this as Date;
  _this.setDate(_this.getDate() + days);
  return _this;
}

Date.prototype.addMonths = function (months: number) {
  const _this = this as Date;
  _this.setMonth(_this.getMonth() + 1 + months);
  return _this;
}

Date.prototype.addYears = function (years: number) {
  const _this = this as Date;
  _this.setFullYear(_this.getFullYear() + years);
  return _this;
}

Date.prototype.removeTime = function () {
  const _this = this as Date;
  _this.setHours(0, 0, 0, 0);
  return _this;
}

Date.prototype.toLastMonday = function () {
  const _this = this as Date;
  const subday = _this.getDay() + (_this.getDay() == 0 ? 13 : 6);
  return new Date().addDays(-subday);
};

Date.prototype.toLastSaturday = function () {
  const _this = this as Date;
  const subday = _this.getDay() + (_this.getDay() == 0 ? 8 : 1);
  return new Date().addDays(-subday);
};

Date.prototype.isValidDate = function () {
  return this instanceof Date && !isNaN(this.getTime());
}

String.prototype.toDate = function (): Date {
  const _this = this as string;
  return new Date(_this);
}

String.prototype.toUTCDate = function (): Date {
  const _this = this as string;
  return _this.toDate().toUTCDate();
}

String.prototype.toUrl = function (): String {
  const _this = this as string;
  return _this.toLowerCase().replace(' - ', '-').replace('/', '-').split(' ').join("-");
}

String.prototype.isNullOrWhiteSpace = function (): boolean {
  const _this = this as string;
  return !_this || /^\s*$/.test(_this);
}

Number.prototype.toStringLeadingZeros = function (targetLength: number): string {
  const _this = this as number;
  return String(_this).padStart(targetLength, '0');
}

Array.prototype.findDuplicateItems = function <T>(filterConditions: string | Array<string>): T[] {
  const _this = this as T[];

  let foundElements: T[] = [];
  if (typeof (filterConditions) === 'string') {
    _this.forEach((item: T) => {
      if (_this.filter(x => {
        if (item[filterConditions] instanceof Date)
          return x[filterConditions].toStringDate() == item[filterConditions].toStringDate()
        else
          return x[filterConditions] === item[filterConditions]

      }).length > 1)
        foundElements.push(item);
    });
  }
  else {
    _this.forEach((item: T) => {
      if (_this.filter(x => filterConditions.every(key => {
        if (item[key] instanceof Date)
          return x[key].toStringDate() == item[key].toStringDate()
        else
          return x[key] === item[key]

      })).length > 1)
        foundElements.push(item);
    });
  }

  return Array.from(new Set(foundElements));
}

export { };
