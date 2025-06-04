import { ReportIndexViewModel } from './report-index-view-model.model';

export interface ExportExcelGridDto {
  leaveDate: string;
  sEAMP: string;
  applied: string;
  approved: string;
  actual: string;
  mPPoolOut: string;
  mPPoolIn: string;
  total: string;
  reportIndexViewModelDTO: ReportIndexViewModel;
}
