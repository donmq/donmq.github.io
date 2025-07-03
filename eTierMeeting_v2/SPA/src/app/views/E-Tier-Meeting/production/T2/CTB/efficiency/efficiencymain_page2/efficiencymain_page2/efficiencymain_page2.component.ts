import { Component, OnDestroy, OnInit, effect } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ChartOptions, ChartData, ChartType, ChartConfiguration } from 'chart.js';
import { SnotifyService } from 'ng-alt-snotify';
import { NgxSpinnerService } from 'ngx-spinner';
import { EfficiencyChart } from '@models/production/T2/CTB/efficiency';
import { ProductionT2CTBEfficiencyService } from '@services/production/T2/CTB/production-t2-ctb-efficiency.service.service';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import ChartDataLabels from "chartjs-plugin-datalabels";
import { LocalStorageConstants } from "@constants/storage.constants";

@UntilDestroy()
@Component({
  selector: 'app-efficiencymain_page2',
  templateUrl: './efficiencymain_page2.component.html',
  styleUrls: ['./efficiencymain_page2.component.scss']
})
export class Efficiencymain_page2Component extends CommonComponent implements OnInit, OnDestroy {
  previousLink: string = "";
  nextLink: string = "";
  data: EfficiencyChart = <EfficiencyChart>{};
  targetAchievement: ChartData[] = [];
  performanceAchievement: ChartData[] = [];
  iEAchievement: ChartData[] = [];
  eOLR: ChartData[] = [];
  abnormal_Working_Hours: ChartData[] = []
  public barChartPlugins = [ChartDataLabels];

  checkData = {
    Daily: 'Daily',
    Monthly: 'Monthly'
  }
  preOrNext: string = '';

  public barChartOptionsEOLR: ChartConfiguration['options'] = {
    indexAxis: 'y',
    responsive: true,
    interaction: {
      mode: 'index',
    },
    scales: {
      x:
      {
        beginAtZero: true,
        min: 0,
        // max: 150,
        ticks: {
          callback: function (value: string) {
            return `${value}`;
          },
          stepSize: 25,
        }
      },
      y: {
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
    plugins: {
      title: {
        display: true,
        font: {
          size: 20,
        }
      },
      tooltip: {
        callbacks: {
          label: (context) => {
            const v = context.raw
            return Array.isArray(v) ? ((v[1] + v[0]) / 2).toString() : v.toString();
          }
        },
      },
      datalabels: {
        anchor: 'end',
        align: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'start' : 'end';
        },
        color: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'black' : 'black';
        },
        formatter: function (value, ctx) {
          return ctx.datasetIndex === 1 ? value : "";
        }, font: {
          weight: 'bold'
        },
      }
    },
  };
  public barChartOptionsEOLR_STI: ChartConfiguration['options'] = {
    indexAxis: 'y',
    responsive: true,
    interaction: {
      mode: 'index',
    },
    scales: {
      x:
      {
        beginAtZero: true,
        min: 0,
        // max: 150,
        ticks: {
          callback: function (value: string) {
            return `${value}`;
          },
          stepSize: 25,
        }
      },
      y: {
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
    plugins: {
      title: {
        display: true,
        font: {
          size: 20,
        }
      },
      tooltip: {
        callbacks: {
          label: (context) => {
            const v = context.raw
            return Array.isArray(v) ? ((v[1] + v[0]) / 2).toString() : v.toString();
          }
        },
      },
      datalabels: {
        anchor: 'end',
        align: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'start' : 'end';
        },
        color: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'black' : 'black';
        },
        formatter: function (value, ctx) {
          return ctx.datasetIndex === 1 ? value : "";
        }, font: {
          weight: 'bold'
        },
      }
    },
  };
  public barChartOptionsIEArchievement: ChartConfiguration['options'] = {
    indexAxis: 'y',
    responsive: true,
    interaction: {
      mode: 'index',
    },
    scales: {
      x:
      {
        beginAtZero: true,
        min: 0,
        ticks: {
          callback: function (value: string) {
            return `${value}%`;
          },
          stepSize: 25,
          // max: 150,
        }
      },
      y: {
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
    plugins: {
      title: {
        display: true,
        font: {
          size: 20,
        }
      },
      tooltip: {
        callbacks: {
          label: (context) => {
            const v = context.raw
            return Array.isArray(v) ? ((v[1] + v[0]) / 2).toString() : v.toString();
          }
        },
      },
      datalabels: {
        anchor: 'end',
        align: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'start' : 'end';
        },
        color: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'black' : 'black';
        },
        formatter: function (value, ctx) {
          return ctx.datasetIndex === 1 ? value + "%" : "";
        }, font: {
          weight: 'bold'
        },
      }
    },
  };

