export interface UploadT1Model {
    video_Kind: string;
    unitId: string;
    unitName: string;
    video_Title_ENG: string;
    video_Title_CHT: string;
    vIdeo_Title_LCL: string;
    video: string;
    video_Icon: string;
    video_Remark: string;
    urllIcon: any;
    from_Date: string;
    to_Date: string;
}
export interface UploadVideoParam {
    video_Kind: string;
    video_Title_ENG: string;
    video_Title_CHT: string;
    vIdeo_Title_LCL: string;
    video: string;
    video_Icon: string;
    video_Remark: string;
    from_Date: string;
    to_Date: string;
    unitIds: string[];
}