import { Department } from "../../common/department";
import { Position } from "./position";
import { GroupBase } from "./groupBase";
import { LeaveData } from "./LeaveData";
import { HistoryEmp } from "./HistoryEmp";
import { Category } from "./category";
import { Part } from "./part";

export interface EmployeeInfo {
    employeeHistoryInfo: HistoryEmp;
    listLeave: LeaveData[];
    allCategory: Category[];
    listPosition: Position[];
    listDept: Department[];
    groupBase: GroupBase[];
    listpart: Part[];
}