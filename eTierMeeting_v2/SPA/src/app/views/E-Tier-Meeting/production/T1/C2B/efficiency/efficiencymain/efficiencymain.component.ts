import { Component, OnDestroy, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ChartConfiguration, ChartData, ChartOptions, ChartType } from "chart.js";
import DatalabelsPlugin from 'chartjs-plugin-datalabels';

import { eTM_MES_PT1_Summary } from "@models/eTM_MES_PT1_Summary";
import { Efficiency } from "@models/production/efficiency";
import { EfficiencyService } from "@services/production/T1/C2B/efficiency.service";
import { CommonComponent } from "@commons/common/common";
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";


@UntilDestroy()
@Component({
  selector: "app-efficiencymain",
  templateUrl: "./efficiencymain.component.html",
  styleUrls: ["./efficiencymain.component.scss"],
})
export class EfficiencymainComponent extends CommonComponent implements OnInit, OnDestroy {
  data: Efficiency;
  previousLink: string = "";
  nextLink: string = "";
  isChartDataNotNull: boolean = false;
  dataChart: eTM_MES_PT1_Summary[] = [];
  // Pie chart
  public pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'bottom',
        labels: {
          font: {
            size: 20,
            // style: 'italic',
          } //font size of the data title
        }
      },
    }
  };
  // public pieChartData: ChartDataSets[] = [
  //   { data: [0], label: '4M Information' }
  // ];
  pieChartData: ChartData<'pie'> = {
    datasets: [{
      data: [],
      label: ''

    }], labels: []
  };
  public pieChartType: ChartType = 'pie';
  public pieChartLegend = true;
  public pieChartPlugins = [DatalabelsPlugin];

  constructor(
    private efficiencyService: EfficiencyService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params["deptId"];
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + "/delivery/deliverymain/CTB/" + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + "/kaizen/kaizenmain/CTB/" + this.deptId;
    this.loadData();
    this.loadDataChart();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  loadData() {
    this.efficiencyService
      .getData(this.deptId)
      .pipe(untilDestroyed(this))
      .subscribe((res) => {
        this.data = res;
      });
  }

  loadDataChart() {
    this.efficiencyService.getDataChart(this.deptId).pipe(untilDestroyed(this)).subscribe((res) => {
      this.dataChart = res;
      if (this.dataChart.length > 0) {
        this.isChartDataNotNull = true;
        this.pieChartData.datasets[0].data = this.dataChart.map((item) => item.impact_Qty);
        this.pieChartData.datasets[0].datalabels = {
          formatter: (value) => {
            return `${value} (${this.dataChart.find(x => x.impact_Qty === value).percentage}%) `; //show percentage with value
          },
          labels: {
            title: {
              font: {
                size: 40 //font size of the data percentage
              }
            }
          }
        }
        this.pieChartData.labels = this.dataChart.map((item) => {
          return `[${item.in_Ex}.${item.reason_Code}] ${item.desc_Local}`
        });
      }
      else {
        this.isChartDataNotNull = false;
      }
    })
  }
}
