import { Component, OnDestroy, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ChartConfiguration, ChartData, ChartOptions, ChartType } from 'chart.js';
import ChartDataLabels from "chartjs-plugin-datalabels";
import { environment } from '@environments/environment';
import { DefectTop3 } from '@models/production/T1/C2B/defectop3';
import { FRIBADefect } from '@models/production/T1/C2B/fri-ba-defect';

import { Quality } from '@models/production/T1/C2B/quality';
import { QualityService } from '@services/production/T1/C2B/quality.service';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";


@UntilDestroy()
@Component({
  selector: 'app-qualitymain',
  templateUrl: './qualitymain.component.html',
  styleUrls: ['./qualitymain.component.scss']
})
export class QualitymainComponent extends CommonComponent implements OnInit, OnDestroy {
  quality: Quality = {} as Quality
  defecttop3: DefectTop3[] = []
  fribadefect: FRIBADefect[] = []
  previousLink: string = '';
  nextLink: string = '';
  img_path: string = environment.ip_img_path;

  public barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    // We use these empty structures as placeholders for dynamic theming.
    scales: {
      x: {}, y: {
        display: false,
        beginAtZero: true,
        ticks: {
        }
      }
    },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'start',
        color: 'white',
        font: {
          weight: 'bold',
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
  public barChartPlugins = [ChartDataLabels];

  public barChartData: ChartData<'bar'> = {
    datasets: [
      {
        data: [],
        label: 'Series A',
        borderColor: 'black',
        backgroundColor: 'rgba(0,0,255,1)'
      }
    ],
    labels: []
  };

  constructor(
    private qualityser: QualityService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/safety/safetymain/CTB/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/delivery/deliverymain/CTB/' + this.deptId;
    this.loadData();
    this.loadDefectTop3();
    this.loadDefectTop3Chart();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  loadData() {
    this.qualityser.getData(this.deptId)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.quality = data
      })
  }

  loadDefectTop3() {
    this.qualityser.GetDefectTop3(this.deptId)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.defecttop3 = data
      })
  }
  loadDefectTop3Chart() {
    this.qualityser.GetDefectTop3Chart(this.deptId)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.fribadefect = data
        this.barChartData = {
          datasets: [
            {
              data: [],
              label: '',
              borderColor: 'black',
              backgroundColor: 'rgba(0,0,255,1)'
            }
          ],
          labels: []
        };
        this.barChartData.labels = this.fribadefect.map(item => `${item.reason_ID} (${item.bA_Defect_Desc})`);
        this.barChartData.datasets[0].data = this.fribadefect.map(item => item.finding_Qty);
        this.barChartData.datasets[0].label = 'BA Defect Reasons';
      })
  }
}



