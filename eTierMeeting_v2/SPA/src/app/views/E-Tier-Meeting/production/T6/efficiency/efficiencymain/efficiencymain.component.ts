import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { take, takeUntil } from 'rxjs/operators';
import { EfficiencyKanbanParam } from '@models/production/T6/efficiency';
import { DestroyService } from '@services/destroy.service';
import { ProductionT6EfficiencyService } from '@services/production/T6/efficiency/production-t6-efficiency.service';
import { ChangeRouterService } from '@services/hubs/change-router.service';
import { Router } from '@angular/router';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-efficiencymain',
  templateUrl: './efficiencymain.component.html',
  styleUrls: ['./efficiencymain.component.scss'],
  providers: [DestroyService]
})
export class EfficiencymainComponent implements OnInit {
  checkAll: boolean = true;
  autoChangePage: boolean = true;
  myInterval = 0;
  subscription: Subscription;
  isShowChart: boolean = false;
  colors: string[] = ['#3399ff', '#4d9619', '#e55353', '#a320cb', '#c2cb20'];
  // factories: string[] = ['all'];
  efficiencyParam: EfficiencyKanbanParam = {
    type: 'week',
    is_T5_External: false,
    factorys: [],
    brands: []
  };
  itemsPerSlide: number;
  dataAll: any[] = [{
    chart: {
      caption: null,
      showhovereffect: "1",
      numbersuffix: "%",
      drawcrossline: "1",
      showLegend: "0",
      plottooltext: null,
      theme: "candy"
    },
    categories: [
      {
        category: []
      }
    ],
    dataset: []
  }];
  chartConfig: Object = {
    width: '100%',
    height: '420',
    type: 'msline',
    dataFormat: 'json',
  };

  constructor(
    private destroyService: DestroyService,
    private spinner: NgxSpinnerService,
    private service: ProductionT6EfficiencyService,
    private router: Router,
    private changeRouterService: ChangeRouterService,
    private cdr: ChangeDetectorRef) {
  }

  ngOnInit() {
    this.getListFactory();
    this.getListBrand();
    this.refresh();
    if (this.autoChangePage)
      this.myInterval = 60000; // 1 phut

    this.changeRouterService.getMessageObservable().subscribe({
      next: (router) => {
        if (router) {
          this.router.navigate(["Production/T5/" + router]).then(() => {
            window.location.reload();
          });
        }
      },
      error: () => {
        console.log('Errror getMessageObservable');
      }
    })
  }


