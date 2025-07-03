export interface TOrg {
  id: number;
  level: number;
  parentID: number | null;
  dept_ID: string;
  dept_Name: string;
  class_Name: string;
  sortSeq: number | null;
  lineNum: string;
  rowCount: number | null;
}