export interface PlnoEmploy {
    name: string;
    plnoID: string;
    place: string;
}
export interface Employee {
    iD: number;
    empName: string;
    empNumber: string;
    visible: boolean | null;
    listPlnoEmploy: PlnoEmploy[];
}