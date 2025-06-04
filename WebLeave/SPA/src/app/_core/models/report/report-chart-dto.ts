export interface ReportChartDto {
    names: NameLang[];
    id: number;
    children: ReportChartDto[];
}

export interface NameLang {
    name: string;
    lang: string;
}