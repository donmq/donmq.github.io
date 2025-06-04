import { LeaveDataViewModel } from "../leaveDataViewModel";
import { EmployeeData } from "./employeeData";
import { HistoryEmployee } from "./historyEmployee";

export interface PersonalDataViewModel {
    leaveDataViewModel: LeaveDataViewModel[];
    employee: EmployeeData;
    history: HistoryEmployee;
}