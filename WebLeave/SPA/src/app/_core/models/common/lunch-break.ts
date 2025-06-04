export interface LunchBreak {
    id: number;
    key: string;
    workTimeStart: string;
    workTimeEnd: string;
    lunchTimeStart: string;
    lunchTimeEnd: string;
    value_vi: string;
    value_en: string;
    value_zh: string;
    seq: number | null;
    visible: boolean | null;
    createdBy: number | null;
    createdTime: string | null;
    updatedBy: number | null;
    updatedTime: string | null;
    type?: string;
}

export interface WorkShift {
    key: string;
    workTimeStart: Time;
    workTimeEnd: Time;
    lunchTimeStart: Time;
    lunchTimeEnd: Time;
}

export interface Time {
    hours: number;
    minutes: number;
    seconds: number;
}