  onAutoChangePageChange() {
    if (this.autoChangePage) {
      this.myInterval = 60000; // 1 phut
    } else {
      this.myInterval = 0;
    }
  }
  refresh() {
    const source = interval(3600000);
    this.subscription = source.subscribe(val => this.getData());
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
  getData() {
    document.getElementById('carouselId')?.remove();
    let param: EfficiencyKanbanParam = {
      type: this.efficiencyParam.type,
      is_T5_External: false,
      factorys: this.checkAll ? this.efficiencyParam.factorys : this.efficiencyParam.factorys.filter(x => x.status),
      brands: []
    }
    this.spinner.show();
    this.service.getData(param).subscribe({
      next: (res) => {
        let flagItemNotShow = {
          "isActive": false,
          "chart": {
          },
          "categories": [
            {
              "category": []
            }
          ],
          "dataset": []
        };
        this.itemsPerSlide = Math.max(...res.map(x => x.data_By_Groups.length));
        let newDataAll = [];
        let canvasLeftPadding = "";
        let canvasRightPadding = "";
        res.forEach(group => {
          switch (param.type) {
            case "week":
              canvasLeftPadding = group.data_By_Groups?.length < 3 ? "55" : "25";
              canvasRightPadding = group.data_By_Groups?.length < 3 ? "15" : "5"
              break;
            case "month":
              canvasLeftPadding = group.data_By_Groups?.length < 3 ? "75" : "35";
              canvasRightPadding = group.data_By_Groups?.length < 3 ? "45" : "20"
              break;
            case "year":
              canvasLeftPadding = group.data_By_Groups?.length < 3 ? "75" : "25";
              canvasRightPadding = group.data_By_Groups?.length < 3 ? "35" : "20"
              break;
            default:
              canvasLeftPadding = group.data_By_Groups?.length < 3 ? "75" : "35";
              canvasRightPadding = group.data_By_Groups?.length < 3 ? "45" : "20"
              break;
          };
          group.data_By_Groups.forEach(item => {
            newDataAll.push({
              isActive: item.isActive,
              chart: {
                caption: item.title,
                captionFontSize: "22",
                showhovereffect: "1",
                numbersuffix: item.chartUnit,
                drawcrossline: "1",
                showLegend: "0",
                showXAxisValues: "0",
                paletteColors: this.colors.join(','),
                plottooltext: `$value${item.chartUnit} $seriesName`,
                theme: "candy",
                yAxisValueFontColor: "#ffffff",
                yAxisValueFontSize: "15px",
                canvasLeftPadding: canvasLeftPadding,
                canvasRightPadding: canvasRightPadding,
                chartLeftMargin: "50",
                chartBottomMargin: "5",
                divLineDashed: "1",
                divLineColor: "#6699cc",
                crosslinecolor: "#6699cc",
                showValues: item.showValues,
                forceDecimals: '1',
                decimals: '1'
              },
              categories: [
                { category: item.labels?.map(x => ({ label: x })) }
              ],
              dataset: item.data?.map((x, y) => ({
                seriesname: x.name,
                anchorBgColor: this.colors[y],
                data: x.value.map((value, valueIndex) => ({
                  value: value.toFixed(item.digits),
                  displayValue: x.actualQty ? x.actualQty[valueIndex].toString() : undefined
                }))
              }))
            });
          });
          if (group.data_By_Groups?.length < this.itemsPerSlide) {
            for (let i = 1; i <= (this.itemsPerSlide - group.data_By_Groups?.length); i++) {
              if (newDataAll.length == 1) {
                newDataAll.splice(newDataAll?.length - 1, 0, flagItemNotShow);
              } else {
                if (newDataAll[newDataAll.length - 1].isActive == newDataAll[newDataAll.length - 2].isActive)
                  newDataAll.splice(newDataAll?.length - 1, 0, flagItemNotShow);
                else
                  newDataAll.splice(newDataAll?.length, 0, flagItemNotShow);
              }
              // newDataAll.splice(newDataAll?.length - 1, 0, flagItemNotShow);
            };
          };
        });
        this.dataAll = newDataAll;
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        this.isShowChart = false;
        let time = interval(500);
        time.pipe(take(1), takeUntil(this.destroyService.destroyed$)).subscribe(() => {
          this.isShowChart = true;
          this.cdr.detectChanges();
        });
      }
    }).add(() => this.spinner.hide());
  }
  getListFactory() {
    this.service.getListFactory().subscribe({
      next: (res) => {
        this.efficiencyParam.factorys = res
      },
      error: (err) => console.log(err),
      complete: () => this.getData()
    })
  }
  getListBrand() {
    this.service.getListBrand().subscribe({
      next: (res) => {
        this.efficiencyParam.brands = res
        this.efficiencyParam.brands.push('Adidas')
        this.efficiencyParam.brands.push('Moolah Kids')
        this.efficiencyParam.brands.push('Salomon')
        console.log('this.efficiencyParam.brands :', this.efficiencyParam.brands);
        
      },
      error: (err) => console.log(err),
      complete: () => this.getData()
    })
  }
  onBrandChange(item: string) {
    console.log('item :', item);

  }
  selectedBrand: string
  changeDateRange(target: any) {
    this.efficiencyParam.type = target;
    this.getData();
  }

  changeFactory(id: string, index?: number) {
    if (id === 'all') {
      this.efficiencyParam.factorys.forEach(x => {
        x.status = false;
      });
    } else {
      if (this.efficiencyParam.factorys.every(x => x.status)) {
        this.efficiencyParam.factorys.forEach(x => {
          x.status = false;
          document.getElementById('factory_' + x.id).classList.remove('active');
        });
        interval(1).pipe(take(1), takeUntil(this.destroyService.destroyed$)).subscribe(() => {
          document.getElementById('factory_' + id).classList.remove('active');
        })
        this.checkAll = true;
      } else {
        this.checkAll = false;
      }
    }

    if (this.efficiencyParam.factorys.every(x => !x.status) && !this.checkAll) {
      index ? this.efficiencyParam.factorys[index].status = true : this.checkAll = true;
      interval(1).pipe(take(1), takeUntil(this.destroyService.destroyed$)).subscribe(() => {
        document.getElementById(!index ? 'checkAll' : 'factory_' + id).classList.add('active');
      })
    }
    this.getData();
  }
}
