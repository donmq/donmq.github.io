import { Component, OnDestroy, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Chart, ChartConfiguration, ChartData, ChartOptions, ChartType } from 'chart.js';
// import * as pluginDataLabels from 'chartjs-plugin-datalabels';
// import * as pluginAnnotation from 'chartjs-plugin-annotation';
import DataLabelsPlugin from 'chartjs-plugin-datalabels';
import { BaseChartDirective } from 'ng2-charts';
import { environment } from '@environments/environment';
import { DefectTop3 } from '@models/production/T1/C2B/defectop3';
import { FRIBADefect } from '@models/production/T1/C2B/fri-ba-defect';
import { ProductionT2CTBQualityService } from '@services/production/T2/CTB/production-t2-ctb-quality.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { T2CTBQuality } from '@models/production/T2/CTB/T2CTBQuality';
import { ChartLegendItem } from '@commons/chart-legend/chart-legend.component';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import Annotation from 'chartjs-plugin-annotation';
import { LocalStorageConstants } from "@constants/storage.constants";

@UntilDestroy()
@Component({
  selector: 'app-quality-main',
  templateUrl: './quality-main.component.html',
  styleUrls: ['./quality-main.component.scss']
})
export class QualityMainComponent extends CommonComponent implements OnInit, OnDestroy {
  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;
  @ViewChild(BaseChartDirective) chartRFT: BaseChartDirective | undefined;
  @ViewChild(BaseChartDirective) chartBA: BaseChartDirective | undefined;
  tuCode: string = '';
  dataQuality: T2CTBQuality = {} as T2CTBQuality
  defecttop3: DefectTop3[] = []
  fribadefect: FRIBADefect[] = []
  previousLink: string = '';
  nextLink: string = '';
  img_path: string = environment.ip_img_path;
  switchDate: boolean = false;
  //===================Top3Chart===========================
  public barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    scales: {
      x: {},
      y: {
        display: false,
        ticks: {
          // beginAtZero: true
        }
      }
    },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'start',
        color: 'white',
        font: {
          weight: 'bold'
        },
        formatter: function (value) {
          return value == 0 ? '' : value;
        },
      }
    }
  };
  // public financialChartColors: Color[] = [
  //   {
  //     borderColor: 'black',
  //     backgroundColor: 'rgba(0,0,255,1)',
  //   },
  // ];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [DataLabelsPlugin];
  public barChartData: ChartData<'bar'> =
    {
      labels: [],
      datasets: [
        { data: [], label: '', borderColor: 'black', backgroundColor: 'rgba(0,0,255,1)' }
      ],
    }
    ;
  //======================RFT Chart==============================
  public horizontalBarChartType: ChartType = 'bar';
  public horizontalBarChartLegend = false;
  public barRFTChartOptions: ChartConfiguration['options'] = {
    indexAxis: 'y',
    responsive: true,
    scales: {
      x: {
        min: 0,
        max: 100,
        ticks: {
          callback: function (value: string) {
            return `${value}%`;
          }
        }
      },
      y: {
        grid: {
          display: false
        }
      }
    },
    plugins: {
      tooltip: {
        callbacks: {
          label: function (context) {
            return context.formattedValue + '%';
          }
        },
      },
      title: {
        display: true,
        text: 'Building RFT %',
        font: {
          size: 20
        },
        color: 'black'
      },
      datalabels: {
        anchor: 'end',
        align: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'start' : 'end';
        },
        color: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'white' : 'black';
        },
        formatter: function (value) {
          return value + "%";
        }, font: {
          weight: 'bold'
        },
      },
      annotation: {
        annotations: [
          {
            type: 'line',
            scaleID: 'x',
            borderColor: 'blue',
            borderWidth: 3,
            label: {
              display: true,
              position: 'start',
              color: 'orange',
              content: 'Target',
              font: {
                weight: 'bold',
              },
              yAdjust: -15,
            }
          },
        ],
      },
    },



  };
  barRFTChartLabels: string[] = [];
  barRFTChartData: ChartData<'bar'> = {
    datasets: [{
      data: [],
      label: ''
    }], labels: []
  };
  barRFTChartLegends: ChartLegendItem[] = [
    { color: 'green', text: 'RFT% >= 90%' },
    { color: 'yellow', text: '85% <= RFT% < 90%' },
    { color: 'red', text: 'RFT% < 85%' },
  ];

  //=========================BA Chart==========================================
  public barBAChartOptions: ChartConfiguration['options'] = {
    indexAxis: 'y',
    responsive: true,
    scales: {

      x: {
        min: 0.00,
        max: 5.00,
        ticks: {
          callback: function (value: number) {
            return value?.toFixed(2);
          },
        }
      },
      y: {
        grid: {
          display: false,
        }
      }
    },
    plugins: {
      tooltip: {
        callbacks: {
          label: function (context) {
            return parseFloat(context.formattedValue).toFixed(2);
          }
        },
      },
      title: {
        display: true,
        text: 'Building BA Score',
        font: {
          size: 20
        },
        color: 'black'
      },
      datalabels: {
        anchor: 'end',
        align: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 0.2 ? 'start' : 'end';
        },
        color: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 0.2 ? 'white' : 'black';
        },
        formatter: function (value) {
          return value?.toFixed(2);
        },
        font: {
          weight: 'bold'
        },
      },
      annotation: {
        annotations: [
          {
            type: 'line',
            scaleID: 'x',
            borderColor: 'blue',
            borderWidth: 3,
            label: {
              display: true,
              position: 'start',
              color: 'orange',
              content: 'Target',
              font: {
                weight: 'bold',
              },
              yAdjust: 0,
            }
          },
        ],
      },
    },

  };
  // public barBAChartColors: Color[] = [{}];
  public barBAChartLabels: string[] = [];
  public barBAChartData: ChartData<'bar'> = {
    datasets: [{
      data: [],
      label: ''
    }],
    labels: []
  };
  public barBAChartLegends: ChartLegendItem[] = [
    { color: 'green', text: 'BA >= 3.8 stars' },
    { color: 'yellow', text: '3.5 stars <= BA < 3.8 stars' },
    { color: 'red', text: 'BA < 3.5 stars' },
  ];

  //=================================================================
  constructor(
    private service: ProductionT2CTBQualityService,
    private spinner: NgxSpinnerService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params["deptId"];
    this.tuCode = this.deptId.trim();
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/safety/safetymain/CTB/' + this.tuCode;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/efficiency/efficiencymain/CTB/' + this.tuCode;

    this.loadDefectTop3();
    this.loadDefectTop3Chart();
    this.getData();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  getData() {
    this.spinner.show();
    if (this.tuCode) {
      this.barRFTChartOptions.plugins.title.text = this.tuCode + ' Building RFT %';
      this.barBAChartOptions.plugins.title.text = this.tuCode + ' Building BA Score';
    }

    this.service.getData(this.tuCode, this.switchDate)
      .pipe(untilDestroyed(this)).subscribe({
        next: (res) => {
          if (res) {
            this.dataQuality = res;
            this.barRFTChartData =
            {
              datasets: [{ data: [], label: '', backgroundColor: [] }],
              labels: []
            }
            this.barBAChartData =
            {
              datasets: [{ data: [], label: '', backgroundColor: [] }],
              labels: []
            }
            /* --------------------------- RFT CHART SETTINGS --------------------------- */
            this.barRFTChartData.labels = this.dataQuality.rfT_Chart.map(item => item.line_Sname);
            this.barRFTChartData.datasets[0].data = this.dataQuality.rfT_Chart.map(item => item.rft);
            this.barRFTChartData.datasets[0].datalabels = { color: this.dataQuality.rfT_Chart.map(item => item.rft >= 90 ? 'white' : item.rft >= 85 ? 'black' : 'white') };
            this.barRFTChartData.datasets[0].backgroundColor = this.dataQuality.rfT_Chart.map(item => item.rft >= 90 ? 'green' : item.rft >= 85 ? 'yellow' : 'red');

            this.barRFTChartData.labels.unshift('');
            this.barRFTChartData.datasets[0].data.unshift(null);
            (this.barRFTChartData.datasets[0].datalabels.color as string[]).unshift('');
            (this.barRFTChartData.datasets[0].backgroundColor as string[]).unshift('');

            // setting annotation chart RFT
            this.barRFTChartOptions.plugins.annotation.annotations[0].value = res.rfT_Target;
            this.barRFTChartOptions.plugins.annotation.annotations[0].label.content = 'Target: ' + res.rfT_Target + '%';
            this.barRFTChartOptions.plugins.annotation.annotations[0].label.xAdjust = res.rfT_Target <= 3 ? -32 : (res.rfT_Target >= 96 ? 35 : 0);
            this.barRFTChartOptions.plugins.annotation.annotations[0].label.yAdjust = 0;

            /* ---------------------------- BA CHART SETTINGS --------------------------- */
            this.barBAChartData.labels = this.dataQuality.bA_Chart.map(item => item.line_Sname);
            this.barBAChartData.datasets[0].data = this.dataQuality.bA_Chart.map(item => item.ba);
            this.barBAChartData.datasets[0].datalabels = { color: this.dataQuality.bA_Chart.map(item => item.ba >= 3.8 ? 'white' : item.ba >= 3.5 ? 'black' : 'white') };
            this.barBAChartData.datasets[0].backgroundColor = this.dataQuality.bA_Chart.map(item => item.ba >= 3.8 ? 'green' : item.ba >= 3.5 ? 'yellow' : 'red');

            this.barBAChartData.labels.unshift('');
            this.barBAChartData.datasets[0].data.unshift(null);
            (this.barBAChartData.datasets[0].datalabels.color as string[]).unshift('');
            (this.barBAChartData.datasets[0].backgroundColor as string[]).unshift('');

            // setting annotation chart BA
            this.barBAChartOptions.plugins.annotation.annotations[0].value = res.bA_Target;
            this.barBAChartOptions.plugins.annotation.annotations[0].label.content = 'Target: ' + res.bA_Target;
            this.barBAChartOptions.plugins.annotation.annotations[0].label.xAdjust = res.bA_Target <= 3 ? -32 : (res.bA_Target >= 96 ? 35 : 0);
            this.barBAChartOptions.plugins.annotation.annotations[0].label.yAdjust = 0;
            Chart.register(Annotation);
            //Update chart again when target is changed
            this.chart?.ngOnChanges({});

            // this.chartBA?.update();
            // this.chartRFT?.update();
          }
        },
        error: (e) => {
          console.log(e);
        }
      }).add(() => this.spinner.hide())
  }

  loadDefectTop3() {
    this.service.getDefectTop3Photos(this.tuCode, this.switchDate)
      .pipe(untilDestroyed(this)).subscribe({
        next: (res) => {
          this.defecttop3 = res;
        },
        error: (e) => {
          console.log(e);
        }
      });
  }

  loadDefectTop3Chart() {
    this.service.getDefectTop3Chart(this.tuCode, this.switchDate)
      .pipe(untilDestroyed(this)).subscribe({
        next: (res) => {
          if (res) {
            this.fribadefect = res;
            this.barChartData =
            {
              labels: [],
              datasets: [
                { data: [], label: '', borderColor: 'black', backgroundColor: 'rgba(0,0,255,1)' }
              ],
            }
            this.barChartData.datasets[0].data = this.fribadefect.map(item => item.finding_Qty);
            this.barChartData.datasets[0].label = "BA Defect Reasons";
            this.barChartData.labels = this.fribadefect.map(item => `${item.reason_ID} (${item.bA_Defect_Desc})`);
            this.chart?.update();
          }
        },
        error: (e) => {
          console.log(e);
        }
      })
  }

  changeSwitchDate(value: boolean) {
    this.switchDate = value;
    this.loadDefectTop3();
    this.loadDefectTop3Chart();
    this.getData();
  }
}