  public barChartOptionsIEArchievement_STI: ChartConfiguration['options'] = {
    indexAxis: 'y',
    responsive: true,
    interaction: {
      mode: 'index',
    },
    scales: {
      x:
      {
        beginAtZero: true,
        min: 0,
        ticks: {
          callback: function (value: string) {
            return `${value}%`;
          },
          stepSize: 25,
          // max: 150,
        }
      },
      y: {
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
    plugins: {
      title: {
        display: true,
        font: {
          size: 20,
        }
      },
      tooltip: {
        callbacks: {
          label: (context) => {
            const v = context.raw
            return Array.isArray(v) ? ((v[1] + v[0]) / 2).toString() : v.toString();
          }
        },
      },
      datalabels: {
        anchor: 'end',
        align: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'start' : 'end';
        },
        color: function (context) {
          var value = context.dataset.data[context.dataIndex] as number;
          return value > 3 ? 'black' : 'black';
        },
        formatter: function (value, ctx) {
          return ctx.datasetIndex === 1 ? value + "%" : "";
        }, font: {
          weight: 'bold'
        },
      }
    },
  };

  //   indexAxis: 'y',
  //   responsive: true,
  //   interaction: {
  //     mode: 'index',
  //   },
  //   scales: {
  //     x:
  //     {
  //       beginAtZero: true,
  //       min: 0,
  //       ticks: {
  //         callback: function (value: string) {
  //           return `${value}%`;
  //         },
  //         stepSize: 25,
  //         // max: 150,
  //       }
  //     },
  //     y: {
  //       stacked: true,
  //       beginAtZero: false,

  //       ticks: {
  //       }
  //     },
  //     y1: {
  //       display: false,
  //       offset: true,
  //       stacked: true,
  //       // id: "y-axis-actual",
  //       grid: {
  //         offset: true
  //       }
  //     },
  //   },
  //   plugins: {
  //     title: {
  //       display: true,
  //       font: {
  //         size: 20,
  //       }
  //     },
  //     tooltip: {
  //       callbacks: {
  //         label: (context) => {
  //           const v = context.raw
  //           return Array.isArray(v) ? ((v[1] + v[0]) / 2).toString() : v.toString();
  //         }
  //       },
  //     },
  //     datalabels: {
  //       anchor: 'end',
  //       align: function (context) {
  //         var value = context.dataset.data[context.dataIndex] as number;
  //         return value > 3 ? 'start' : 'end';
  //       },
  //       color: function (context) {
  //         var value = context.dataset.data[context.dataIndex] as number;
  //         return value > 3 ? 'black' : 'black';
  //       },
  //       formatter: function (value, ctx) {
  //         return ctx.datasetIndex === 1 ? value + "%" : "";
  //       }, font: {
  //         weight: 'bold'
  //       },
  //     }
  //   },
  // };
  public barChartType: ChartType = 'bar';
  public barChartLegend = false;

  public barChartData: ChartData<'bar'> = {
    datasets: [
      {
        data: [],
        label: ''
      }
    ],
    labels: []
  };
  public barChartDataEOLR: ChartData<'bar'> = {
    datasets: [
      {
        data: [],
        label: ''
      }
    ],
    labels: []
  };
  public barChartDataEOLR_STI: ChartData<'bar'> = {
    datasets: [
      {
        data: [],
        label: ''
      }
    ],
    labels: []
  };
  public barChartDataIEArchievement: ChartData<'bar'> = {
    datasets: [
      {
        data: [],
        label: ''
      }
    ],
    labels: []
  };
  public barChartDataIEArchievement_STI: ChartData<'bar'> = {
    datasets: [
      {
        data: [],
        label: ''
      }
    ],
    labels: []
  };
  public barChartDataPerformanArchievement: ChartData<'bar'> = {
    datasets: [
      {
        data: [],
        label: ''
      }
    ],
    labels: []
  };

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

  async ngOnInit() {
    this.deptId = this.route.snapshot.params["deptId"];
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + "/efficiency/efficiencymain/CTB/" + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + "/kaizen/kaizenmain/CTB/" + this.deptId;
    this.search('Daily');
    if (this.preOrNext.includes('previousPage')) {
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
    this.spinner.show();
    this.efficiencyService.getData(this.deptId, param).pipe(untilDestroyed(this))
      .subscribe((res) => {
        this.spinner.hide();
        if (res) {
          this.data = res;
          //EOLR (ASY)
          this.barChartDataEOLR =
          {
            datasets: [
              { data: [], label: '', backgroundColor: "" },
              { data: [], label: '', backgroundColor: [] }
            ],
            labels: []
          }
          var width = res.eolr ? Math.max(...res.eolr.map(m => m.dataLine)) / 400 : 0;
          res.eolr?.forEach(x => {
            this.barChartDataEOLR.datasets[0].data.push([x.target - width, x.target + width]);
            this.barChartDataEOLR.datasets[0].backgroundColor = 'blue'
          });
          res.eolr?.forEach(x => this.barChartDataEOLR.datasets[1].data.push(x.dataLine));
          res.eolr?.forEach(x => this.barChartDataEOLR.labels.push(x.line));
          res.eolr?.forEach(x => (this.barChartDataEOLR.datasets[1].backgroundColor as string[]).push(x.color));
          this.barChartOptionsEOLR.plugins.title.text = res.eolr ? `${res.eolr[0].title_LL} / ${res.eolr[0].title}` : '';

          //EOLR (STI)
          this.barChartDataEOLR_STI =
          {
            datasets: [
              { data: [], label: '', backgroundColor: "" },
              { data: [], label: '', backgroundColor: [] }
            ],
            labels: []
          }
          var width = res.eolR_STI ? Math.max(...res.eolR_STI.map(m => m.dataLine)) / 400 : 0;
          res.eolR_STI?.forEach(x => {
            this.barChartDataEOLR_STI.datasets[0].data.push([x.target - width, x.target + width]);
            this.barChartDataEOLR_STI.datasets[0].backgroundColor = 'blue'
          });
          res.eolR_STI?.forEach(x => this.barChartDataEOLR_STI.datasets[1].data.push(x.dataLine));
          res.eolR_STI?.forEach(x => this.barChartDataEOLR_STI.labels.push(x.line));
          res.eolR_STI?.forEach(x => (this.barChartDataEOLR_STI.datasets[1].backgroundColor as string[]).push(x.color));
          this.barChartOptionsEOLR_STI.plugins.title.text = res.eolR_STI ? `${res.eolR_STI[0].title_LL} / ${res.eolR_STI[0].title}` : '';

          //IE Archievement (ASY)
          this.barChartDataIEArchievement =
          {
            datasets: [
              { data: [], label: '', backgroundColor: "" },
              { data: [], label: '', backgroundColor: [] }
            ],
            labels: []
          }
          var width = res.ieAchievement ? Math.max(...res.ieAchievement?.map(m => m.dataLine)) / 400 : 0;
          res.ieAchievement?.forEach(x => {
            console.log(x.target);
            this.barChartDataIEArchievement.datasets[0].data.push([x.target - width, x.target + width]);
            this.barChartDataIEArchievement.datasets[0].backgroundColor = 'blue'
          });
          res.ieAchievement?.forEach(x => this.barChartDataIEArchievement.datasets[1].data.push(x.dataLine));
          res.ieAchievement?.forEach(x => this.barChartDataIEArchievement.labels.push(x.line));
          res.ieAchievement?.forEach(x => (this.barChartDataIEArchievement.datasets[1].backgroundColor as string[]).push(x.color));
          this.barChartOptionsIEArchievement.plugins.title.text = res.ieAchievement ? `${res.ieAchievement[0].title_LL} / ${res.ieAchievement[0].title}` : '';

          //IE Archievement (STI)
          this.barChartDataIEArchievement_STI =
          {
            datasets: [
              { data: [], label: '', backgroundColor: "" },
              { data: [], label: '', backgroundColor: [] }
            ],
            labels: []
          }
          var width = res.ieAchievement_STI ? Math.max(...res.ieAchievement_STI?.map(m => m.dataLine)) / 400 : 0;
          res.ieAchievement_STI?.forEach(x => {
            this.barChartDataIEArchievement_STI.datasets[0].data.push([x.target - width, x.target + width]);
            this.barChartDataIEArchievement_STI.datasets[0].backgroundColor = 'blue'
          });
          res.ieAchievement_STI?.forEach(x => this.barChartDataIEArchievement_STI.datasets[1].data.push(x.dataLine));
          res.ieAchievement_STI?.forEach(x => this.barChartDataIEArchievement_STI.labels.push(x.line));
          res.ieAchievement_STI?.forEach(x => (this.barChartDataIEArchievement_STI.datasets[1].backgroundColor as string[]).push(x.color));
          this.barChartOptionsIEArchievement_STI.plugins.title.text = res.ieAchievement_STI ? `${res.ieAchievement_STI[0].title_LL} / ${res.ieAchievement_STI[0].title}` : '';

        }
      },
        (error) => {
          this.snotify.error(error);
          this.spinner.hide();
        }
      );
  }

}
