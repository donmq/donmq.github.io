export interface MonthlyCompulsoryInsuranceSummaryReport_Param
{
    factory: string
    year_Month: string
    permission_Group: string[]
    permission_Group_Name: string[]
    language: string
    department: string
    insurance_Type: string
    insurance_Type_Full: string
    kind: string
}
export interface MonthlyCompulsoryInsuranceSummaryReport
{
    department: string
    department_Name: string
    number_Of_Employees: number
    insured_Salary: number
    employer_Contribution: number
    employee_Contribution: number
    total_Amount: number
}
export interface MonthlyCompulsoryInsuranceSummaryReport_Source
{
    param: MonthlyCompulsoryInsuranceSummaryReport_Param
    data: MonthlyCompulsoryInsuranceSummaryReport
    total: number
}