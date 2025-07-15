export interface InventoryLine {
  plnoName: string;
  plnoId: string;
  place: string;
  timeSoKiem: string | null;
  timePhucKiem: string | null;
  timeRutKiem: string | null;
  pecenMatchPhucKiem: string;
  pecenMatchSoKiem: string;
  pecenMatchRutKiem: string;
  isTimeSoKiemValid: boolean;
  isTimePhucKiemValid: boolean;
  isTimeRutKiemValid: boolean;
}
