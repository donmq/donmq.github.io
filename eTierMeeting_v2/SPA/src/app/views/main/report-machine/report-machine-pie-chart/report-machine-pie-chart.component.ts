import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import * as Highcharts from 'highcharts';
import { ReportMachine } from '../../../../_core/_models/report-machine';
@Component({
  selector: 'app-pie-chart',
  templateUrl: './report-machine-pie-chart.component.html',
  styleUrls: ['./report-machine-pie-chart.component.scss']
})
export class ReportMachinePieChartComponent implements OnInit, OnChanges {
  machineUsed: number;
  machineNotUsed: number;
  public data: any = [];
  @Input() dataChart: ReportMachine={
    totalIdle: 0,
    totalInuse: 0,
    listReportMachineItem: [],
  };

  highcharts = Highcharts;

  constructor(
    private translate: TranslateService
  ) { }

  chartOptions: any = {
    chart: {
      type: 'pie',
    },
    title: {
      text: this.translate.instant('reportmachine.chart')
    },
    plotOptions: {
      column: {
        stacking: 'normal',
        dataLabels: {
          enabled: true
        }
      }
    },
    credits: {
      enabled: false
    },
    series: [
      {
        name: 'Biểu đồ chart',
        data: [
          {
            name: this.translate.instant('reportmachine.idle'),
            y: 0,
            sliced: true,
            selected: true,
            color: '#ab6908',
          },
          {
            name: this.translate.instant('reportmachine.inuse'),
            sliced: true,
            selected: true,
            y: 0,
            color: '#197ec1',
          }
        ]
      }
    ]
  };
  ngOnInit(): void {

  }

  ngOnChanges(changes: SimpleChanges): void {
    this.machineNotUsed = this.dataChart.totalIdle;
    this.machineUsed = this.dataChart.totalInuse;

    this.chartOptions = {
      chart: {
        type: 'pie',
      },
      title: {
        text: this.translate.instant('reportmachine.chart')
      },
      plotOptions: {
        column: {
          stacking: 'normal',
          dataLabels: {
            enabled: true
          }
        }
      },
      credits: {
        enabled: false
      },
      series: [
        {
          name: 'Biểu đồ chart',
          data: [
            {
              name: this.translate.instant('reportmachine.idle') + ':' + this.machineNotUsed,
              y: this.machineNotUsed,
              sliced: true,
              selected: true,
              color: '#ab6908',
            },
            {
              name: this.translate.instant('reportmachine.inuse') + ':' + this.machineUsed,
              sliced: true,
              selected: true,
              y: this.machineUsed,
              color: '#197ec1',
            }
          ]
        }
      ]
    };
  }
}
