import { PlnoEmploy } from "./employee";

export interface EmployAdmin {
    iD: number;
    empName: string;
    empNumber: string;
    visible: boolean | null;
    cellID: number | null;
    listPlnoEmploy: PlnoEmploy[];
}

