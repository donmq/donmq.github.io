export interface ImageFileInfo {
    image: File;
    remark: string;
}
export interface ImageDataUpload {
    images: File[];
    remarks: string[];
    hseID: number | null;
}
export interface ImageRemark {
    name: string;
    remark: string;
}