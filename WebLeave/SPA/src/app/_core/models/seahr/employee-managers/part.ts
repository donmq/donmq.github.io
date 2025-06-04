export interface Part {
    partID: number;
    partName: string;
    partSym: string;
    partCode: string;
    number: number | null;
    deptID: number | null;
    visible: boolean | null;
}