import { Component, OnInit } from '@angular/core';
import { TreeNode } from 'primeng/api';
import { interval, take, takeUntil } from 'rxjs';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { ReportChartDto } from '@models/report/report-chart-dto';
import { ReportChartService } from '@services/report/report-chart.service';
import { ReportShowService } from '@services/report/report-show/reportShow.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { DestroyService } from '@services/destroy.service';
import { InjectBase } from '@utilities/inject-base-app';

enum StyleChart {
  COMPANY = 'company-chart',
  AREA = 'area-chart',
  DEPARTMENT = 'department-chart',
  PART = 'part-chart',
  ID_CHART2 = 'card-chart2',
}
@Component({
  selector: 'app-report-main',
  templateUrl: './report-main.component.html',
  styleUrls: ['./report-main.component.scss'],
  providers: [DestroyService]
})
export class ReportMainComponent extends InjectBase implements OnInit {
  dataChart1: TreeNode[] = [];
  dataChart1Const: ReportChartDto;
  dataChart2: TreeNode[] = [];
  dataChart2Const: ReportChartDto;
  data1: TreeNode[];
  data2: TreeNode[];
  selectedNode: TreeNode;
  langIds: string[] = [LangConstants.EN, LangConstants.VN, LangConstants.ZH_TW];
  constructor(
    private reportChartService: ReportChartService,
    private reportShowService: ReportShowService,
  ) {
    super();
  }

  ngOnInit() {
    this.getData();
    this.changeLang();
  }
  getData() {
    this.spinnerService.show();
    let indexLang = this.getIndexCurrentLangSystem();
    this.reportChartService.getData().subscribe({
      next: (res) => {
        this.dataChart1Const = JSON.parse(JSON.stringify(res));
        this.dataChart1 = this.getDataChart1(indexLang);
      },
      error: (err) => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide();
      },
      complete: () => {
        this.spinnerService.hide();
      },
    });
  }

  selectChart1(event) {
    if (event.node.styleClass === StyleChart.AREA) {
      let areaId = event.node.data.id;
      this.spinnerService.show();
      let indexLang = this.getIndexCurrentLangSystem();
      this.reportChartService.getDataChartInArea(areaId).subscribe({
        next: (res) => {
          this.dataChart2Const = JSON.parse(JSON.stringify(res));
          this.dataChart2 = this.getDataChart2(indexLang);
          let time = interval(2);
          time.pipe(take(1)).subscribe((res) => {
            const el = document.getElementById(StyleChart.ID_CHART2);
            el.scrollLeft = el.scrollWidth / 2 - el.offsetWidth / 2 + 80;
          });

          let time1 = interval(500);
          time1.pipe(take(1)).subscribe((res1) => {
            let elements = document.getElementsByClassName('ng-star-inserted');
            for (let i = 0; i < elements.length; i++) {
              const element = elements[i] as HTMLElement;
              if (element.innerHTML === 'QC') {
                element.style.width = '150px';
                break;
              }
            }
          });

          this.spinnerService.hide();
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        },
      });
    } else {
      // id company router tới VIEW REPORT
      this.reportShowService.updatedParam(<KeyValuePair>{
        key: event.node.data.id,
        value: 'company',
      });
      this.router.navigate(['/report/report-show']);
    }
  }

  selectChart2(event) {
    let itemChart = event.node;
    let id = itemChart.data.id;
    // Truyền id tới VIEW REPORT
    if (itemChart.styleClass === StyleChart.AREA) {
      this.reportShowService.updatedParam(<KeyValuePair>{
        key: id,
        value: 'area',
      });
      this.router.navigate(['/report/report-show']);
    } else if (itemChart.styleClass === StyleChart.DEPARTMENT) {
      this.reportShowService.updatedParam(<KeyValuePair>{
        key: id,
        value: 'department',
      });
      this.router.navigate(['/report/report-show']);
    } else if (itemChart.styleClass === StyleChart.PART) {
      this.reportShowService.updatedParam(<KeyValuePair>{
        key: id,
        value: 'part',
      });
      this.router.navigate(['/report/report-show']);
    } else {
    }
  }
  getIndexCurrentLangSystem() {
    let langSysmtemCurrent: string = localStorage.getItem(
      LocalStorageConstants.LANG
    );
    let indexLang = this.langIds.findIndex((x) =>
      x.includes(langSysmtemCurrent)
    );
    return indexLang;
  }
  getDataChart1(indexLang: number) {
    let data = [
      {
        label: this.dataChart1Const.names[0].name,
        styleClass: StyleChart.COMPANY,
        expanded: true,
        data: {
          name: this.dataChart1Const.names[0].name,
          id: this.dataChart1Const.id,
        },
        children: this.dataChart1Const.children.map((itemArea) => {
          return {
            label: itemArea.names[indexLang].name,
            styleClass: StyleChart.AREA,
            expanded: true,
            data: { name: itemArea.names[indexLang].name, id: itemArea.id },
          };
        }),
      },
    ];
    return data;
  }
  getDataChart2(indexLang: number) {
    let data = [
      {
        label: this.dataChart2Const.names[indexLang].name,
        styleClass: StyleChart.AREA,
        expanded: true,
        data: {
          name: this.dataChart2Const.names[indexLang].name,
          id: this.dataChart2Const.id,
        },
        children: this.dataChart2Const.children.map((item1) => {
          return {
            label: item1.names[indexLang].name,
            styleClass: StyleChart.DEPARTMENT,
            expanded: true,
            data: { name: item1.names[indexLang].name, id: item1.id },
            children: item1.children.map((item2) => {
              return {
                label: item2.names[indexLang].name,
                styleClass: StyleChart.PART,
                expanded: true,
                data: { name: item2.names[indexLang].name, id: item2.id },
              };
            }),
          };
        }),
      },
    ];
    return data;
  }
  changeLang() {
    this.translateService.onLangChange
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe((event) => {
        let indexLang = this.langIds.findIndex((x) => x.includes(event.lang));
        this.dataChart1.length = 0;
        this.dataChart2.length = 0;
        this.dataChart1Const &&
          (this.dataChart1 = this.getDataChart1(indexLang));
        this.dataChart2Const &&
          (this.dataChart2 = this.getDataChart2(indexLang));
      });
  }
}
