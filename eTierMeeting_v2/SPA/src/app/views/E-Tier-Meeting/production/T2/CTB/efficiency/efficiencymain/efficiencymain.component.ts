import { Component, OnDestroy, OnInit, ViewChild, effect } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ChartConfiguration, ChartData, ChartOptions, ChartType } from 'chart.js';
import { SnotifyService } from 'ng-alt-snotify';
import { NgxSpinnerService } from 'ngx-spinner';
import { ProductionT2CTBEfficiencyService } from '@services/production/T2/CTB/production-t2-ctb-efficiency.service.service';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import ChartDataLabels from "chartjs-plugin-datalabels";
import { CommonService } from "@services/common.service";
import { EfficiencyChart } from '@models/production/T2/CTB/efficiency';
import { BaseChartDirective } from 'ng2-charts';
import { LocalStorageConstants } from "@constants/storage.constants";

@UntilDestroy()
@Component({
  selector: 'app-efficiencymain',
  templateUrl: './efficiencymain.component.html',
  styleUrls: ['./efficiencymain.component.scss']
})
export class EfficiencyMainComponent extends CommonComponent implements OnInit, OnDestroy {
  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;

  previousLink: string = "";
  nextLink: string = "";
  data: EfficiencyChart = <EfficiencyChart>{};
  targetAchievement: ChartData[] = [];
  performanceAchievement: ChartData[] = [];
  iEAchievement: ChartData[] = [];
  eOLR: ChartData[] = [];
  abnormal_Working_Hours: ChartData[] = [];
  public barChartPlugins = [ChartDataLabels];
  page: number = 1;
  checkData = {
    Daily: 'Daily',
    Monthly: 'Monthly'
  }
  preOrNext: string = '';
  getOptionBarChart() {
    let config: ChartConfiguration['options'] = {
      indexAxis: 'y',
      // rotation: 90,
      responsive: true,
      scales: {
        x:
        {
          min: 0,
          beginAtZero: true,
          ticks: {
            callback: function (value: string) {
              return `${value}%`;
            },
            stepSize: 25,
            // max: 150,
          }
        },
        y: {
          // id: "y-axis-target",
          stacked: true,
          beginAtZero: false,
          ticks: {

          }
        },
        y1: {
          display: false,
          offset: true,
          stacked: true,
          // id: "y-axis-actual",
          grid: {
            offset: true
          }
        },
      },
      interaction: {
        mode: 'index',
      },
      plugins: {
        title: {
          display: true,
          font: {
            size: 20,
          },
        },
        tooltip: {
          callbacks: {
            label: (context) => {
              const v = context.raw;
              return Array.isArray(v) ? ((v[1] + v[0]) / 2).toString() : v.toString();
            }
          },
        },
        datalabels: {
          anchor: 'end',
          align: function (context) {
            var value = context.dataset.data[context.dataIndex] as number;
            return value > 0 ? 'start' : 'end';
          },
          color: function (context) {
            var value = context.dataset.data[context.dataIndex] as number;
            return value > 0 ? 'black' : 'black';
          },
          formatter: function (value, ctx) {
            return ctx.datasetIndex === 1 ? value + "%" : "";
          }, font: {
            weight: 'bold'
          },
        }
      },
    }
    return config;
  }
  public barChartOptions: ChartConfiguration['options'] = {
  };

  public barChartOptionsAbnormal_Working_Hours: ChartConfiguration['options'] = {
  };

  public barChartType: ChartType = 'bar';
  public barChartLegend = false;
  public barChartData: ChartData<'bar'> = {
    datasets: [{
      data: [],
      label: '',
      backgroundColor: []
    }], labels: []
  };
  public barChartDataAbnormal_Working_Hours: ChartData<'bar'> = {
    datasets: [
      {
        data: [],
        label: ''
      }
    ],
    labels: []
  };

  chartLabels: ChartOptions = {};

  constructor(
    private spinner: NgxSpinnerService,
    private snotify: SnotifyService,
    private commonService: CommonService,
    private route: ActivatedRoute,
    private efficiencyService: ProductionT2CTBEfficiencyService) {
    super(commonService, route);
    effect(() => {
      this.preOrNext = this.commonService.dataPreOrNext();
    });
  }

  ngOnInit() {
    this.deptId = this.route.snapshot.params["deptId"];
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + "/quality/qualitymain/CTB/" + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/efficiency/efficiencymain_2/CTB/' + this.deptId;
    this.search('Daily')
    if (this.preOrNext.includes('nextPage')) {
      setTimeout(() => {
        this.addMeetingLogPage();
      }, 1000);
    }
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  // events
  public chartClicked({ event, active }: { event: MouseEvent, active: {}[] }): void {
    // console.log(event, active);
  }

  public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
    // console.log(event, active);
  }

  getDataChart() {
    // this.efficiencyService.getData()
  }

  search(param: string) {
    this.barChartOptions = this.getOptionBarChart();
    this.barChartOptionsAbnormal_Working_Hours = this.getOptionBarChart();
    this.spinner.show();
    this.efficiencyService.getData(this.deptId, param).pipe(untilDestroyed(this))
      .subscribe((res) => {
        this.spinner.hide();
        if (res) {
          this.data = res;
          this.barChartData = {
            datasets: [
              { data: [], label: '', backgroundColor: "" },
              { data: [], label: '', backgroundColor: [] }
            ],
            labels: []
          };
          this.barChartDataAbnormal_Working_Hours =
          {
            datasets: [
              { data: [], label: '', backgroundColor: "" },
              { data: [], label: '', backgroundColor: [] }
            ],
            labels: []
          }
          //Target Archivement
          var width = res.targetAchievement ? Math.max(...res.targetAchievement.map(m => m.dataLine)) / 400 : 0;

          res.targetAchievement?.forEach(x => {
            this.barChartData.datasets[0].data.push([x.target - width, x.target + width]);
            this.barChartData.datasets[0].backgroundColor = 'blue';
            this.barChartData.datasets[1].data.push(x.dataLine);
            this.barChartData.labels.push(x.line);
            (this.barChartData.datasets[1].backgroundColor as string[]).push(x.color);

          });
          this.barChartOptions.plugins.title.text = res.targetAchievement ? `${res.targetAchievement[0].title_LL} / ${res.targetAchievement[0].title}` : '';
          // Abnormal_Working_Hours
          var width = res.abnormal_Working_Hours ? Math.max(...res.abnormal_Working_Hours?.map(m => m.dataLine)) / 400 : 0;
          res.abnormal_Working_Hours?.forEach(x => {
            width == 0 ?
              this.barChartDataAbnormal_Working_Hours.datasets[0].data.push([x.target - 0.009, x.target + 0.009]) :
              this.barChartDataAbnormal_Working_Hours.datasets[0].data.push([x.target - width, x.target + width]);
            this.barChartDataAbnormal_Working_Hours.datasets[0].backgroundColor = 'blue';
            this.barChartDataAbnormal_Working_Hours.datasets[1].data.push(x.dataLine);
            this.barChartDataAbnormal_Working_Hours.labels.push(x.line);
            (this.barChartDataAbnormal_Working_Hours.datasets[1].backgroundColor as string[]).push(x.color)
          });
          this.barChartOptionsAbnormal_Working_Hours.plugins.title.text = res.abnormal_Working_Hours ? `${res.abnormal_Working_Hours[0].title_LL}` : '';
          this.chart?.ngOnChanges({});
        }
      },
        (error) => {
          this.snotify.error(error);
          this.spinner.hide();
        }
      );
  }

}
