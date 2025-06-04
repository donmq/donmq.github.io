import { LeaveDataViewModel } from "../leaveDataViewModel";
import { EmployeeData } from "../personal/employeeData";

export interface RepresentativeDataViewModel {
    leaveDataViewModel: LeaveDataViewModel;
    employee: EmployeeData;
}