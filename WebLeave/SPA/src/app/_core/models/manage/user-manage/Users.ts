import { Employee } from "./Employee";

export interface Users {
    userID: number;
    userName: string;
    hashPass: string;
    hashImage: string; 
    emailAddress: string;
    userRank: number | null;
    rolePermitted: number | null;
    roleReport: number | null;
    isPermitted: boolean | null; 
    empID: number | null;
    visible: boolean | null;
    updated: string | null;
    fullName: string;
    empNumber: string;
    employee: Employee;
}