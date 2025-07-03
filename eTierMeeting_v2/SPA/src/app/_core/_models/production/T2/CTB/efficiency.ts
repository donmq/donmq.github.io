export interface EfficiencyChart {
    dataDate: string;
    firstDayOfMonth: string;
    targetAchievement: ChartData[];
    performanceAchievement: ChartData[];
    ieAchievement: ChartData[];
    ieAchievement_STI: ChartData[];
    eolr: ChartData[];
    eolR_STI: ChartData[];
    abnormal_Working_Hours:ChartData[];
}

export interface ChartData {
    title: string;
    title_LL: string;
    line: string;
    dataLine: number;
    target: number;
    color: string;
}

export interface EfficiencyDTO {
    dept_ID: string;
    data_Date: string;
    current_Date: string;
    line_Sname: string;
    targetAchievement: number;
    performanceAchievement: number;
    iEAchievement: number;
    eOLR: number;
    targetAchievement_Target: number;
    performanceAchievement_Target: number;
    iEAchievement_Target: number;
    eOLR_Target: number;
}
