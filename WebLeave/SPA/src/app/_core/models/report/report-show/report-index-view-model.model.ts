import { ReportShowModel } from './report-show-model.model';
import { GetTitleByLang } from './title-by-lang';
export interface ReportIndexViewModel {
  titleExportExcel: string;
  title: GetTitleByLang;
  lang: string;
  startDay: string | null;
  endDay: string | null;
  listReportShowModel: ReportShowModel[];
  listParent: ReportShowModel[][];
}
