//https://getdatepicker.com/4/Options/
export interface DateTimePickerOption {
  format: string;
  stepping: number;
  locale: string;
  sideBySide: boolean;
  allowInputToggle: boolean;
  daysOfWeekDisabled: number[];
  useCurrent: boolean;
  calendarWeeks: boolean;
  minDate: Date | null;
  maxDate: Date | null;
  icons: {
    time: string;
    date: string;
    up: string;
    down: string;
    previous: string;
    next: string;
    today: string;
    clear: string;
    close: string;
  };
  showClear: boolean;
  keyBinds: any;
}
