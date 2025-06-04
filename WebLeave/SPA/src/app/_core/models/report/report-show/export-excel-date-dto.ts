import { ReportDateDetail } from './report-date-detail.model';

export interface ExportExcelDateDto {
  employeeNumber: string;
  no: string;
  employeeName: string;
  partCode: string;
  employeePostition: string;
  leaveType: string;
  time_Start: string;
  time_Of_Leave: string;
  status: string;
  fromDate: string;
  endDate: string;
  reportDateDetailDTO: ReportDateDetail;
}
