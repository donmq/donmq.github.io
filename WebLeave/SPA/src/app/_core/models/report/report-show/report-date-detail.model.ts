import { ReportShowModel } from './report-show-model.model';
import { GetTitleByLang } from './title-by-lang';

export interface ReportDateDetail {
  // title: string;
  title: GetTitleByLang;
  lang: string;
  leaveDate: string;
  listReportShowDateDetail: ReportShowModel[];
}
