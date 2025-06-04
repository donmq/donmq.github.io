import { BsDatepickerConfig } from "ngx-bootstrap/datepicker";

export const bsDatepickerConfig: Partial<BsDatepickerConfig> = {
  isAnimated: true,
  adaptivePosition: true,
  containerClass: 'theme-dark-blue',
  dateInputFormat: 'DD/MM/YYYY',
  daysDisabled: [0],
  showWeekNumbers: false,
  customTodayClass: 'custom-today-class'
}
