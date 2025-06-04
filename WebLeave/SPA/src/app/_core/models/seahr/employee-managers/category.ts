export interface Category {
    cateID: number;
    cateName: string;
    cateSym: string;
    exhibit: boolean | null;
    visible: boolean | null;
    orderby: number | null;
}